using System.IO;

namespace Jam.Core
{

    public class UnityLog : ILogger
    {
        public LoggerType LoggerType => LoggerType.Unity;

        public void Log(LogLevel level, string file, int fileLine, object message)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            string msg = string.Format("[{0}:{1}] {2}", fileName, fileLine, message);

            switch (level)
            {
                case LogLevel.Info:
                    UnityEngine.Debug.Log(msg);
                    break;
                case LogLevel.Debug:
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(msg);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(msg);
                    break;
            }
        }
    }

}