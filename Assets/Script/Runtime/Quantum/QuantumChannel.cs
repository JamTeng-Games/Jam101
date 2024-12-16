using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using Jam.Runtime.Scene_;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using UnityEngine;
using Input = Quantum.Input;
using Random = System.Random;

namespace Jam.Runtime.Quantum_
{

    public class QuantumChannel : QuantumMonoBehaviour
    {
        [SerializeField] QuantumConnectArgs _connectArgs;
        public bool IsReconnectionCheckEnabled;

        private CancellationTokenSource _cancellation;
        private CancellationTokenSource _linkedCancellation;
        private string _loadedScene;
        private QuantumConnectionShutdownFlag _shutdownFlags;
        private DisconnectCause _disconnectCause;
        private IDisposable _disconnectSubscription;
        private RealtimeClient _client;

        public RealtimeClient Client => _client;
        public string SessionName => Client?.CurrentRoom?.Name; // Room name
        public string Region => Client?.CurrentRegion;
        public string AppVersion => Client?.AppSettings?.AppVersion;
        public int MaxPlayerCount => Client?.CurrentRoom != null ? Client.CurrentRoom.MaxPlayers : 0;
        public bool IsConnected => Client == null ? false : Client.IsConnected;
        public int Ping => Runner?.Session != null ? Runner.Session.Stats.Ping : 0;

        public QuantumRunner Runner { get; private set; }

        // protected virtual async void OnPlayButtonPressed() {
        //     ConnectionArgs.Session = null;
        //     ConnectionArgs.Creating = false;
        //     ConnectionArgs.Region = ConnectionArgs.PreferredRegion;
        //
        //     Controller.Show<QuantumMenuUILoading>();
        //
        //     var result = await Connection.ConnectAsync(ConnectionArgs);
        //
        //     await Controller.HandleConnectionResult(result, this.Controller);
        // }

