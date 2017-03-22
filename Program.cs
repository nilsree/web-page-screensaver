using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace pl.polidea.lab.Web_Page_Screensaver
{
    static class Program
    {
        public static readonly string KEY = "Software\\Web-Page-Screensaver";
        private static int ViewOnScreen = 0;

        [STAThread]
        static void Main(string[] args)
        {
            // Set version of embedded browser (http://weblog.west-wind.com/posts/2011/May/21/Web-Browser-Control-Specifying-the-IE-Version)
            var exeName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", exeName, 0x2AF8, RegistryValueKind.DWord);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0 && args[0].ToLower().Contains("/p"))
                return;

            if (args.Length > 0 && args[0].ToLower().Contains("/c"))
                Application.Run(new PreferencesForm());
            else
            {
                var primaryScreenBounds = Screen.PrimaryScreen.Bounds;
                for (var i = Screen.AllScreens.GetLowerBound(0); i <= Screen.AllScreens.GetUpperBound(0); i++)
                {
                    ViewOnScreen = i;
                    var thread = new Thread(ThreadStart);
                    if(Screen.AllScreens[i].Bounds != primaryScreenBounds) thread.IsBackground = true;
                    thread.TrySetApartmentState(ApartmentState.STA);
                    thread.Start();

                    Thread.Sleep(100);
                }
            }
        }

        private static void ThreadStart()
        {
            Application.Run(new ScreensaverForm(ViewOnScreen));
        }
    }
}
