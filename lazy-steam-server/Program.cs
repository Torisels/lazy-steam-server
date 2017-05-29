using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lazy_steam_server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
       
        [STAThread]
        static void Main()
        {
          
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new App());
                mutex.ReleaseMutex();
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