        public async Task<QuantumConnectResult> ConnectAsync(QuantumConnectArgs connectArgs = null)
        {
            if (connectArgs == null)
                connectArgs = _connectArgs;
            PatchConnectArgs(connectArgs);
            if (_cancellation != null)
                throw new Exception("Connection instance still in use");

            // CONNECT ---------------------------------------------------------------

            // Cancellation is used to stop the connection process at any time.
            _cancellation = new CancellationTokenSource();
            _linkedCancellation = AsyncSetup.CreateLinkedSource(_cancellation.Token);
            _shutdownFlags = connectArgs.ShutdownFlags;
            _disconnectCause = DisconnectCause.None;

            var arguments = new MatchmakingArguments
            {
                PhotonSettings =
                    new AppSettings(connectArgs.AppSettings)
                    {
                        AppVersion = connectArgs.AppVersion, FixedRegion = connectArgs.PreferredRegion
                    },
                ReconnectInformation = connectArgs.ReconnectInformation,
                EmptyRoomTtlInSeconds = connectArgs.ServerSettings.EmptyRoomTtlInSeconds,
                PlayerTtlInSeconds = connectArgs.ServerSettings.PlayerTtlInSeconds,
                MaxPlayers = connectArgs.MaxPlayerCount,
                RoomName = connectArgs.Session,
                CanOnlyJoin = !string.IsNullOrEmpty(connectArgs.Session) && !connectArgs.Creating,
                PluginName = connectArgs.PhotonPluginName,
                AsyncConfig = new AsyncConfig()
                {
                    TaskFactory = AsyncConfig.CreateUnityTaskFactory(), CancellationToken = _linkedCancellation.Token
                },
                NetworkClient = connectArgs.Client,
                AuthValues = connectArgs.AuthValues,
            };

            // Connect to Photon
            try
            {
                if (!connectArgs.Reconnecting)
                {
                    JLog.Info("Connecting..");
                    _client = await MatchmakingExtensions.ConnectToRoomAsync(arguments);
                }
                else
                {
                    JLog.Info("Reconnecting..");
                    _client = await MatchmakingExtensions.ReconnectToRoomAsync(arguments);
                }
            }
            catch (Exception e)
            {
                JLog.Exception(e);
                return new QuantumConnectResult
                {
                    FailReason = AsyncConfig.Global.IsCancellationRequested ? QuantumConnectFailReason.ApplicationQuit :
                                 _disconnectCause == DisconnectCause.None   ? QuantumConnectFailReason.RunnerFailed :
                                                                              QuantumConnectFailReason.Disconnect,
                    DisconnectCause = (int)_disconnectCause,
                    DebugMessage = e.Message,
                    WaitForCleanup = CleanupAsync()
                };
            }

            // Save region summary
            if (!string.IsNullOrEmpty(Client.SummaryToCache))
            {
                connectArgs.ServerSettings.BestRegionSummary = Client.SummaryToCache;
            }

            //  Make sure to notice socket disconnects during the rest of the connection/start process
            _disconnectSubscription = Client.CallbackMessage.ListenManual<OnDisconnectedMsg>(m =>
            {
                if (_cancellation != null && !_cancellation.IsCancellationRequested)
                {
                    _disconnectCause = m.cause;
                    _cancellation.Cancel();
                }
            });

            // LOAD SCENE ---------------------------------------------------------------

            var preloadMap = false;
            if (connectArgs.RuntimeConfig != null &&
                connectArgs.RuntimeConfig.Map.Id.IsValid &&
                connectArgs.RuntimeConfig.SimulationConfig.Id.IsValid)
            {
                if (QuantumUnityDB.TryGetGlobalAsset(connectArgs.RuntimeConfig.SimulationConfig,
                                                     out SimulationConfig simulationConfigAsset))
                {
                    // Only preload the scene if SimulationConfig.AutoLoadSceneFromMap is not enabled.
                    // Caveat: preloading the scene here only works if the runtime config is not expected to change (e.g. by other clients/random matchmaking or webhooks)
                    preloadMap = simulationConfigAsset.AutoLoadSceneFromMap == SimulationConfig.AutoLoadSceneFromMapMode.Disabled;
                }
            }

            if (preloadMap)
            {
                JLog.Info("Loading scene..");

                if (!QuantumUnityDB.TryGetGlobalAsset(connectArgs.RuntimeConfig.Map, out Map map))
                {
                    return new QuantumConnectResult
                    {
                        FailReason = QuantumConnectFailReason.MapNotFound,
                        DebugMessage = $"Requested map {connectArgs.RuntimeConfig.Map} not found.",
                        WaitForCleanup = CleanupAsync()
                    };
                }

                using (new ConnectionServiceScope(Client))
                {
                    try
                    {
                        // Load Unity scene async
                        await SceneMgr.LoadSceneAsync(map.Scene);
                        SceneMgr.SetActiveScene(SceneMgr.GetSceneByName(map.Scene));
                        _loadedScene = map.Scene;

                        // Check if cancellation was triggered while loading the map
                        if (_linkedCancellation.Token.IsCancellationRequested)
                        {
                            throw new TaskCanceledException();
                        }
                    }
                    catch (Exception e)
                    {
                        JLog.Exception(e);
                        return new QuantumConnectResult
                        {
                            FailReason =
                                AsyncConfig.Global.IsCancellationRequested
                                    ? QuantumConnectFailReason.ApplicationQuit
                                    : _disconnectCause == DisconnectCause.None
                                        ? QuantumConnectFailReason.RunnerFailed
                                        : QuantumConnectFailReason.Disconnect,
                            DisconnectCause = (int)_disconnectCause,
                            DebugMessage = e.Message,
                            WaitForCleanup = CleanupAsync()
                        };
                    }
                }
            }

            // START GAME ---------------------------------------------------------------
            JLog.Info("Starting game..");

            var sessionRunnerArguments = new SessionRunner.Arguments
            {
                RunnerFactory = QuantumRunnerUnityFactory.DefaultFactory,
                GameParameters = QuantumRunnerUnityFactory.CreateGameParameters,
                ClientId =
                    // Use client id from connection args first
                    !string.IsNullOrEmpty(connectArgs.QuantumClientId) ? connectArgs.QuantumClientId :
                    // Then chose the user id that was returned by the authentication
                    !string.IsNullOrEmpty(Client.UserId) ? Client.UserId :
                                                           // Or create a random id
                                                           Guid.NewGuid().ToString(),
                RuntimeConfig = connectArgs.RuntimeConfig,
                SessionConfig = connectArgs.SessionConfig?.Config ?? QuantumDeterministicSessionConfigAsset.DefaultConfig,
                GameMode = connectArgs.GameMode,
                PlayerCount = connectArgs.MaxPlayerCount,
                Communicator = new QuantumNetworkCommunicator(Client),
                CancellationToken = _linkedCancellation.Token,
                RecordingFlags = connectArgs.RecordingFlags,
                InstantReplaySettings = connectArgs.InstantReplaySettings,
                DeltaTimeType = connectArgs.DeltaTimeType,
                StartGameTimeoutInSeconds = connectArgs.StartGameTimeoutInSeconds,
            };

            // QuantumMppm.MainEditor?.Send(new QuantumMenuMppmJoinCommand() {
            //     AppVersion = connectArgs.AppVersion,
            //     Session = Client.CurrentRoom.Name,
            //     Region = Client.CurrentRegion,
            // });

            // Register to plugin disconnect messages to display errors
            string pluginDisconnectReason = null;
            var pluginDisconnectListener =
                QuantumCallback.SubscribeManual<CallbackPluginDisconnect>(m => pluginDisconnectReason = m.Reason);

            try
            {
                // Start Quantum and wait for the start protocol to complete
                Runner = (QuantumRunner)await SessionRunner.StartAsync(sessionRunnerArguments);
            }
            catch (Exception e)
            {
                pluginDisconnectListener.Dispose();
                JLog.Exception(e);
                return new QuantumConnectResult
                {
                    FailReason = DetermineFailReason(_disconnectCause, pluginDisconnectReason),
                    DisconnectCause = (int)_disconnectCause,
                    DebugMessage = pluginDisconnectReason ?? e.Message,
                    WaitForCleanup = CleanupAsync()
                };
            }

            pluginDisconnectListener.Dispose();
            _cancellation.Dispose();
            _cancellation = null;
            _linkedCancellation.Dispose();
            _linkedCancellation = null;
            _disconnectSubscription.Dispose();
            _disconnectSubscription = null;

            Runner.Game.AddPlayer(connectArgs.RuntimePlayers[0]);

            return new QuantumConnectResult { Success = true };
        }

