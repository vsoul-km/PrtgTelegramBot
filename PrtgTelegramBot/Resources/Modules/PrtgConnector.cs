using System;
using System.Collections.Generic;
using System.Configuration;
using PrtgAPI;

namespace PrtgTelegramBot.Resources.Modules
{
    class PrtgConnector
    {
        private static readonly string PrtgServerApiAddress = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrtgServerApiAddress"]) ? "" : ConfigurationManager.AppSettings["PrtgServerApiAddress"];
        private static readonly string PrtgServerUsesHttp = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrtgServerUsesHttp"]) ? "1" : ConfigurationManager.AppSettings["PrtgServerUsesHttp"];
        private static readonly string PrtgServerApiUserName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrtgServerApiUserName"]) ? "" : ConfigurationManager.AppSettings["PrtgServerApiUserName"];
        private static readonly string PrtgServerApiPasshash = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrtgServerApiPasshash"]) ? "" : ConfigurationManager.AppSettings["PrtgServerApiPasshash"];
        private static readonly string PrtgServerApiConnectionTimeout = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrtgServerApiConnectionTimeout"]) ? "10" : ConfigurationManager.AppSettings["PrtgServerApiConnectionTimeout"];
        private static readonly string PrtgServerApiConnectionRetryCount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrtgServerApiConnectionRetryCount"]) ? "2" : ConfigurationManager.AppSettings["PrtgServerApiConnectionRetryCount"];
        private static WriteLog _writeLog = new WriteLog();

        private static PrtgClient Initialize()
        {
            if (Int32.TryParse(PrtgServerUsesHttp, out var prtgServerUsesHttp))
            {
                Int32.TryParse(PrtgServerUsesHttp, out prtgServerUsesHttp);
            }
            else
            {
                prtgServerUsesHttp = 1;
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Can't convert argument PrtgServerUsesHttp from MonitoringTelegramBot.exe.config to integer. Using default value = 1 (true). Please check the application configuration file.");
            }

            string serverAddress = PrtgServerApiAddress;

            if (prtgServerUsesHttp == 1)
            {
                serverAddress = "http://" + PrtgServerApiAddress;
            }

            var prtgClient = new PrtgClient(serverAddress, PrtgServerApiUserName, PrtgServerApiPasshash, AuthMode.PassHash);

            if (Int32.TryParse(PrtgServerApiConnectionTimeout, out var prtgServerApiConnectionTimeout))
            {
                Int32.TryParse(PrtgServerApiConnectionTimeout, out prtgServerApiConnectionTimeout);
            }
            else
            {
                prtgServerApiConnectionTimeout = 5;
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Can't convert argument PrtgServerApiConnectionTimeout from MonitoringTelegramBot.exe.config to integer. Using default value = 5 seconds. Please check the application configuration file.");
            }

            if (Int32.TryParse(PrtgServerApiConnectionRetryCount, out var prtgServerApiConnectionRetryCount))
            {
                Int32.TryParse(PrtgServerApiConnectionRetryCount, out prtgServerApiConnectionRetryCount);
            }
            else
            {
                prtgServerApiConnectionRetryCount = 2;
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Can't convert argument PrtgServerApiConnectionRetryCount from MonitoringTelegramBot.exe.config to integer. Using default value = 2. Please check the application configuration file.");
            }

            prtgClient.RetryDelay = prtgServerApiConnectionTimeout;
            prtgClient.RetryCount = prtgServerApiConnectionRetryCount;

            return prtgClient;
        }
        public Message GetStatus()
        {
            Message message = new Message();

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Starting initialization the PRTG API Connector.");
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following PRTG API Server to connect: " + PrtgServerApiAddress);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following Username to connect: " + PrtgServerApiUserName);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following Passhash to connect: " + PrtgServerApiPasshash);

            PrtgClient prtgClient = Initialize();

            ServerStatus serverStatus;

