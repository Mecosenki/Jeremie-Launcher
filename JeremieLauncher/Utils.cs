using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
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

        public static string GamesFolder = "games\\";

        public static readonly string[] TimeSuffixes = { "Seconds", "Minutes", "Hours" };

        
        [Obsolete("Please Use FileDownload class for a better control over the download")]
        public static void DownloadFile(string url, string destinationName, DownloadProgressChangedEventHandler progress = null, AsyncCompletedEventHandler complete = null)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += progress;
            webClient.DownloadFileCompleted += complete;
            webClient.DownloadFileAsync(new Uri(url), destinationName);
        }

        public static async Task ExtractAllAsync(string zipFile, string destinationFolder, bool deleteZip, ExtractProgressChangedEventHandler progressChanged=null)
        {
            await Task.Run(() =>
            {
                ExtractProgressChangedEventHandler handler=progressChanged;
                DirectoryInfo di = Directory.CreateDirectory(destinationFolder);
                string destinationDirectoryFullpath = di.FullName;

                ZipArchive source = ZipFile.OpenRead(zipFile);
                int count = 0;
                foreach (ZipArchiveEntry entry in source.Entries)
                {
                    count++;
                    string fileDestinationPath = Path.GetFullPath(Path.Combine(destinationDirectoryFullpath, entry.FullName));

                    if (!fileDestinationPath.StartsWith(destinationDirectoryFullpath, StringComparison.OrdinalIgnoreCase))
                        throw new IOException("File is extracting to outside of the folder specified.");

                    if (Path.GetFileName(fileDestinationPath).Length == 0)
                    {
                        if (entry.Length != 0)
                            throw new IOException("Directory entry with data.");

                        Directory.CreateDirectory(fileDestinationPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath));
                        entry.ExtractToFile(fileDestinationPath, overwrite: true);
                    }

                    ExtractProgressChangedEventArgs args = new ExtractProgressChangedEventArgs((count * 100) / source.Entries.Count, count, source.Entries.Count, entry);

                    if(handler!=null)
                    handler?.Invoke(JeremieLauncher.instance, args);

                }

                source.Dispose();

                if (deleteZip)
                    File.Delete(zipFile);
            });
        }

    }

    public class ExtractProgressChangedEventArgs : EventArgs
    {
        public ExtractProgressChangedEventArgs(int progress, int extractedCount, int allExtractCount, ZipArchiveEntry zipArchiveEntry)
        {
            Progress = progress;
            ExtractedCount = extractedCount;
            AllExtractCount = allExtractCount;
            CurrentEntry = zipArchiveEntry;
            CurrentFileName = zipArchiveEntry.FullName;
        }

        public int Progress { get; }
        public int ExtractedCount { get; }
        public int AllExtractCount { get; }
        public string CurrentFileName { get; }
        public ZipArchiveEntry CurrentEntry;
    }

    public delegate void ExtractProgressChangedEventHandler(Object sender, ExtractProgressChangedEventArgs e);

}
