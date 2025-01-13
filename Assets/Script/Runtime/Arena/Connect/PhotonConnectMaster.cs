using System;
using System.Collections.Generic;
using Jam.Core;
using Photon.Client;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;

namespace Jam.Arena
{

    public class PhotonConnectMaster : QuantumMonoBehaviour, IConnectionCallbacks
    {
        private RealtimeClient _client;

        // Game Server Connection
        // public bool ConnectToMaster()
        // {
        //     JLog.Info("ConnectToMaster");
        //     var appSettings = new AppSettings(PhotonServerSettings.Global.AppSettings);
        //     _client = new RealtimeClient(PhotonServerSettings.Global.AppSettings.Protocol);
        //
        //     // Get connect callback events etc
        //     _client.AddCallbackTarget(this);
        //     if (string.IsNullOrEmpty(appSettings.AppIdQuantum.Trim()))
        //     {
        //         JLog.Error("Missing Realtime AppId in the PhotonServerSettings");
        //         return false;
        //     }
        //
        //     if (!_client.ConnectUsingSettings(appSettings))
        //     {
        //         JLog.Error("Failed to connect to master command");
        //         return false;
        //     }
        //
        //     JLog.Info($"Attempting to connect to region {appSettings.FixedRegion}");
        //
        //     // PhotonServerSettings.Global.AppSettings;
        //     return true;
        // }

        public async void BuildRunner(RuntimeConfig runtimeConfig)
        {
            var sessionRunnerArguments = new SessionRunner.Arguments
            {
                // The runner factory is the glue between the Quantum.Runner and Unity
                RunnerFactory = QuantumRunnerUnityFactory.DefaultFactory,
                // Creates a default version of `QuantumGameStartParameters`
                GameParameters = QuantumRunnerUnityFactory.CreateGameParameters,
                // A secret user id that is for example used to reserved player slots to reconnect into a running session
                ClientId = _client.UserId,
                // The player data
                RuntimeConfig = runtimeConfig,
                // The session config loaded from the Unity asset tagged as `QuantumDefaultGlobal`
                SessionConfig = QuantumDeterministicSessionConfigAsset.DefaultConfig,
                // GameMode has to be multiplayer for online sessions
                GameMode = DeterministicGameMode.Multiplayer,
                // The number of player that the session is running for, in this case we use the code-generated max possible players for the Quantum simulation
                PlayerCount = Input.MAX_COUNT,
                // A timeout to fail the connection logic and Quantum protocol
                StartGameTimeoutInSeconds = 10,
                // The communicator will take over the network handling after the simulation has started
                Communicator = new QuantumNetworkCommunicator(_client),
            };

            // This method completes when the client has successfully joined the online session
            QuantumRunner runner = (QuantumRunner)await SessionRunner.StartAsync(sessionRunnerArguments);

            QuantumCallback.Subscribe(this, (CallbackLocalPlayerAddConfirmed c) => OnLocalPlayerAddConfirmed(c));
            QuantumCallback.Subscribe(this, (CallbackLocalPlayerRemoveConfirmed c) => OnLocalPlayerRemoveConfirmed(c));
            QuantumCallback.Subscribe(this, (CallbackLocalPlayerAddFailed c) => OnLocalPlayerAddFailed(c));
            QuantumCallback.Subscribe(this, (CallbackLocalPlayerRemoveFailed c) => OnLocalPlayerRemoveFailed(c));

            RuntimePlayer player = new RuntimePlayer();
            runner.Game.AddPlayer(player);
        }

        private async void Disconnect()
        {
            // Signal all runners to shutdown and wait until each one has disconnected
            await QuantumRunner.ShutdownAllAsync();

            // OR just signal their shutdown
            QuantumRunner.ShutdownAll();
        }

        private void OnLocalPlayerAddConfirmed(CallbackLocalPlayerAddConfirmed callback)
        {
            JLog.Info($"OnLocalPlayerAddConfirmed {callback}");
        }

        private void OnLocalPlayerRemoveConfirmed(CallbackLocalPlayerRemoveConfirmed callback)
        {
            JLog.Info($"OnLocalPlayerRemoveConfirmed {callback}");
        }