            try
            {
                serverStatus = prtgClient.GetStatus();
            }
            catch (Exception e)
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + " Error to connect PRTG Server");
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + e);
                message.Append("Error to connect to PRTG Server " + PrtgServerApiAddress);
                return message;
            }

            message.Append("Healthy Sensors: " + serverStatus.UpSensors);
            message.Append("Unhealthy Sensors: " + serverStatus.Alarms);
            message.Append("Warning Sensors: " + serverStatus.WarningSensors);
            message.Append("Paused Sensors: " + serverStatus.PausedSensors);
            message.Append("Partial Alarams Sensors: " + serverStatus.PartialAlarms);
            message.Append("Unknown Sensors: " + serverStatus.UnknownSensors);
            message.Append("Unusual Sensors: " + serverStatus.UnusualSensors);

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": PRTG Server Returned:\n");

            if (message.GetMessages() != null)
            {
                foreach (string text in message.GetMessages())
                {
                    _writeLog.Append(text);
                }
            }

            return message;
        }
        public Message GetUnhealthy()
        {
            Message message = new Message();

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Starting initialization the PRTG API Connector.");
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following PRTG API Server to connect: " + PrtgServerApiAddress);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following Username to connect: " + PrtgServerApiUserName);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following Passhash to connect: " + PrtgServerApiPasshash);

            PrtgClient prtgClient = Initialize();

            new ServerStatus();
            List<Device> prtgDevices = new List<Device>();
            List<Sensor> prtgSensors = new List<Sensor>();

            try
            {
                prtgClient.GetStatus();
            }
            catch (Exception e)
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + " Error to connect to PRTG Server");
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + e);
                message.Append("Error to connect to PRTG Server " + PrtgServerApiAddress);
                return message;
            }

            foreach (Device prtgDevice in prtgClient.GetDevices())
            {
                prtgDevices.Add(prtgDevice);
            }

            foreach (Sensor prtgSensor in prtgClient.GetSensors())
            {
                prtgSensors.Add(prtgSensor);
            }

            foreach (Sensor sensor in prtgSensors.FindAll(sensor => sensor.Status == Status.Down || sensor.Status == Status.DownPartial || sensor.Status == Status.DownAcknowledged))
            {
                foreach (Device device in prtgDevices.FindAll(device => device.Id == sensor.ParentId))
                {
                    if (!message.IsEmpty())
                    {
                        message.Append("");
                    }
                    message.Append("Device: " + device.Name);
                    message.Append("{");
                    message.Append(" Device State: " + device.Status);
                    message.Append(" Device Monitoring Status: " + device.Active);
                    message.Append("Sensor: " + sensor.Name);
                    message.Append(" Sensor Status: " + sensor.Status);
                    message.Append(" Sensor Last Check Time: " + sensor.LastCheck);
                    message.Append(" Sensor Last Downtime: " + sensor.LastDown);
                    message.Append(" Sensor Downtime Duration: " + sensor.DownDuration);
                    message.Append(" Sensor Last Up Time: " + sensor.LastUp);
                    message.Append("}");
                }
            }

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": PRTG Server Returned:\n");

            if (message.GetMessages() != null)
            {
                foreach (string text in message.GetMessages())
                {
                    _writeLog.Append(text);
                }
            }

            return message;
        }

        public Message GetDetailedStatus()
        {
            Message message = new Message();

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Starting initialization the PRTG API Connector.");
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following PRTG API Server to connect: " + PrtgServerApiAddress);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following Username to connect: " + PrtgServerApiUserName);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following Passhash to connect: " + PrtgServerApiPasshash);

            PrtgClient prtgClient = Initialize();

            new ServerStatus();
            List<Device> prtgDevices = new List<Device>();
            List<Sensor> prtgSensors = new List<Sensor>();

            try
            {
                prtgClient.GetStatus();
            }
            catch (Exception e)
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + " Error to connect PRTG Server");
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + e);
                message.Append("Error to connect to PRTG Server " + PrtgServerApiAddress);
                return message;
            }

            foreach (Device prtgDevice in prtgClient.GetDevices())
            {
                prtgDevices.Add(prtgDevice);
            }

            foreach (Sensor prtgSensor in prtgClient.GetSensors())
            {
                prtgSensors.Add(prtgSensor);
            }

            foreach (Device device in prtgDevices)
            {
                message.Append("");
                message.Append(device.Name);
                message.Append("  Device State: " + device.Status);
                message.Append("  Device Monitoring Status: " + device.Active);

                foreach (Sensor sensor in prtgSensors.FindAll(sensor => sensor.Device == device.Name))
                {
                    message.Append("");
                    message.Append("    " + sensor.Name);
                    message.Append("     Sensor Status: " + sensor.Status);
                    message.Append("     Sensor Last Check Time: " + sensor.LastCheck);
                    message.Append("     Sensor Last Downtime: " + sensor.LastDown);
                    message.Append("     Sensor Downtime Duration: " + sensor.DownDuration);
                    message.Append("     Sensor Last Up Time: " + sensor.LastUp);
                    //messageText.Append("     Sensor Last Value: " + sensor.LastValue);
                }
            }

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": PRTG Server Returned:\n");

            if (message.GetMessages() != null)
            {
                foreach (string text in message.GetMessages())
                {
                    _writeLog.Append(text);
                }
            }

            return message;
        }
    }
}
