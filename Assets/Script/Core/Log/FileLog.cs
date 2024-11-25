using System;
using System.IO;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Jam.Core
{

    public class FileLog : ILogger
    {
        private const int MaxLogFileCount = 5;
        private StreamWriter _logFileStream;
        private readonly string DirectoryPath = $"{Application.persistentDataPath}/Log";
        public LoggerType LoggerType => LoggerType.File;

        public FileLog()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            _logFileStream = new StreamWriter($"{DirectoryPath}/{fileName}");

            Application.logMessageReceivedThreaded += UnityLogCallback;

            try
            {
                DeletePreviousLogFiles();
            }
            catch (Exception e)
            {
                JLog.Exception(e);
            }
        }

        public void Log(LogLevel level, string file, int fileLine, object message)
        {
            if (UnityEngine.Debug.unityLogger.logEnabled)
                return;
            var now = DateTime.Now.ToString("HH:mm:ss");
            var fileName = Path.GetFileNameWithoutExtension(file);
            string content = $"[{level}: {now}] [{fileName}:{fileLine}] {message}\n";
            _logFileStream.Write(content);
        }

        private void UnityLogCallback(string logString, string stackTrace, LogType type)
        {
            _logFileStream.Write($"[{type}] {logString}\n");
        }

        public void Dispose()
        {
            _logFileStream.Close();
            Application.logMessageReceived -= UnityLogCallback;
        }

        private void DeletePreviousLogFiles()
        {
            var files = Directory.GetFiles(DirectoryPath);
            if (files.Length <= MaxLogFileCount)
                return;
            Array.Sort(files);
            for (int i = 0; i < files.Length - MaxLogFileCount; i++)
            {
                File.Delete(files[i]);
            }
        }
    }

}