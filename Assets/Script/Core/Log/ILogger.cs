using System.Runtime.CompilerServices;

namespace Jam.Core
{

    public enum LoggerType
    {
        Unity,
        JLog,
        File,
    }

    public interface ILogger
    {
        public LoggerType LoggerType { get; }

        // public void Log(LogLevel level, string message);

        public void Log(LogLevel level, string file, int fileLine, object message);

        public void Dispose()
        {
        }
    }

}