using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace J.Core
{

    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Debug = 3,
        Info = 4,
        All = 9,
    }

    public static class JLog
    {
        private static ILogger _logger;
        private static ILogger _fileLogger;
        private static bool _enabled = true;
        private static bool _fileLogEnabled = false;
        private static LogLevel _logLevel = LogLevel.All;

        public static bool IsEnabled => _enabled;
        public static LogLevel Level => _logLevel;

        static JLog()
        {
            _logger = new UnityLog();
        }

        public static void Shutdown()
        {
            _logger.Dispose();
            _fileLogger?.Dispose();
        }

        public static void SetLogLevel(LogLevel level)
        {
            _logLevel = level;
        }

        public static void Enable()
        {
            _enabled = true;
            UnityEngine.Debug.unityLogger.logEnabled = true;
        }

        public static void Disable()
        {
            _enabled = false;
            UnityEngine.Debug.unityLogger.logEnabled = false;
        }

        public static void EnableFileLog()
        {
            _fileLogEnabled = true;
            if (_fileLogger == null)
                _fileLogger = new FileLog();
        }

        public static void DisableFileLog()
        {
            _fileLogEnabled = false;
            if (_fileLogger != null)
            {
                _fileLogger.Dispose();
                _fileLogger = null;
            }
        }

        public static bool IsLevel(LogLevel level)
        {
            return _logLevel >= level;
        }

        public static void Info(object message, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!IsLevel(LogLevel.Info))
                return;

            if (_enabled)
                _logger.Log(LogLevel.Info, sourceFilePath, sourceLineNumber, message);

            if (_fileLogEnabled)
                _fileLogger.Log(LogLevel.Info, sourceFilePath, sourceLineNumber, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Debug(object message, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!IsLevel(LogLevel.Debug))
                return;

            if (_enabled)
                _logger.Log(LogLevel.Debug, sourceFilePath, sourceLineNumber, message);

            if (_fileLogEnabled)
                _fileLogger.Log(LogLevel.Debug, sourceFilePath, sourceLineNumber, message);
        }

        public static void Warning(object message, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!IsLevel(LogLevel.Warning))
                return;
            if (_enabled)
                _logger.Log(LogLevel.Warning, sourceFilePath, sourceLineNumber, message);

            if (_fileLogEnabled)
                _fileLogger.Log(LogLevel.Warning, sourceFilePath, sourceLineNumber, message);
        }

        public static void Error(object message, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!IsLevel(LogLevel.Error))
                return;

            if (_enabled)
                _logger.Log(LogLevel.Error, sourceFilePath, sourceLineNumber, message);

            if (_fileLogEnabled)
                _fileLogger.Log(LogLevel.Error, sourceFilePath, sourceLineNumber, message);
        }

        public static void Exception(Exception e)
        {
            if (_enabled)
            {
                var trace = new StackTrace(e, true);
                var frame = trace.GetFrame(0);
                var fileName = frame.GetFileName();
                var fileLineNumber = frame.GetFileLineNumber();
                _logger.Log(LogLevel.Error, fileName, fileLineNumber, e);
            }

            if (_fileLogEnabled)
            {
                var trace = new StackTrace(e, true);
                var frame = trace.GetFrame(0);
                var fileName = frame.GetFileName();
                var fileLineNumber = frame.GetFileLineNumber();
                _fileLogger?.Log(LogLevel.Error, fileName, fileLineNumber, e);
            }
        }

        private static (string fileName, int lineNumber) GetCallLocation()
        {
            StackFrame callStack = new StackFrame(2, true);
            string fileName = callStack.GetFileName();
            int fileLineNumber = callStack.GetFileLineNumber();
            return (fileName, fileLineNumber);
        }
    }

}