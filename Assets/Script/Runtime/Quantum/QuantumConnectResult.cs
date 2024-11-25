using System.Threading.Tasks;

namespace Jam.Runtime.Quantum_
{

    public class QuantumConnectResult
    {
        public bool Success;
        public QuantumConnectFailReason FailReason;
        /// <summary>
        /// Another custom code that can be filled by out by RealtimeClient.DisconnectCause for example.
        /// </summary>
        public int DisconnectCause;
        /// <summary>
        /// A debug message.
        /// </summary>
        public string DebugMessage;
        /// Set to true to disable all error handling by the menu.
        public bool CustomResultHandling;
        /// An optional task to signal the menu to wait until cleanup operation have completed (e.g. level unloading).
        public Task WaitForCleanup;

        /// Creates a successful result.
        /// <returns>Initialized result object</returns>
        public static QuantumConnectResult Ok()
        {
            return new QuantumConnectResult { Success = true };
        }

        /// Create a failed result.
        /// <param name="failReason">Fail reason <see cref="FailReason"/></param>
        /// <param name="debugMessage">Debug message</param>
        /// <param name="waitForCleanup">Should the receiving code wait until the connection or the scene has been cleaned up.</param>
        /// <returns></returns>
        public static QuantumConnectResult Fail(QuantumConnectFailReason failReason, string debugMessage = null, Task waitForCleanup = null)
        {
            return new QuantumConnectResult
            {
                Success = false,
                FailReason = failReason,
                DebugMessage = debugMessage,
                WaitForCleanup = waitForCleanup
            };
        }
    }

    public enum QuantumConnectFailReason
    {
        /// <summary>
        /// No reason code available.
        /// </summary>
        None = 0,
        /// <summary>
        /// User requested cancellation or disconnect.
        /// </summary>
        UserRequest = 1,
        /// <summary>
        /// App or Editor closed
        /// </summary>
        ApplicationQuit = 2,
        /// <summary>
        /// Connection disconnected.
        /// </summary>
        Disconnect = 3,
        /// <summary>
        /// The connection to Photon servers failed.
        /// </summary>
        ConnectingFailed = 10,
        /// <summary>
        /// The Quantum map asset was not found.
        /// </summary>
        MapNotFound = 11,
        /// <summary>
        /// The scene loading failed.
        /// </summary>
        LoadingFailed = 12,
        /// <summary>
        /// Starting the runner failed.
        /// </summary>
        RunnerFailed = 13,
        /// <summary>
        /// Plugin disconnected.
        /// </summary>
        PluginError = 14,
    }

}