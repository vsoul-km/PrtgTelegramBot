using System;
using System.Configuration;
using System.IO;

namespace PrtgTelegramBot.Resources.Modules
{
    class WriteLog
    {
        private readonly string _debugLogging = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DebugLogging"]) ? "0" : ConfigurationManager.AppSettings["DebugLogging"];
        private readonly string _logPath;
        public WriteLog()
        {
            if (_debugLogging == "1")
            {
                _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MonitoringTelegramBot.log");

                try
                {
                    File.AppendAllText(_logPath, "" + Environment.NewLine);
                }
                catch
                {
                    Console.WriteLine($"Can't write log to file {_logPath}.");
                    _logPath = Path.Combine(Path.GetTempPath(), "MonitoringTelegramBot.log");
                    Console.WriteLine($"Using the following file to log: {_logPath}");
                }
            }
        }

        public void Append(string text)
        {
            if (_debugLogging == "1")
            {
                try
                {
                    File.AppendAllText(_logPath, text + Environment.NewLine);
                }
                catch
                {
                    Console.WriteLine($"Can't write log to file {_logPath}");
                }
            }
        }
    }
}