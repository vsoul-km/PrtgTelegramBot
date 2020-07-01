using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace PrtgTelegramBot
{
    internal partial class PrtgTelegramBot : ServiceBase
    {
        private MainRoutine _mainRoutine;
        private Task _mainTask;
        private CancellationTokenSource _cancellationToken;

        public PrtgTelegramBot()
        {
            InitializeComponent();
            CanPauseAndContinue = false;
        }

        protected override void OnStart(string[] args)
        {
            _cancellationToken = new CancellationTokenSource();
            _mainTask = new Task(Loop);
            _mainTask.Start();
        }

        private void Loop()
        {
            var token = _cancellationToken.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using (_mainRoutine = new MainRoutine())
                    {
                        _mainRoutine.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (!token.IsCancellationRequested)
                    {
                        _cancellationToken.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(15));
                    }
                }
            }
        }

        protected override void OnStop()
        {
            _mainRoutine.Stop();
        }
        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }
    }
}