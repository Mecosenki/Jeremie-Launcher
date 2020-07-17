using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.ComponentModel;

namespace JeremieLauncher
{
    public static class Utils
    {
        public static bool Is64BitProcess = (IntPtr.Size == 8);
        public static bool Is64BitOperatingSystem = Is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }

        public static string ApplicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\JeremieLauncher";

        public static readonly string[] FileSuffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static readonly string GamesFolder = "games";

        public static string[] TimeSuffixes = { "Seconds", "Minutes", "Hours" };

        public static string ConvertBytesToString(long bytes)
        {
            decimal number = (decimal)bytes;
            int counter = 0;
            while (number / 1024 >= 1)
            {
                number /= 1024;
                counter++;
            }
            return string.Format("{0:n2} {1}", number, FileSuffixes[counter]);
        }

        public static bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                File.WriteAllText(folderPath + "\\test.txt", "");
                File.Delete(folderPath + "\\test.txt");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return HasWriteAccessToFolder(Directory.GetParent(folderPath).FullName);
            }
        }

        public static void StartApplicationInAdminMode()
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo("KankrelatLauncher.exe");
                info.UseShellExecute = true;
                info.Verb = "runas";
                Process.Start(info);
                Environment.Exit(0);
            }
            catch (Win32Exception)
            {
                Environment.Exit(0);
            }
        }

        public static void OpenURL(string url)
        {
            if (MessageBox.Show("This will open the link in your browser, continue?", "Open Link", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process.Start(url);
            }
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

    }
}
