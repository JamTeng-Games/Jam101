using System;
using System.IO;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using UnityEngine;

namespace Jam.Runtime.Quantum_
{

    [Serializable]
    public class QuantumConnectArgs
    {
        [HideInInspector] public string Username;

        /// The session that the client wants to join. Is not persisted.
        /// Use ReconnectionInformation instead to recover it between application shutdowns.
        [NonSerialized]
        public string Session;
        [HideInInspector] public string PreferredRegion;
        [NonSerialized] public string Region;
        [HideInInspector] public string AppVersion;
        public int MaxPlayerCount;
        [NonSerialized] public bool Creating;         // Toggle to create or join-only game sessions/rooms.
        [NonSerialized] public RealtimeClient Client; // For reconnection
        [ScenePath] public string ScenePath;
        [NonSerialized] public AuthenticationValues AuthValues; // Photon Realtime authentication settings.
        [NonSerialized] public bool Reconnecting;               // Set to true to try to perform a reconnect
        /// The Quantum client id. This is a secret between the client and the server and should not be shared with anyone else.
        /// It does not have to be the Photon UserId for example. It's used to reclaim the same player slot after a reconnection.
        /// If null, the <see cref="AuthenticationValues.UserId"/> is used.
        [NonSerialized]
        public string QuantumClientId;
        [HideInInspector]
        public string PhotonPluginName = "QuantumPlugin"; // The Photon plugin name. Default is "QuantumPlugin".
        /// The reconnection information used to try to reconnect quickly to the same room.
        [NonSerialized]
        public MatchmakingReconnectInformation ReconnectInformation = new QuantumReconnectInformation();

        public RuntimeConfig RuntimeConfig;
        /// The RuntimePlayer which are automatically added to the simulation after is started.
        /// When empty a default player is created when connecting.
        // public RuntimePlayer[] RuntimePlayers = new RuntimePlayer[] { new RuntimePlayer() };
        public RuntimePlayer RuntimePlayer = new RuntimePlayer();
        public QuantumDeterministicSessionConfigAsset SessionConfig;
        public PhotonServerSettings ServerSettings;
        /// Fine-tune what internals gets disposed when the connection is terminated.
        public QuantumConnectionShutdownFlag ShutdownFlags = QuantumConnectionShutdownFlag.All;
        public DeterministicGameMode GameMode = DeterministicGameMode.Multiplayer;
        public RecordingFlags RecordingFlags = RecordingFlags.None; // Start Quantum game in recording mode.
        public InstantReplaySettings InstantReplaySettings = InstantReplaySettings.Default;
        public SimulationUpdateTime DeltaTimeType = SimulationUpdateTime.EngineDeltaTime;
        /// A client timeout for the Quantum start game protocol, measured in seconds.
        /// Large snapshots and/or slow webhooks could make this go above the default value of 10 sec. Configure this value appropriately.
        public float StartGameTimeoutInSeconds = SessionRunner.Arguments.DefaultStartGameTimeoutInSeconds;
        public AppSettings AppSettings => ServerSettings?.AppSettings;
        public AssetRef<Map> Map => RuntimeConfig.Map;
        public AssetRef<SystemsConfig> SystemsConfig => RuntimeConfig.SystemsConfig;
        public string SceneName => ScenePath == null ? null : Path.GetFileNameWithoutExtension(ScenePath);
    }

    [Serializable, Flags]
    public enum QuantumConnectionShutdownFlag
    {
        Disconnect,
        ShutdownRunner,
        UnloadScene,
        All = Disconnect | ShutdownRunner | UnloadScene
    }

    [Serializable]
    public struct QuantumOnlineRegion
    {
        /// <summary>
        /// Photon region code.
        /// </summary>
        public string Code;
        /// <summary>
        /// Last ping result.
        /// </summary>
        public int Ping;
    }

}