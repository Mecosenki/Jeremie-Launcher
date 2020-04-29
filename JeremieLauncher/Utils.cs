using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace JeremieLauncher
{
    public static class Utils
    {

        public static void downloadFile(string url, string destinationName, DownloadProgressChangedEventHandler progress = null, AsyncCompletedEventHandler complete = null)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += progress;
            webClient.DownloadFileCompleted += complete;
            webClient.DownloadFileAsync(new Uri(url), destinationName);
        }

        public static async Task ExtractAllAsync(string zipFile, string destinationFolder, bool deleteZip)
        {
            await Task.Run(() =>
            {
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

                    JeremieLauncher.instance.Invoke(new Action(() =>
                    {
                        int progress = (count * 100) / source.Entries.Count;
                        JeremieLauncher.instance.lblDownload.Text = "Extracting... " + " (" + progress.ToString() + "%)";
                        JeremieLauncher.instance.pbDownload.Value = progress;
                    }));

                }

                source.Dispose();

                if (deleteZip)
                    File.Delete(zipFile);
            });
        }

    }
}
