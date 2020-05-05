using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

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

        public static string GamesFolder = "games";

        public static readonly string[] TimeSuffixes = { "Seconds", "Minutes", "Hours" };

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

    }



}
