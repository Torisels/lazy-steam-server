using System;
using System.Threading;
using System.Windows.Forms;

namespace lazy_steam_server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private static readonly Mutex Mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
       
        [STAThread]
        private static void Main()
        {
          
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new App());
                Mutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                MessageBox.Show("Another instance of app is already running");
                Application.Exit();
                Environment.Exit(1);

            }
        }
    }
}