        public Task DisconnectAsync(QuantumConnectFailReason reason)
        {
            if (_cancellation != null)
            {
                // Cancel connection logic and let the code handle cancel errors
                _cancellation.Cancel();
                return Task.CompletedTask;
            }
            else
            {
                if (reason == QuantumConnectFailReason.UserRequest)
                {
                    QuantumReconnectInformation.Reset();
                }

                // Stop the running game
                return CleanupAsync();
            }
        }

        public async Task<List<QuantumOnlineRegion>> RequestAvailableOnlineRegionsAsync(QuantumConnectArgs connectArgs)
        {
            // TODO: fix when implemented in realtime.
            try
            {
                var client = connectArgs.Client ?? new RealtimeClient();
                var appSettings = connectArgs.AppSettings ?? PhotonServerSettings.Global.AppSettings;
                var regionHandler = await client.ConnectToNameserverAndWaitForRegionsAsync(appSettings);
                return regionHandler.EnabledRegions
                                    .Select(r => new QuantumOnlineRegion { Code = r.Code, Ping = r.Ping })
                                    .ToList();
            }
            catch (Exception e)
            {
                JLog.Exception(e);
                return null;
            }
        }

        private async Task ProcessDisconnect(string disconnectReason)
        {
            var reconnectInformation = QuantumReconnectInformation.Load();
            if (IsReconnectionCheckEnabled && reconnectInformation != null && !reconnectInformation.HasTimedOut)
            {
                // If none set in the connection args, save the client object to use for reconnection
                var client = _connectArgs.Client == null ? Client : null;

                await DisconnectAsync(QuantumConnectFailReason.Disconnect);

                // await Task.WhenAll(
                //     DisconnectAsync(ConnectFailReason.Disconnect),
                //     Controller.PopupAsync($"Network error '{disconnectReason}'. Trying to reconnect.", "Connection Error"));

                JLog.Error($"Network error '{disconnectReason}'. Trying to reconnect.");
                // Controller.Show<QuantumMenuUILoading>();
                _connectArgs.Session = null;
                _connectArgs.Creating = false;
                _connectArgs.Client = client;
                _connectArgs.Reconnecting = true;
                _connectArgs.ReconnectInformation = reconnectInformation;

                var result = await ConnectAsync(_connectArgs);

                if (client != null)
                {
                    // If it was just set for this reconnection attempts, forget the client again
                    _connectArgs.Client = null;
                }

                await HandleConnectionResult(result);
            }
            else
            {
                await DisconnectAsync(QuantumConnectFailReason.Disconnect);
                JLog.Error($"Network error '{disconnectReason}'");

                // await Task.WhenAll(
                //     Connection.DisconnectAsync(ConnectFailReason.Disconnect),
                //     Controller.PopupAsync($"Network error '{disconnectReason}'", "Connection Error"));
                // Controller.Show<QuantumMenuUIMain>();
            }
        }

