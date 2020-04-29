using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public class Game
    {
        public Game(string gameFolder, string executable, string gameName, string versionFile, string csvFileURL, string csvFile, string zipFile)
        {
            GameFolder = JeremieLauncher.gamesFolder + gameFolder;
            Executable = gameFolder + "\\" + executable;
            GameName = gameName;
            VersionFile = GameFolder + "\\" + versionFile;
            CSVFileURL = csvFileURL;
            CSVFile = JeremieLauncher.applicationFolder + "\\" + csvFile + ".csv";
            ZipFile = JeremieLauncher.applicationFolder + "\\" + zipFile + ".zip";
        }

        public void SwitchGame()
        {
            Utils.downloadFile(CSVFileURL, CSVFile, downloadCsvProgressChanged, downloadCsvComplete);
        }

        private void downloadCsvProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            JeremieLauncher.instance.lblDownload.Text = "Checking for updates for: " + GameName + " " + e.ProgressPercentage.ToString() + "%";
            JeremieLauncher.instance.pbDownload.Value = e.ProgressPercentage;
        }

        private async void downloadCsvComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ((WebClient)sender).Dispose();
                await Task.Run(() => File.Delete(CSVFile));
                return;
            }

            Dictionary<string, string> values = new Dictionary<string, string>();

            String[] lines = File.ReadAllLines(CSVFile);

            foreach (string line in lines)
            {
                var lineValues = line.Split(',');

                values.Add(lineValues[0], lineValues[1]);
            }
            File.Delete(CSVFile);

            string os = "";
            OperatingSystem os_info = Environment.OSVersion;
            PlatformID pid = os_info.Platform;
            switch (pid)
            {
                case PlatformID.Unix:
                    os = "lin";
                    break;
                case PlatformID.MacOSX:
                    os = "mac";
                    break;
                case PlatformID.Win32NT:
                    os = "win";
                    if (JeremieLauncher.is64BitOperatingSystem)
                    {
                        os += ".64";
                    }
                    else
                    {
                        os += ".32";
                    }
                    break;
                default:
                    break;
            }

            foreach (KeyValuePair<string, string> item in values)
            {
                if (item.Key == "curVerNum." + os)
                {
                    Version = item.Value;
                    values.TryGetValue("URL." + os, out URL);
                }
                if (item.Key == "trailerID")
                {
                    VidURL = item.Value;
                }
            }

            JeremieLauncher.instance.pbVideo.Load("http://img.youtube.com/vi/" + VidURL + "/0.jpg");
            if (VidURL != "")
            {
                JeremieLauncher.instance.pbVideo.Cursor = Cursors.Hand;
            }

            checkGameStatus();

            switch (GameStatus)
            {
                case GameStatuses.NOTAVAILABLE:
                    JeremieLauncher.instance.lblDownload.Text = GameName+" not available for your OS";
                    break;
                case GameStatuses.NEEDINSTALL:
                    JeremieLauncher.instance.lblDownload.Text = GameName + " needs to be installed";
                    break;
                case GameStatuses.NEEDUPDATE:
                    JeremieLauncher.instance.lblDownload.Text = "New Update for: " + GameName + " version: "+Version+", you have version: "+getInstalledVersion()+" installed";
                    break;
                case GameStatuses.OK:
                    JeremieLauncher.instance.lblDownload.Text = GameName+" is up to date";
                    break;
            }

            updateButton();

            JeremieLauncher.instance.lblVersion.Text = "Installed Version: "+getInstalledVersion();
        }

        private void updateButton()
        {
            Button btn = JeremieLauncher.instance.btnInstall;
            switch (GameStatus)
            {
                case GameStatuses.NOTAVAILABLE:
                    btn.Text = "Install";
                    JeremieLauncher.instance.btnInstall.Enabled = false;
                    break;
                case GameStatuses.OK:
                    btn.Text = "Play";
                    JeremieLauncher.instance.btnInstall.Enabled = true;
                    break;
                case GameStatuses.NEEDUPDATE:
                    btn.Text = "Update";
                    JeremieLauncher.instance.btnInstall.Enabled = true;
                    break;
                case GameStatuses.NEEDINSTALL:
                    btn.Text = "Install";
                    JeremieLauncher.instance.btnInstall.Enabled = true;
                    break;
            }

            //JeremieLauncher.instance.btnInstall.Enabled = GameStatus == GameStatuses.NOTAVAILABLE ? false : true;
        }

        public void downloadGame()
        {
            switch (GameStatus)
            {
                case GameStatuses.OK:
                    var startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + JeremieLauncher.gamesFolder;
                    var programPath = Path.Combine(startupPath, Executable);
                    Process.Start(programPath);
                    Environment.Exit(0);
                    break;
                case GameStatuses.NEEDINSTALL:
                case GameStatuses.NEEDUPDATE:
                    Timer timer = new Timer();
                    timer.Tick += new EventHandler(timer_tick);
                    timer.Interval = 1000;
                    timer.Start();
                    JeremieLauncher.instance.btnInstall.Enabled = false;
                    if (URL != "")
                    {
                        Utils.downloadFile(URL, ZipFile, downloadProgressChange, downloadComplete);
                        Downloading = true;
                    }
                    else
                        MessageBox.Show("Your OS is not supported");
                    break;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            downloadSpeedBytes = nowBytes - lastBytes;
            lastBytes = nowBytes;
        }

        private void downloadProgressChange(object sender, DownloadProgressChangedEventArgs e)
        {
            nowBytes = e.BytesReceived;
            JeremieLauncher.instance.lblDownload.Text = "Progress: " + convertBytes(e.BytesReceived) + "/" + convertBytes(e.TotalBytesToReceive) + " (" + e.ProgressPercentage.ToString() + "%) " + convertBytes(downloadSpeedBytes) + "/s " + getTimeRemaing(e.TotalBytesToReceive - e.BytesReceived);
            JeremieLauncher.instance.pbDownload.Value = e.ProgressPercentage;
        }

        private string getTimeRemaing(long allBytes)
        {
            if (downloadSpeedBytes <= 0)
            {
                return "infinity";
            }
            decimal time = (decimal)allBytes / downloadSpeedBytes;
            int counter = 0;
            for (int i = 0; i < 2; i++)
            {
                if (time / 60 >= 1)
                {
                    time /= 60;
                    counter++;
                }
            }
            return string.Format("{0:n" + (counter == 0 ? "0" : "2") + "} {1}", time, JeremieLauncher.timeSuffixes[counter]);
        }

        private string convertBytes(long bytes)
        {
            decimal number = (decimal)bytes;
            int counter = 0;
            while (number / 1024 >= 1)
            {
                number /= 1024;
                counter++;
            }
            return string.Format("{0:n2} {1}", number, JeremieLauncher.suffixes[counter]);
        }

        private async void downloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ((WebClient)sender).Dispose();
                await Task.Run(() => File.Delete(ZipFile));
                return;
            }
            ((WebClient)sender).Dispose();
            await Utils.ExtractAllAsync(ZipFile, GameFolder, true);
            File.WriteAllText(VersionFile, Version);
            JeremieLauncher.instance.lblDownload.Text = "Finished " + (GameStatus == GameStatuses.NEEDINSTALL ? "Installing" : "Updating");
            checkGameStatus();
            updateButton();
            JeremieLauncher.instance.lblVersion.Text = "Installed Version: " + getInstalledVersion();
            Downloading = false;
        }

        private bool checkVersion()
        {

            return getInstalledVersion() == Version;
        }

        private void checkGameStatus()
        {
            GameStatus = URL==""?GameStatuses.NOTAVAILABLE:( Directory.Exists(GameFolder) ? (checkVersion() ? GameStatuses.OK : GameStatuses.NEEDUPDATE) : GameStatuses.NEEDINSTALL);
        }

        private string getInstalledVersion()
        {
            string version = "Not Installed";
            if (File.Exists(VersionFile))
            {
                version = File.ReadAllLines(VersionFile)[0];
            }
            return version;
        }

        public string GameFolder { get; }
        public string VersionFile { get; }
        public GameStatuses GameStatus { get; private set; }
        public string URL = "";
        public string Version { get; private set; }
        public string CSVFileURL { get; }
        public string CSVFile { get; }
        public string ZipFile { get; }
        public string Executable { get; }
        public string GameName { get; }
        public string VidURL { get; private set; }
        public bool Downloading { get; private set; }

        private long nowBytes = 0;
        private long lastBytes = 0;
        private long downloadSpeedBytes = 0;
    }
}
