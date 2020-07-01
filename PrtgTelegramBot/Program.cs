using System;
using System.ServiceProcess;


namespace PrtgTelegramBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            PrtgTelegramBot service = new PrtgTelegramBot();
            if (Environment.UserInteractive)
            {
                MainRoutine runMainRoutine = new MainRoutine();
                runMainRoutine.Start();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new PrtgTelegramBot()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}