        private async Task CleanupAsync()
        {
            _cancellation?.Dispose();
            _cancellation = null;
            _linkedCancellation?.Dispose();
            _linkedCancellation = null;
            _disconnectSubscription?.Dispose();
            _disconnectSubscription = null;

            if (Runner != null && (_shutdownFlags & QuantumConnectionShutdownFlag.ShutdownRunner) >= 0)
            {
                try
                {
                    Runner.Game.RemovePlayer();
                    if (AsyncConfig.Global.IsCancellationRequested)
                    {
                        Runner.Shutdown();
                    }
                    else
                    {
                        await Runner.ShutdownAsync();
                    }
                }
                catch (Exception e)
                {
                    JLog.Exception(e);
                }
            }
            Runner = null;

            if (Client != null && (_shutdownFlags & QuantumConnectionShutdownFlag.Disconnect) >= 0)
            {
                try
                {
                    if (AsyncConfig.Global.IsCancellationRequested)
                    {
                        Client.Disconnect();
                    }
                    else
                    {
                        await Client.DisconnectAsync();
                    }
                }
                catch (Exception e)
                {
                    JLog.Exception(e);
                }
            }
            _client = null;

            if (!string.IsNullOrEmpty(_loadedScene) &&
                (_shutdownFlags & QuantumConnectionShutdownFlag.ShutdownRunner) >= 0 &&
                !AsyncConfig.Global.IsCancellationRequested)
            {
                try
                {
                    // await SceneMgr.UnloadSceneAsync(_loadedScene);
                    G.Event.Send(GlobalEventId.ExitCombat);
                }
                catch (Exception e)
                {
                    JLog.Exception(e);
                }
            }
            _loadedScene = null;
        }

        private static void PatchConnectArgs(QuantumConnectArgs connectArgs)
        {
            // set global configs for ServerSettings and SessionConfig when null
            connectArgs.ServerSettings = connectArgs.ServerSettings ?? PhotonServerSettings.Global;
            connectArgs.SessionConfig = connectArgs.SessionConfig ?? QuantumDeterministicSessionConfigAsset.Global;

            // limit player count
            connectArgs.MaxPlayerCount = Math.Min(connectArgs.MaxPlayerCount, Input.MaxCount);

            // runtime config alterations
            {
                // always re roll the seed if 0.
                if (connectArgs.RuntimeConfig.Seed == 0)
                    connectArgs.RuntimeConfig.Seed = Guid.NewGuid().GetHashCode();

                // if SimulationConfig not set, try to get from global default configs
                if (!connectArgs.RuntimeConfig.SimulationConfig.Id.IsValid)
                {
                    if (QuantumDefaultConfigs.TryGetGlobal(out var defaultConfigs))
                        connectArgs.RuntimeConfig.SimulationConfig = defaultConfigs.SimulationConfig;
                }
            }

            // runtime player alterations
            {
                if (!string.IsNullOrEmpty(connectArgs.Username) && connectArgs.RuntimePlayers?.Length > 0)
                {
                    // Always overwrite nickname, set ConnectionArgs.Username to null to avoid
                    connectArgs.RuntimePlayers[0].PlayerNickname = connectArgs.Username;
                }
            }

            // auth values
            if (connectArgs.AuthValues == null ||
                (connectArgs.AuthValues.AuthType == CustomAuthenticationType.None &&
                 string.IsNullOrEmpty(connectArgs.AuthValues.UserId)))
            {
                // Set the UserId to the username if no authtype is set
                connectArgs.AuthValues ??= new AuthenticationValues();
                connectArgs.AuthValues.UserId = $"{connectArgs.Username}({new Random().Next(99999999):00000000}";
            }
        }

        /// <summary>
        /// Match errors to one error number.
        /// </summary>
        /// <param name="disconnectCause">Photon disconnect reason</param>
        /// <param name="pluginDisconnectReason">Plugin disconnect message</param>
        /// <returns></returns>
        private static QuantumConnectFailReason DetermineFailReason(DisconnectCause disconnectCause,
                                                                    string pluginDisconnectReason)
        {
            if (AsyncConfig.Global.IsCancellationRequested)
            {
                return QuantumConnectFailReason.ApplicationQuit;
            }

            switch (disconnectCause)
            {
                case DisconnectCause.None: return QuantumConnectFailReason.RunnerFailed;
                case DisconnectCause.DisconnectByClientLogic:
                    if (!string.IsNullOrEmpty(pluginDisconnectReason))
                    {
                        return QuantumConnectFailReason.PluginError;
                    }
                    return QuantumConnectFailReason.Disconnect;
                default: return QuantumConnectFailReason.Disconnect;
            }
        }

        public virtual async Task HandleConnectionResult(QuantumConnectResult result)
        {
            if (result.CustomResultHandling)
                return;

            if (result.Success)
            {
                JLog.Info("Connection successful");
                // controller.Show<QuantumMenuUIGameplay>();
            }
            else if (result.FailReason != QuantumConnectFailReason.ApplicationQuit)
            {
                // var popup = controller.PopupAsync(result.DebugMessage, "Connection Failed");
                JLog.Info($"{result.DebugMessage} Connection Failed");
                if (result.WaitForCleanup != null)
                {
                    await result.WaitForCleanup;
                }
                else
                {
                    // await popup;
                }
                // controller.Show<QuantumMenuUIMain>();
            }
        }
    }

}