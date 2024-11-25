using System;
using Photon.Realtime;
using Quantum;
using UnityEngine;

namespace Jam.Arena
{

    [Serializable, Flags]
    public enum ConnectionShutdownFlag
    {
        /// <summary>
        ///  Disconnect the connection
        /// </summary>
        Disconnect,
        /// <summary>
        /// Shutdown the runner
        /// </summary>
        ShutdownRunner,
        /// <summary>
        /// Unload the loaded scene
        /// </summary>
        UnloadScene,
        /// <summary>
        /// All flags
        /// </summary>
        All = Disconnect | ShutdownRunner | UnloadScene
    }

    public class PhotonConnectConfig
    {
        [HideInInspector] public string Username;

        /// <summary>
        /// The session that the client wants to join. Is not persisted. Use ReconnectionInformation instead to recover it between application shutdowns.
        /// </summary>
        [NonSerialized]
        public string Session;

        /// <summary>
        /// The preferred region the user selected in the menu.
        /// </summary>
        [HideInInspector]
        public string PreferredRegion;

        /// <summary>
        /// The actual region that the client will connect to.
        /// </summary>
        [NonSerialized]
        public string Region;

        /// <summary>
        /// The app version used for the Photon connection.
        /// </summary>
        [HideInInspector]
        public string AppVersion;

        /// <summary>
        /// The max player count that the user selected in the menu.
        /// </summary>
        [HideInInspector]
        public int MaxPlayerCount;

        /// <summary>
        /// The map or scene information that the user selected in the menu.
        /// </summary>
        // [HideInInspector]
        // public PhotonMenuSceneInfo Scene;

        /// <summary>
        /// Toggle to create or join-only game sessions/rooms.
        /// </summary>
        [NonSerialized]
        public bool Creating;

        /// <summary>
        /// Getter to retrieve Photon Realtime <see cref="Photon.Realtime.AppSettings"/> from <see cref="ServerSettings"/>
        /// </summary>
        public AppSettings AppSettings => ServerSettings?.AppSettings;
        /// <summary>
        /// Photon Realtime authentication settings.
        /// </summary>
        [NonSerialized]
        public AuthenticationValues AuthValues;
        /// <summary>
        /// (optional) Photon Realtime connection object for reconnection.
        /// </summary>
        [NonSerialized]
        public RealtimeClient Client;
        /// <summary>
        /// The Photon plugin name. Default is "QuantumPlugin".
        /// </summary>
        [HideInInspector]
        public string PhotonPluginName = "QuantumPlugin";
        /// <summary>
        /// The Quantum client id. This is a secret between the client and the server and should not be shared with anyone else.
        /// It does not have to be the Photon UserId for example. It's used to reclaim the same player slot after a reconnection.
        /// If null, the <see cref="AuthenticationValues.UserId"/> is used.
        /// </summary>
        [NonSerialized]
        public string QuantumClientId;
        /// <summary>
        /// Set to true to try to perform a reconnect. <see cref="ReconnectInformation"/> must be available then.
        /// </summary>
        [NonSerialized]
        public bool Reconnecting;
        /// <summary>
        /// The reconnection information used to try to reconnect quickly to the same room.
        /// </summary>
        [NonSerialized]
        public MatchmakingReconnectInformation ReconnectInformation = new QuantumReconnectInformation();
        /// <summary>
        /// The runtime config of the Quantum simulation. Every client sends theirs to the server.
        /// This is controlled by <see cref="PhotonMenuSceneInfo.RuntimeConfig"/>.
        /// </summary>
        [NonSerialized]
        public RuntimeConfig RuntimeConfig;
        /// <summary>
        /// The RuntimePlayer which are automatically added to the simulation after is started.
        /// When empty a default player is created when connecting.
        /// </summary>
        [InlineHelp]
        public RuntimePlayer[] RuntimePlayers = new RuntimePlayer[] { new RuntimePlayer() };
        /// <summary>
        /// The session config used for the simulation. Every client sends theirs to the server. If null the global config will be searched.
        /// </summary>
        [InlineHelp]
        public QuantumDeterministicSessionConfigAsset SessionConfig;
        /// <summary>
        /// The server settings file used for the connection attempts. If null the global config will be searched.
        /// </summary>
        [InlineHelp]
        public PhotonServerSettings ServerSettings;
        /// <summary>
        /// Fine-tune what internals gets disposed when the connection is terminated.
        /// </summary>
        [InlineHelp]
        public ConnectionShutdownFlag ShutdownFlags = ConnectionShutdownFlag.All;
        /// <summary>
        /// Start Quantum game in recording mode.
        /// </summary>
        [InlineHelp]
        public RecordingFlags RecordingFlags = RecordingFlags.None;
        /// <summary>
        /// Instant replay settings.
        /// </summary>
        [InlineHelp]
        public InstantReplaySettings InstantReplaySettings = InstantReplaySettings.Default;
        /// <summary>
        /// How to update the session using <see cref="SimulationUpdateTime"/>. 
        /// Default is EngineDeltaTime.
        /// </summary>
        [InlineHelp]
        public SimulationUpdateTime DeltaTimeType = SimulationUpdateTime.EngineDeltaTime;
        /// <summary>
        /// A client timeout for the Quantum start game protocol, measured in seconds.
        /// Large snapshots and/or slow webhooks could make this go above the default value of 10 sec. Configure this value appropriately.
        /// </summary>
        [InlineHelp]
        public float StartGameTimeoutInSeconds = SessionRunner.Arguments.DefaultStartGameTimeoutInSeconds;
    }

}