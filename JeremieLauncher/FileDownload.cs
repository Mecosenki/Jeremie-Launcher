using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JeremieLauncher
{
    //Credit to ebb and Felix D. on stackoverflow.com
    public class FileDownload
    {
        private volatile bool AllowedToRun;
        private string Source;
        private string Destination;
        private int ChunkSize;

        private Lazy<int> ContentLength;

        public int BytesWritten { get; private set; }

        public DownloadProgressChangeEventHandler ProgressChanged;

        public EventHandler DownloadCompleted;

        private bool Overwrite;

        private bool Paused;

        public FileDownload(string source, string destination, bool overwrite=true, int chunckSize=5120)
        {
            AllowedToRun = true;

            Source = source;
            Destination = destination;
            ChunkSize = chunckSize;
            ContentLength = new Lazy<int>(()=>Convert.ToInt32(GetContentLength()));

            Overwrite = overwrite;

            BytesWritten = 0;
        }

        private long GetContentLength()
        {
            var request = (HttpWebRequest)WebRequest.Create(Source);
            request.Method = "HEAD";

            using (var response = request.GetResponse())
            {
                return response.ContentLength;
            }
        }

        private async Task Start(int range)
        {
            if (!AllowedToRun)
                throw new InvalidOperationException();

            var request = (HttpWebRequest)WebRequest.Create(Source);
            request.Method = "GET";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            request.AddRange(range);

            DownloadProgressChangeEventHandler DPGhandler = ProgressChanged;

            using (var response = await request.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(Destination, (Overwrite&&!Paused) ?FileMode.Create:FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        Paused = false;
                        if (fs.Length != ContentLength.Value || Overwrite)
                            while (AllowedToRun)
                            {
                                var buffer = new byte[ChunkSize];
                                var bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length);

                                DownloadProgressChangeEventArgs DPGea = new DownloadProgressChangeEventArgs(fs.Length, ContentLength.Value);

                                if (DPGhandler != null)
                                {
                                    DPGhandler?.Invoke(this, DPGea);
                                }

                                if (bytesRead == 0) break;

                                await fs.WriteAsync(buffer, 0, bytesRead);
                                BytesWritten += bytesRead;
                            }

                        await fs.FlushAsync();
                    }
                }
            }
            if (AllowedToRun)
            {
                EventHandler handler = DownloadCompleted;
                if (handler != null)
                {
                    handler?.Invoke(this, new EventArgs());
                }
            }
        }

        public Task Start()
        {
            AllowedToRun = true;
            return Start(BytesWritten);
        }

        public void Pause()
        {
            Paused = true;
            AllowedToRun = false;
        }
    }

    public class DownloadProgressChangeEventArgs : EventArgs
    {
        public DownloadProgressChangeEventArgs(long bytesReceived, long totalBytesToReceive)
        {
            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;
            ProgressPercentage = (int) ((bytesReceived * 100) / totalBytesToReceive);
            RemainingBytes = totalBytesToReceive - bytesReceived;
        }
        public string ConvertDownloadedBytesToString()
        {
            return ConvertBytesToString(BytesReceived);
        }
        public string ConvertRemainingBytesToString()
        {
            return ConvertBytesToString(RemainingBytes);
        }
        public string ConvertTotalBytesToString()
        {
            return ConvertBytesToString(TotalBytesToReceive);
        }
        public string ConvertBytesToString(long bytes)
        {
            decimal number = (decimal)bytes;
            int counter = 0;
            while (number / 1024 >= 1)
            {
                number /= 1024;
                counter++;
            }
            return string.Format("{0:n2} {1}", number, Utils.FileSuffixes[counter]);
        }

        public long BytesReceived { get; }
        public long TotalBytesToReceive { get; }
        public int ProgressPercentage { get; }
        public long RemainingBytes { get; }
    }

    public delegate void DownloadProgressChangeEventHandler(object sender, DownloadProgressChangeEventArgs e);
}
