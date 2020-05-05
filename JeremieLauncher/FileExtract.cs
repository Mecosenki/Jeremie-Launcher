using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace JeremieLauncher
{
    public class FileExtract
    {
        public FileExtract(string zipName, string destDir, bool deleteZip)
        {
            ZipName = zipName;
            DestDir = destDir;
            DeleteZip = deleteZip;
        }

        private async Task Start()
        {
            ExtractProgressChangedEventHandler handler = ExtractProgressChanged;
            DirectoryInfo di = Directory.CreateDirectory(DestDir);
            string destinationDirectoryFullpath = di.FullName;
            ZipArchive source = ZipFile.OpenRead(ZipName);
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
                    await Task.Run(() => { entry.ExtractToFile(fileDestinationPath, overwrite: true); });
                }

                ExtractProgressChangedEventArgs args = new ExtractProgressChangedEventArgs(count, source.Entries.Count, entry);

                if (handler != null)
                    handler?.Invoke(this, args);

            }

            EventHandler eventHandler = ExtractCompleted;
            if (eventHandler != null)
            {
                eventHandler?.Invoke(this, new EventArgs());
            }

            source.Dispose();

            if (DeleteZip)
                File.Delete(ZipName);
        }

        public Task StartExtract()
        {
            return Start();
        }

        public ExtractProgressChangedEventHandler ExtractProgressChanged;
        public EventHandler ExtractCompleted;

        private string ZipName;
        private string DestDir;
        private bool DeleteZip;
    }

    public class ExtractProgressChangedEventArgs : EventArgs
    {
        public ExtractProgressChangedEventArgs(int extractedCount, int allExtractCount, ZipArchiveEntry zipArchiveEntry)
        {
            Progress = (extractedCount * 100) / allExtractCount;
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