        private void OnLocalPlayerAddFailed(CallbackLocalPlayerAddFailed callback)
        {
            JLog.Error($"OnLocalPlayerAddFailed {callback}");
        }

        private void OnLocalPlayerRemoveFailed(CallbackLocalPlayerRemoveFailed callback)
        {
            JLog.Error($"OnLocalPlayerRemoveFailed {callback}");
        }

        // public async void Connect()
        // {
        //     EnterRoomArgs customRoomProperties = CreateEnterRoomArgs("room-name");
        //
        //     var connectionArguments = new MatchmakingArguments
        //     {
        //         // The Photon application settings
        //         PhotonSettings = PhotonServerSettings.Global.AppSettings,
        //         //Will be configured as "EnterRoomArgs.RoomOptions.PlayerTtl" when creating a Photon room
        //         PlayerTtlInSeconds = 10,
        //         //Will be configured as "EnterRoomArgs.RoomOptions.EmptyRoomTtl" when creating a Photon room
        //         EmptyRoomTtlInSeconds = 10,
        //         // Will be configured as "EnterRoomArgs.RoomOptions.RoomName when creating a Photon room.
        //         RoomName = "room-name",
        //         // The maximum number of clients for the room, in this case we use the code-generated max possible players for the Quantum simulation
        //         MaxPlayers = Input.MAX_COUNT,
        //         // Configure if the connect request can also create rooms or if it only tries to join
        //         CanOnlyJoin = false,
        //         // Custom room properties that are configured as "EnterRoomArgs.RoomOptions.CustomRoomProperties"
        //         CustomProperties = customRoomProperties.RoomOptions.CustomRoomProperties,
        //         // Async configuration that include TaskFactory and global cancellation support. If null then "AsyncConfig.Global" is used
        //         AsyncConfig = null,
        //         //Provide authentication values for the Photon server connection. Use this in conjunction with custom authentication. This field is created when "UserId" is set
        //         AuthValues = null, //customAuthValues,
        //         // The plugin to request from the Photon cloud
        //         PluginName = "QuantumPlugin",
        //         //Optional object to save and load reconnect information
        //         ReconnectInformation = new MatchmakingReconnectInformation(),
        //         //Optional Realtime lobby to use for matchmaking
        //         Lobby = new TypedLobby(),
        //         // This sets the AuthValues and should be replaced with the custom authentication
        //         UserId = Guid.NewGuid().ToString(),
        //     };
        //
        //     // This line connects to the Photon cloud and performs matchmaking based on the arguments to finally enter a room.
        //     RealtimeClient Client = await MatchmakingExtensions.ConnectToRoomAsync(connectionArguments);
        // }
        //
        // private EnterRoomArgs CreateEnterRoomArgs(string roomName)
        // {
        //     EnterRoomArgs args = new EnterRoomArgs();
        //     args.RoomName = roomName;
        //     args.RoomOptions = new RoomOptions();
        //     args.RoomOptions.IsVisible = true;
        //     args.RoomOptions.MaxPlayers = 6;
        //     args.RoomOptions.Plugins = new string[] { "QuantumPlugin" };
        //     args.RoomOptions.CustomRoomProperties = new PhotonHashtable() { { "MapGuid", 1 } };
        //     args.RoomOptions.PlayerTtl = PhotonServerSettings.Global.PlayerTtlInSeconds * 1000;
        //     args.RoomOptions.EmptyRoomTtl = PhotonServerSettings.Global.EmptyRoomTtlInSeconds * 1000;
        //     return args;
        // }

        public void OnConnected()
        {
            JLog.Info($"OnConnected UserId: {_client.UserId}");
        }

        public void OnConnectedToMaster()
        {
            JLog.Info($"OnConnectedToMaster in region: {_client.CurrentRegion}");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            JLog.Info($"OnDisconnected cause: {cause}");
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            JLog.Info($"OnRegionListReceived");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            JLog.Info($"OnCustomAuthenticationResponse");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            JLog.Info($"OnCustomAuthenticationFailed {debugMessage}");
        }
    }

}