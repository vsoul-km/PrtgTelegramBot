using System;
using System.Configuration;
using System.Threading;
using PrtgTelegramBot.Resources.Modules;

namespace PrtgTelegramBot
{
    public sealed class MainRoutine : IDisposable
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly string _serviceCycleInterval = string.IsNullOrEmpty(ConfigurationManager.AppSettings["ServiceCycleInterval"]) ? "10000" : ConfigurationManager.AppSettings["ServiceCycleInterval"];
        private readonly string _telegramToken = string.IsNullOrEmpty(ConfigurationManager.AppSettings["TelegramAccessToken"]) ? "" : ConfigurationManager.AppSettings["TelegramAccessToken"];
        private static WriteLog _writeLog = new WriteLog();

        public void Start()
        {
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Starting the MonitoringTelegramBot Service.");

            if (string.IsNullOrEmpty(_telegramToken))
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Telegram access token does not exist in the application configuration file. Terminating the service. ");
                Environment.Exit(-1);
            }

            if (Int32.TryParse(_serviceCycleInterval, out var serviceCycleInterval))
            {
                Int32.TryParse(_serviceCycleInterval, out serviceCycleInterval);
            }
            else
            {
                serviceCycleInterval = 10000;
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Can't convert argument ServiceCycleInterval from MonitoringTelegramBot.exe.config to integer. Using default value = 10000 milliseconds. Please check the application configuration file.");
            }

            while (!_cancellationToken.IsCancellationRequested)
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Starting the new chat query cycle.");
                //
                try
                {
                    TelegramBotConnector bot = new TelegramBotConnector(_telegramToken);
                    bot.Initialize();
                }
                catch (Exception e)
                {
                    _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "There is an error.");
                    _writeLog.Append(e.ToString());
                }

                //Wait serviceCycleIntervalSeconds seconds before starting a new cycle.
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Chat query cycle is finished. Waiting " + serviceCycleInterval + " milliseconds to start a new cycle.");
                _cancellationToken.Token.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(serviceCycleInterval));
            }
        }

        public void Stop()
        {
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Stop service method has been called. Trying to stop service.");

            try
            {
                _cancellationToken.Cancel();
            }
            catch (Exception e)
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Can't stop service. See log bellow for details.");
                _writeLog.Append(e.ToString());
            }
        }

        public void Dispose()
        {
            _cancellationToken.Dispose();
        }
    }
}