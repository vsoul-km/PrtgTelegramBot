

# PrtgTelegramBot
Windows service that uses requests from Telegram to checks status of PRTG devices and sensors and then sends their status to Telegram Chat.
Service uses .NET Framework 4.8 or higher.


**Usage:**
Before using the service you have to:
1. Create a Telegram bot - https://core.telegram.org/bots#creating-a-new-bot
2. Add account to PRTG server, generate Passhash to it and give it appropriate permissions to devices.
3. Configure the access to the PRTG server API by HTTP/HTTPS.
4. Manually init PrtgTelegramBot.exe.config:

**DebugLogging** - Enable (1) or Disable (0) debug log.

**TelegramAccessToken** - Telegram Access Token for your Telegram bot. 

**PrtgServerApiAddress** - IP address or the name (reccomended) of PRTG server API.

**PrtgServerUsesHttp** - Using HTTP (1) or HTTPS protocol to access to PRTG API.

**PrtgServerApiUserName** - PRTG server user using to access the PRTG server and getting the status of devices and sensors.

**PrtgServerApiPasshash** - PRTG server user Passhash

## **Supported Telegram commands:**

**/getstatus**

**/getdetailedstatus**

**/getunhealthy**


## **Output examples**
**/getstatus**
```
Healthy Sensors: 31
Unhealthy Sensors: 0
Warning Sensors: 0
Paused Sensors: 1
Partial Alarams Sensors: 0
Unknown Sensors: 0
Unusual Sensors: 2
```

**/getdetailedstatus**
```
dns.somesite.site
 Device State: PausedByUser
 Device Monitoring Status: False

 somesite.site
  Sensor Status: PausedByUser
  Sensor Last Check Time: 01.03.2020 15:40:59
  Sensor Last Downtime: 01.03.2020 15:40:59
  Sensor Downtime Duration: 
  Sensor Last Up Time: 30.01.2020 4:59:14

 othersite.site
  Sensor Status: PausedByUser
  Sensor Last Check Time: 01.03.2020 15:41:04
  Sensor Last Downtime: 01.03.2020 15:41:04
  Sensor Downtime Duration: 
  Sensor Last Up Time: 30.01.2020 4:59:19

mail.site
 Device State: Up
 Device Monitoring Status: True

 ActiveSync
  Sensor Status: Up
  Sensor Last Check Time: 22.06.2020 23:01:42
  Sensor Last Downtime: 22.06.2020 9:38:23
  Sensor Downtime Duration: 
  Sensor Last Up Time: 22.06.2020 23:01:42
```

**/getunhealthy**
```
There are no unhealthy sensors at the moment!
```


## **v1.0.0.0**
First release

## Other notes
Uses the following Nuget packages:

**PrtgApi** - https://www.nuget.org/packages/PrtgAPI/

**TelegramBot** - https://www.nuget.org/packages/Telegram.Bot/


```VS 2019 project.```
