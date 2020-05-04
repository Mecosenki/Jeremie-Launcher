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
        public Game(string gameFolder, string executable, string gameName, string csvFileURL, string csvFile, string zipFile)
        {
            GameFolder = Utils.GamesFolder + gameFolder;
            Executable = gameFolder + "\\" + executable+".exe";
            GameName = gameName;
            VersionFile = GameFolder + "\\"+executable+ "_Data\\StreamingAssets\\dataV.dat";
            CSVFileURL = csvFileURL;
            CSVFile = Utils.ApplicationFolder + "\\" + csvFile + ".csv";
            ZipFile = Utils.ApplicationFolder + "\\" + zipFile + ".zip";
        }

        public void SwitchGame()
        {
            //Utils.DownloadFile(CSVFileURL, CSVFile, downloadCsvProgressChanged, downloadCsvComplete);
            JeremieLauncher.instance.ChangeGameName(GameName);
            if (!Downloading)
            {
                FileDownload fw = new FileDownload(CSVFileURL, CSVFile);
                fw.DownloadCompleted += downloadCsvComplete;
                fw.ProgressChanged += downloadCsvProgressChanged;
                fw.Start();
            }
            else
            {
                SetupGame();
            }
        }

        private void downloadCsvProgressChanged(object sender, DownloadProgressChangeEventArgs e)
        {
            JeremieLauncher.instance.lblDownload.Text = "Checking for updates for: " + GameName + " " + e.ProgressPercentage.ToString() + "%";
            JeremieLauncher.instance.pbDownload.Value = e.ProgressPercentage;
        }

        private void downloadCsvComplete(object sender, EventArgs e)
        {
            /*if (e.Cancelled)
            {
                ((WebClient)sender).Dispose();
                await Task.Run(() => File.Delete(CSVFile));
                return;
            }*/
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
                    if (Utils.Is64BitOperatingSystem)
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
                    Version = Version.CreateFromString(item.Value);
                    values.TryGetValue("URL." + os, out URL);
                }
                if (item.Key == "trailerID")
                {
                    VidURL = item.Value;
                }
            }


            SetupGame();
        }

        private void SetupGame()
        {
            JeremieLauncher.instance.pbVideo.Load("http://img.youtube.com/vi/" + VidURL + "/0.jpg");
            if (VidURL != "")
            {
                JeremieLauncher.instance.pbVideo.Cursor = Cursors.Hand;
            }
            checkGameStatus();

            JeremieLauncher.instance.lblVersion.Text = "Installed Version: " + getInstalledVersion();
            switch (GameStatus)
            {
                case GameStatuses.NOTAVAILABLE:
                    JeremieLauncher.instance.lblDownload.Text = GameName + " not available for your OS";
                        JeremieLauncher.instance.btnGameFolder.Enabled = false;
                    JeremieLauncher.instance.btnUninstall.Visible = false;
                    break;
                case GameStatuses.NEEDINSTALL:
                    JeremieLauncher.instance.lblDownload.Text = GameName + " needs to be installed";
                        JeremieLauncher.instance.btnGameFolder.Enabled = false;
                    JeremieLauncher.instance.btnUninstall.Visible = false;
                    break;
                case GameStatuses.NEEDUPDATE:
                    JeremieLauncher.instance.lblDownload.Text = "New Update for: " + GameName;
                    JeremieLauncher.instance.lblVersion.Text = "Installed Version: " + getInstalledVersion()+" New Version: "+Version;
                        JeremieLauncher.instance.btnGameFolder.Enabled = true;
                    JeremieLauncher.instance.btnUninstall.Visible = true;
                    break;
                case GameStatuses.OK:
                    JeremieLauncher.instance.lblDownload.Text = GameName + " is up to date";
                        JeremieLauncher.instance.btnGameFolder.Enabled = true;
                    JeremieLauncher.instance.btnUninstall.Visible = true;
                    break;
            }
            updateButton();
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
                    if (!Downloading && !Extracting)
                        JeremieLauncher.instance.btnInstall.Enabled = true;
                    break;
                case GameStatuses.NEEDINSTALL:
                    btn.Text = "Install";
                    if (!Downloading && !Extracting)
                        JeremieLauncher.instance.btnInstall.Enabled = true;
                    break;
            }

            //JeremieLauncher.instance.btnInstall.Enabled = GameStatus == GameStatuses.NOTAVAILABLE ? false : true;
        }

        public void OpenGameFolder()
        {
            if (GameStatus==GameStatuses.NEEDUPDATE||GameStatus==GameStatuses.OK) {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = GameFolder,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
        }

        public void downloadGame()
        {
            switch (GameStatus)
            {
                case GameStatuses.OK:
                    var startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + Utils.GamesFolder;
                    var programPath = Path.Combine(startupPath, Executable);
                    Process.Start(programPath);
                    if(Options.GetOption<bool>("closeOnLaunch"))
                    Environment.Exit(0);
                    break;
                case GameStatuses.NEEDINSTALL:
                case GameStatuses.NEEDUPDATE:
                    JeremieLauncher.instance.btnInstall.Enabled = false;
                    if (URL != "")
                    {
                        //Utils.DownloadFile(URL, ZipFile, downloadProgressChange, downloadComplete);
                        fw = new FileDownload(URL, ZipFile);
                        fw.DownloadCompleted += downloadComplete;
                        fw.ProgressChanged += downloadProgressChange;
                        Downloading = true;
                        fw.Start();
                    }
                    else
                        MessageBox.Show("Your OS is not supported");
                    break;
            }
        }

        private void downloadProgressChange(object sender, DownloadProgressChangeEventArgs e)
        {
            JeremieLauncher.instance.lblDownload.Text = "Progress: " + e.ConvertDownloadedBytesToString() + "/" + e.ConvertTotalBytesToString() + " (" + e.ProgressPercentage.ToString() + "%) " + e.ConvertBytesToString(e.DownloadSpeedBytes) + "/s " + e.getTimeRemaing();
            JeremieLauncher.instance.pbDownload.Value = e.ProgressPercentage;
        }

        private void downloadComplete(object sender, EventArgs e)
        {
            /*if (e.Cancelled)
            {
                ((WebClient)sender).Dispose();
                await Task.Run(() => File.Delete(ZipFile));
                return;
            }*/
            Extracting = true;
            //await Utils.ExtractAllAsync(ZipFile, GameFolder, true, gameExtractProgressChanged);
            JeremieLauncher.instance.btnInstall.Enabled = false;
            if (Directory.Exists(GameFolder))
                Directory.Delete(GameFolder, true);
            FileExtract fe = new FileExtract(ZipFile, GameFolder, true);
            fe.ExtractProgressChanged += gameExtractProgressChanged;
            fe.ExtractCompleted += gameExtractCompleted;
            fe.StartExtract();
        }

        private void gameExtractCompleted(object sender, EventArgs e) {

            Extracting = false;
            //File.WriteAllText(VersionFile, Version.versionString);
            JeremieLauncher.instance.lblDownload.Text = "Finished " + (GameStatus == GameStatuses.NEEDINSTALL ? "Installing" : "Updating");
            checkGameStatus();
            updateButton();
            JeremieLauncher.instance.lblVersion.Text = "Installed Version: " + getInstalledVersion();
            Downloading = false;
            SetupGame();
        }

        private void gameExtractProgressChanged(object sender, ExtractProgressChangedEventArgs e)
        {
            JeremieLauncher.instance.lblDownload.Text = $"Extracting... {Utils.ConvertBytesToString(e.ExtractedSize)}/{Utils.ConvertBytesToString(e.ZipSize)} ({e.Progress}%)";
            JeremieLauncher.instance.pbDownload.Value = e.Progress;
        }

        private bool UpToDate()
        {
            return getInstalledVersion() >= Version;
        }

        private void checkGameStatus()
        {
            GameStatus = URL == "" ? GameStatuses.NOTAVAILABLE : (File.Exists(VersionFile) ? (UpToDate() ? GameStatuses.OK : GameStatuses.NEEDUPDATE) : GameStatuses.NEEDINSTALL);
        }

        private Version getInstalledVersion()
        {
            string version = "";
            if (File.Exists(VersionFile))
            {
                string ver = File.ReadAllLines(VersionFile)[0];
                string build = File.ReadAllLines(VersionFile)[1];
                version = ver+"."+build;
            }
            return Version.CreateFromString(version);
        }

        public void PauseDownload()
        {
            if (Downloading)
                fw.Pause();
        }

        public void ResumeDownload()
        {
            if (Downloading)
                fw.Start();
        }

        public void Uninstall()
        {
            if (File.Exists(VersionFile))
            {
                Directory.Delete(GameFolder, true);
                SetupGame();
            }
        }

        private FileDownload fw;
        public string GameFolder { get; }
        public string VersionFile { get; }
        public GameStatuses GameStatus { get; private set; }
        public string URL = "";
        public Version Version { get; private set; }
        public string CSVFileURL { get; }
        public string CSVFile { get; }
        public string ZipFile { get; }
        public string Executable { get; }
        public string GameName { get; }
        public string VidURL { get; private set; }
        public bool Downloading { get; private set; }
        public bool Extracting { get; private set; }
    }
}
