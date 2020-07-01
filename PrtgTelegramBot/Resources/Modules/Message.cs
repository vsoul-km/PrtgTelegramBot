using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PrtgTelegramBot.Resources.Modules
{
    class Message
    {
        string _textMessage = string.Empty;
        private static readonly string TelegramMaxMessageSize = string.IsNullOrEmpty(ConfigurationManager.AppSettings["TelegramMaxMessageSize"]) ? "4096" : ConfigurationManager.AppSettings["TelegramMaxMessageSize"];
        private static WriteLog _writeLog = new WriteLog();
        public void Append(string messageText)
        {
            if (_textMessage.Length != 0)
            {
                _textMessage += "\n";
            }

            _textMessage += messageText;
        }

        private static IEnumerable<string> Split(string str, int n)
        {
            if (!String.IsNullOrEmpty(str) || n < 1)
            {
                //throw new ArgumentException();
                if (str != null)
                    for (var i = 0; i < str.Length; i += n)
                        yield return str.Substring(i, Math.Min(n, str.Length - i));
            }
        }

        public List<string> GetMessages()
        {
            if (Int32.TryParse(TelegramMaxMessageSize, out var telegramMaxMessageSize))
            {
                Int32.TryParse(TelegramMaxMessageSize, out telegramMaxMessageSize);

            }
            else
            {
                telegramMaxMessageSize = 4096;
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Can't convert argument PrtgServerApiConnectionTimeout from MonitoringTelegramBot.exe.config to integer. Using default value = 4096 seconds. Please check the application configuration file.");
            }

            if (!String.IsNullOrEmpty(_textMessage))
            {
                return Split(_textMessage, telegramMaxMessageSize).ToList();
            }

            return new List<string>();
        }

        public bool IsEmpty()
        {
            if (String.IsNullOrEmpty(_textMessage))
            {
                return true;
            }

            return false;
        }
    }
}