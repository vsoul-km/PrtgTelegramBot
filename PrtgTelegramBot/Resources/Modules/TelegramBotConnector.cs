using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace PrtgTelegramBot.Resources.Modules
{
    class TelegramBotConnector
    {
        private readonly string _botToken;
        private static WriteLog _writeLog = new WriteLog();
        private static string _botName;
        public TelegramBotConnector(string botToken)
        {
            _botToken = botToken;
        }

        static ITelegramBotClient _botClient;

        public void Initialize()
        {
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Starting initialization the bot.");
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Using the following token: " + _botToken);

            _botClient = new TelegramBotClient(_botToken);

            var me = _botClient.GetMeAsync().Result;
            _botName = me.Username;

            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Bot id: " + me.Id);
            _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Bot name: " + me.Username);

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs messageEvent)
        {
            if (messageEvent.Message.Text != null)
            {
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Chat id is: " + messageEvent.Message.Chat.Id);
                _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + "Received the following message in a chat: " + messageEvent.Message.Text);

                if (messageEvent.Message.Text == "/getstatus" || messageEvent.Message.Text.Contains("/getstatus" + "@" + _botName))
                {
                    PrtgConnector prtgConnector = new PrtgConnector();
                    Message messages;

                    try
                    {
                        messages = prtgConnector.GetStatus();

                        if (!messages.IsEmpty())
                        {

                            foreach (string message in messages.GetMessages())
                            {
                                await _botClient.SendTextMessageAsync(chatId: messageEvent.Message.Chat, text: message);
                            }
                        }
                        else
                        {
                            await _botClient.SendTextMessageAsync(chatId: messageEvent.Message.Chat, text: "Can't get status from the server!");
                        }
                    }
                    catch (Exception e)
                    {
                        _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + " Error to send message to Telegram");
                        _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + e);
                    }
                }

                if (messageEvent.Message.Text == "/getdetailedstatus" || messageEvent.Message.Text.Contains("/getdetailedstatus" + "@" + _botName))
                {
                    PrtgConnector prtgConnector = new PrtgConnector();
                    Message messages;

                    try
                    {
                        messages = prtgConnector.GetDetailedStatus();
                        if (!messages.IsEmpty())
                        {
                            foreach (string message in messages.GetMessages())
                            {
                                await _botClient.SendTextMessageAsync(chatId: messageEvent.Message.Chat, text: message);
                            }
                        }
                        else
                        {
                            await _botClient.SendTextMessageAsync(chatId: messageEvent.Message.Chat, text: "There are no devices/sensors in the system!");
                        }
                    }
                    catch (Exception e)
                    {
                        _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " +
                                         " Error to send message to Telegram");
                        _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + e);
                    }
                }

                if (messageEvent.Message.Text == "/getunhealthy" || messageEvent.Message.Text.Contains("/getunhealthy" + "@" + _botName))
                {
                    PrtgConnector prtgConnector = new PrtgConnector();
                    Message messages;

                    try
                    {
                        messages = prtgConnector.GetUnhealthy();

                        if (!messages.IsEmpty())
                        {
                            foreach (string message in messages.GetMessages())
                            {
                                await _botClient.SendTextMessageAsync(chatId: messageEvent.Message.Chat, text: message);
                            }
                        }
                        else
                        {
                            await _botClient.SendTextMessageAsync(chatId: messageEvent.Message.Chat, text: "There are no unhealthy sensors at the moment!");
                        }
                    }
                    catch (Exception e)
                    {
                        _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + " Error to send message to Telegram");
                        _writeLog.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + ": " + e);
                    }
                }
            }

            //else
            //{
            //    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "You said:\n" + e.Message.Text);
            //}
        }
    }
}
