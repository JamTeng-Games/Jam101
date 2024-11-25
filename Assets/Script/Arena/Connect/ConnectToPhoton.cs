using System;
using System.Collections.Generic;
using Jam.Core;
using Photon.Client;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

namespace Jam.Arena
{

    public class ConnectToPhoton : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback
    {
        public static ConnectToPhoton Instance { get; private set; }

        private RealtimeClient _client;

        private long _mapGuid = 0;

        private bool _isQuickPlayMode = true;

        enum PhotonEventCode : byte
        {
            GameList = 230,
            GameListUpdate = 229,
            QueueState = 228,
            Match = 227,
            AppStats = 226,
            LobbyStats = 224,
            AzureNodeInfo = 210,
            Join = 255,
            Leave = 254,
            PropertiesChanged = 253,
            SetProperties = 253,
            ErrorInfo = 251,
            CacheSliceChanged = 250,
            AuthEvent = 223,
        }

        private void Awake()
        {
            Instance = this;
        }

        public void Tick(float dt)
        {
            _client?.Service();
        }

        public void QuickPlay()
        {
            if (_client != null && _client.IsConnected)
            {
                JoinRandomOrCreateRoom();
            }
            else
            {
                ConnectToMaster();
            }
        }

        public bool ConnectToMaster()
        {
            JLog.Info("ConnectToMaster");
            var appSettings = new AppSettings(PhotonServerSettings.Global.AppSettings);
            _client = new RealtimeClient(PhotonServerSettings.Global.AppSettings.Protocol);

            // Get connect callback events etc
            _client.AddCallbackTarget(this);
            if (string.IsNullOrEmpty(appSettings.AppIdQuantum.Trim()))
            {
                JLog.Error("Missing Realtime AppId in the PhotonServerSettings");
                return false;
            }

            if (!_client.ConnectUsingSettings(appSettings))
            {
                JLog.Error("Failed to connect to master command");
                return false;
            }

            JLog.Info($"Attempting to connect to region {appSettings.FixedRegion}");

            // PhotonServerSettings.Global.AppSettings;
            return true;
        }

        private void JoinRandomOrCreateRoom()
        {
            if (_client == null || !_client.IsConnected)
            {
                JLog.Error("JoinRandomRoom failed, client is not connected");
                return;
            }

            if (_client.CurrentRoom != null)
            {
                JLog.Error("JoinRandomRoom failed, client is already in a room");
                return;
            }

            // _connectionHandler.Client = _client;
            // _connectionHandler.StartFallbackSendAckThread();

            // Pick the first map we can find
            var allMapRes = UnityEngine.Resources.LoadAll<Map>("");

            _mapGuid = allMapRes[1].Guid.Value;
            JLog.Info($"Using map long guid: {_mapGuid}, GUID: {allMapRes[1].Guid}");

            // Setup room properties
            EnterRoomArgs enterRoomArgs = CreateEnterRoomArgs("");
            JoinRandomRoomArgs joinRandomRoomArgs = new JoinRandomRoomArgs();
            if (!_client.OpJoinRandomOrCreateRoom(joinRandomRoomArgs, enterRoomArgs))
            {
                JLog.Error("Unable to join random room or create room");
                return;
            }
            JLog.Info($"Join random room or create room");
        }

        private EnterRoomArgs CreateEnterRoomArgs(string roomName)
        {
            EnterRoomArgs args = new EnterRoomArgs();
            args.RoomName = roomName;
            args.RoomOptions = new RoomOptions();
            args.RoomOptions.IsVisible = true;
            args.RoomOptions.MaxPlayers = 6;
            args.RoomOptions.Plugins = new string[] { "QuantumPlugin" };
            args.RoomOptions.CustomRoomProperties = new PhotonHashtable() { { "MapGuid", _mapGuid } };
            args.RoomOptions.PlayerTtl = PhotonServerSettings.Global.PlayerTtlInSeconds * 1000;
            args.RoomOptions.EmptyRoomTtl = PhotonServerSettings.Global.EmptyRoomTtlInSeconds * 1000;
            return args;
        }

        public void StartGame()
        {
            if (!_client.OpRaiseEvent((byte)PhotonEventCode.Match, null,
                                      new RaiseEventArgs() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable))
            {
                JLog.Error("Unable to start game");
                return;
            }

            JLog.Info("Starting game");
        }

        public void StartQuantumGame()
        {
            if (QuantumRunner.Default != null)
            {
                JLog.Warning($"Another QuantumRunner '{QuantumRunner.Default.Id}' has prevented starting the game");
                return;
            }

            RuntimeConfig runtimeConfig = new RuntimeConfig();
            runtimeConfig.Map.Id = _mapGuid;

            var args = new SessionRunner.Arguments()
            {
                RecordingFlags = RecordingFlags.All,
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
                PlayerCount = _client.CurrentRoom.MaxPlayers,
                // A timeout to fail the connection logic and Quantum protocol
                StartGameTimeoutInSeconds = 10f,
                // The communicator will take over the network handling after the simulation has started
                Communicator = new QuantumNetworkCommunicator(_client),
                FrameData = null,
            };

            JLog.Info(
                $"Starting QuantumRunner with clientId {_client.UserId} and map guid {_mapGuid}. Loacl player count {args.PlayerCount}");

            QuantumRunner.StartGame(args);
            // QuantumRunner runner = (QuantumRunner)await SessionRunner.StartAsync(args);
        }

        #region Connection Callbacks

        public void OnConnected()
        {
            JLog.Info($"OnConnected UserId: {_client.UserId}");
        }

        public void OnConnectedToMaster()
        {
            JLog.Info($"OnConnectedToMaster in region: {_client.CurrentRegion}");
            JoinRandomOrCreateRoom();
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

        #endregion

        #region Room Callbacks

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            JLog.Info("OnFriendListUpdate");
        }

        public void OnCreatedRoom()
        {
            JLog.Info("OnCreatedRoom");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            JLog.Error($"OnCreateRoomFailed return code: {returnCode}, message: {message}");
        }

        public void OnJoinedRoom()
        {
            JLog.Info($"OnJoinedRoom");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            JLog.Error($"OnJoinRoomFailed return code: {returnCode}, message: {message}");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            JLog.Error($"OnJoinRandomFailed return code: {returnCode}, message: {message}");
        }

        public void OnLeftRoom()
        {
            JLog.Info($"OnLeftRoom");
        }

        #endregion

        #region IOnEventCallback

        public void OnEvent(EventData photonEvent)
        {
            JLog.Info($"Photon event received code {photonEvent.Code}");

            switch (photonEvent.Code)
            {
                case (byte)PhotonEventCode.Join:
                    if (!_client.CurrentRoom.CustomProperties.TryGetValue("MapGuid", out object mapGuidValue))
                    {
                        JLog.Error("Failed to get map Guid, disconnecting");
                        _client.Disconnect();
                        return;
                    }
                    if (_client.LocalPlayer.IsMasterClient)
                    {
                        _client.CurrentRoom.IsVisible = false;
                        _client.CurrentRoom.IsOpen = false;
                    }
                    StartQuantumGame();
                    break;
                default: break;
            }
        }

        #endregion
    }

}