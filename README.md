
# PrtgTelegramBot
Windows service that uses requests from Telegram to checks status of PRTG devices and sensors and then sends their status to Telegram Chat.
Service uses .NET Framework 4.8 or higher.


**Usage:**
Before using the service you have to:
1. Create a Telegram bot - https://core.telegram.org/bots#creating-a-new-bot
2. Add account to PRTG server, generate Passhash to it and give it appropriate permissions to devices.
3. Configure the access to the PRTG server API by HTTP/HTTPS.
4. Manually init PrtgTelegramBot.exe.config:

DebugLogging - Enable (1) or Disable (0) debug log.
TelegramAccessToken - Telegram Access Token for your Telegram bot. 
PrtgServerApiAddress - IP address or the name (reccomended) of PRTG server API.
PrtgServerUsesHttp - Using HTTP (1) or HTTPS protocol to access to PRTG API.
PrtgServerApiUserName - PRTG server user using to access the PRTG server and getting the status of devices and sensors.
PrtgServerApiPasshash - PRTG server user Passhash

**Supported Telegram commands:**
/getstatus
/getdetailedstatus
/getunhealthy


## **v1.0.0.0**

First release

## Other notes

VS 2019 project.
