using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    //Credit to ebb and Felix D. on stackoverflow.com edited by App24
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

        private long DownloadSpeedBytes;

        private long lastBytes;
        private long nowBytes;

        private Timer timer = new Timer();

        public FileDownload(string source, string destination, bool overwrite = true, int chunckSize = 5120)
        {
            AllowedToRun = true;

            Source = source;
            Destination = destination;
            ChunkSize = chunckSize;
            ContentLength = new Lazy<int>(() => Convert.ToInt32(GetContentLength()));

            Overwrite = overwrite;

            BytesWritten = 0;
            timer.Interval = 1000;
            timer.Tick += timer_tick;
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

        private bool finished;

        private async Task Start(int range)
        {
            if (!AllowedToRun)
                throw new InvalidOperationException();

            var request = (HttpWebRequest)WebRequest.Create(Source);
            request.Method = "GET";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            request.AddRange(range);

            DownloadProgressChangeEventHandler DPGhandler = ProgressChanged;

            timer.Start();

            string path = Path.GetDirectoryName(Destination);

            if(!string.IsNullOrEmpty(path))
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var response = await request.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(Destination, (Overwrite && !Paused) ? FileMode.Create : FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        Paused = false;
                        finished = false;
                        if (fs.Length != ContentLength.Value || Overwrite)
                        {
                            while (AllowedToRun)
                            {
                                var buffer = new byte[ChunkSize];
                                var bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length);

                                DownloadProgressChangeEventArgs DPGea = new DownloadProgressChangeEventArgs(fs.Length + bytesRead, ContentLength.Value, DownloadSpeedBytes);

                                if (DPGhandler != null)
                                {
                                    DPGhandler?.Invoke(this, DPGea);
                                }

                                if (bytesRead == 0) { finished = true; break; }

                                await fs.WriteAsync(buffer, 0, bytesRead);
                                BytesWritten += bytesRead;

                                nowBytes = fs.Length;
                            }
                        }

                        await fs.FlushAsync();
                    }
                }
            }
            if (finished)
            {
                EventHandler handler = DownloadCompleted;
                if (handler != null)
                {
                    handler?.Invoke(this, new EventArgs());
                }
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            DownloadSpeedBytes = nowBytes - lastBytes;
            lastBytes = nowBytes;
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
            timer.Stop();
        }
    }

    public class DownloadProgressChangeEventArgs : EventArgs
    {
        public DownloadProgressChangeEventArgs(long bytesReceived, long totalBytesToReceive, long downloadSpeedBytes)
        {
            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;
            ProgressPercentage = (int)((bytesReceived * 100) / totalBytesToReceive);
            RemainingBytes = totalBytesToReceive - bytesReceived;
            DownloadSpeedBytes = downloadSpeedBytes;
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
            return Utils.ConvertBytesToString(bytes);
        }
        public string getTimeRemaing()
        {
            if (DownloadSpeedBytes <= 0)
            {
                return "infinity";
            }
            decimal time = (decimal)RemainingBytes / DownloadSpeedBytes;
            int counter = 0;
            for (int i = 0; i < 2; i++)
            {
                if (time / 60 >= 1)
                {
                    time /= 60;
                    counter++;
                }
            }
            return string.Format("{0:n" + (counter == 0 ? "0" : "2") + "} {1}", time, Utils.TimeSuffixes[counter]);
        }

        public long BytesReceived { get; }
        public long TotalBytesToReceive { get; }
        public int ProgressPercentage { get; }
        public long RemainingBytes { get; }
        public long DownloadSpeedBytes { get; }
    }

    public delegate void DownloadProgressChangeEventHandler(object sender, DownloadProgressChangeEventArgs e);
}
