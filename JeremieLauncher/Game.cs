using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public class Game
    {
        public Game(string gameName, string executableName, string csvFileURL)
        {
            GameName = gameName;
            GameFolder = Utils.GamesFolder + "\\" + GameName;
            Executable = GameFolder+"\\"+ executableName+".exe";
            CSVFileURL = csvFileURL;
            VersionFile = GameFolder + "\\" + executableName + "_Data\\StreamingAssets\\dataV.dat";
            CSVFile = Utils.GamesFolder + "\\" + GameName + ".csv";
            ZipFile = Utils.GamesFolder + "\\" + GameName + ".zip";

            setupUI();
        }

        private void setupUI()
        {
            Font font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Font font1 = new Font("Microsoft Sans Serif", 20, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            btnInstall = new Button();
            btnUninstall = new Button();
            lblGameName = new Label();
            pbDownload = new ProgressBar();
            lblStatus = new Label();
            pbVideo = new PictureBox();
            lblVersion = new Label();
            lblNewVersion = new Label();
            btnGameFolder = new Button();

            btnInstall.Location = new Point(0, 120);
            btnInstall.Size = new Size(160, 50);
            btnInstall.Visible = false;
            btnInstall.Text = "Install";
            btnInstall.Font = font;
            btnInstall.Click += download_click;

            btnUninstall.Font = font;
            btnUninstall.Location = new Point(0, 180);
            btnUninstall.Size = new Size(120, 40);
            btnUninstall.Text = "Uninstall";
            btnUninstall.Visible = false;
            btnUninstall.Click += uninstall_click;

            lblGameName.AutoSize = true;
            lblGameName.Location = new Point(0, 70);
            lblGameName.Font = font1;
            lblGameName.Text = GameName;
            lblGameName.Visible = false;

            pbDownload.Location = new Point(0, 270);
            pbDownload.Size = new Size(500, 20);
            pbDownload.Visible = false;

            lblStatus.Font = font;
            lblStatus.AutoSize = true;
            pbDownload.Visible = false;

            pbVideo.Cursor = Cursors.Default;
            pbVideo.Location = new Point(548, 303);
            pbVideo.Size = new Size(240, 135);
            pbVideo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbVideo.Click += video_click;
            pbVideo.Visible = false;

            lblVersion.AutoSize = true;
            lblVersion.Font = font;
            lblVersion.Location = new Point(12, 421);
            lblVersion.Text = "Installed Version: ";
            lblVersion.Visible = false;

            lblNewVersion.AutoSize = true;
            lblNewVersion.Font = font;
            lblNewVersion.Location = new Point(12, 421-font.Height-5);
            lblNewVersion.Text = "";
            lblNewVersion.Visible = false;

            btnGameFolder.Visible = false;
            btnGameFolder.Font = font;
            btnGameFolder.Text = "Open Game Folder";
            btnGameFolder.Location = new Point(656, 120);
            btnGameFolder.Size = new Size(132, 59);

            JeremieLauncher.instance.Controls.Add(btnInstall);
            JeremieLauncher.instance.Controls.Add(btnUninstall);
            JeremieLauncher.instance.Controls.Add(lblGameName);
            JeremieLauncher.instance.Controls.Add(pbDownload);
            JeremieLauncher.instance.Controls.Add(lblStatus);
            JeremieLauncher.instance.Controls.Add(pbVideo);
            JeremieLauncher.instance.Controls.Add(lblVersion);
            JeremieLauncher.instance.Controls.Add(lblNewVersion);
            JeremieLauncher.instance.Controls.Add(btnGameFolder);

            CenterControl(lblGameName);
            CenterControl(btnUninstall);
            CenterControl(btnInstall);
            CenterControl(pbDownload);
            lblStatus.Location = new Point(pbDownload.Left, pbDownload.Top - pbDownload.Height - ((int)Math.Round(lblStatus.Font.Size)) / 2);
        }

        public async Task loadData()
        {
            FileDownload fw = new FileDownload(CSVFileURL, CSVFile);
            fw.DownloadCompleted += CSVDownloadComplete;
            await fw.Start();
        }

        private void checkGameStatus()
        {
            if (DownloadURL == "")
            {
                GameStatus = GameStatus.NOTAVAILABLE;
            }
            else if (GameInstalled)
            {
                if (getVersionInstalled() >= NewVersion)
                    GameStatus = GameStatus.OK;
                else
                    GameStatus = GameStatus.NEEDUPDATE;
            }
            else
            {
                GameStatus = GameStatus.NEEDINSTALL;
            }
        }


        private Version getVersionInstalled()
        {
            string version = "";
            if (File.Exists(VersionFile))
            {
                string ver = File.ReadAllLines(VersionFile)[0];
                string build = File.ReadAllLines(VersionFile)[1];
                version = ver + "." + build;
            }
            return Version.CreateFromString(version);
        }

        private void CSVDownloadComplete(object sender, EventArgs e)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            string[] lines = File.ReadAllLines(CSVFile);

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
                    NewVersion = Version.CreateFromString(item.Value);
                    values.TryGetValue("URL." + os, out DownloadURL);
                }
                if (item.Key == "trailerID")
                {
                    VideoURL = item.Value;
                }
            }

            pbVideo.Load("http://img.youtube.com/vi/" + VideoURL + "/0.jpg");
            if (VideoURL != "")
            {
                pbVideo.Cursor = Cursors.Hand;
            }
        }

        private void uninstall_click(object sender, EventArgs e)
        {
            Directory.Delete(GameFolder, true);
            updateUI();
        }

        private void download_click(object sender, EventArgs e)
        {
            switch (GameStatus) {
                case GameStatus.OK:
                    var startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                    var programPath = Path.Combine(startupPath, Executable);
                    Process.Start(programPath);
                    if (Options.GetOption<bool>("closeOnLaunch"))
                        Environment.Exit(0);
                    break;
                case GameStatus.NEEDINSTALL:
                case GameStatus.NEEDUPDATE:
                    btnInstall.Enabled = false;
                    btnUninstall.Enabled = false;
                    if (DownloadURL != "")
                    {
                        //Utils.DownloadFile(URL, ZipFile, downloadProgressChange, downloadComplete);
                        FileDownload fw = new FileDownload(DownloadURL, ZipFile);
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
            lblStatus.Text = "Progress: " + e.ConvertDownloadedBytesToString() + "/" + e.ConvertTotalBytesToString() + " (" + e.ProgressPercentage.ToString() + "%) " + e.ConvertBytesToString(e.DownloadSpeedBytes) + "/s " + e.getTimeRemaing();
            pbDownload.Value = e.ProgressPercentage;
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
            btnInstall.Enabled = false;
            if (Directory.Exists(GameFolder))
                Directory.Delete(GameFolder, true);
            FileExtract fe = new FileExtract(ZipFile, GameFolder, true);
            fe.ExtractProgressChanged += gameExtractProgressChanged;
            fe.ExtractCompleted += gameExtractCompleted;
            fe.StartExtract();
        }

        private void gameExtractCompleted(object sender, EventArgs e)
        {

            Extracting = false;
            //File.WriteAllText(VersionFile, Version.versionString);
            lblStatus.Text = "Finished " + (GameStatus == GameStatus.NEEDINSTALL ? "Installing" : "Updating");
            //updateButton();
            Downloading = false;
            updateUI();
            //SetupGame();
        }

        private void gameExtractProgressChanged(object sender, ExtractProgressChangedEventArgs e)
        {
            lblStatus.Text = $"Extracting... ({e.Progress}%)";
            pbDownload.Value = e.Progress;
        }

        private void video_click(object sender, EventArgs e)
        {
            if (VideoURL != "")
                Process.Start("https://youtu.be/" + VideoURL);
        }

        private void CenterControl(Control c)
        {
            c.Left = (JeremieLauncher.instance.ClientSize.Width - c.Width) / 2;
        }

        private void updateUI()
        {
            checkGameStatus();
            lblVersion.Text = "Installed Version: " + getVersionInstalled();
            lblNewVersion.Text = "";
            switch (GameStatus)
            {
                case GameStatus.NEEDINSTALL:
                    if(!Downloading&&!Extracting)
                    btnInstall.Enabled = true;
                    btnInstall.Text = "Install";
                    lblStatus.Text = GameName + " needs to be installed!";
                    btnUninstall.Enabled = false;
                    btnGameFolder.Enabled = false;
                    break;
                case GameStatus.NEEDUPDATE:
                    btnInstall.Text = "Update";
                    lblStatus.Text = GameName + " needs to be updated!";
                    lblNewVersion.Text = "New Version Available: "+ NewVersion;
                    if (!Downloading && !Extracting)
                    {
                        btnInstall.Enabled = true;
                        btnUninstall.Enabled = true;
                    }
                    btnGameFolder.Enabled = true;
                    break;
                case GameStatus.NOTAVAILABLE:
                    btnInstall.Text = "Install";
                    lblStatus.Text = GameName + " is not availbe for your OS!";
                    btnInstall.Enabled = false;
                    btnUninstall.Enabled = false;
                    btnGameFolder.Enabled = false;
                    break;
                case GameStatus.OK:
                    btnInstall.Text = "Play";
                    lblStatus.Text = GameName + " is installed and up to date!";
                    btnInstall.Enabled = true;
                    btnUninstall.Enabled = true;
                    btnGameFolder.Enabled = true;
                    break;
            }
        }

        public void SwitchToGame()
        {
            updateUI();
            btnInstall.Visible = true;
            btnUninstall.Visible = true;
            lblGameName.Visible = true;
            pbVideo.Visible = true;
            lblStatus.Visible = true;
            pbDownload.Visible = true;
            lblVersion.Visible = true;
            lblNewVersion.Visible = true;
            btnGameFolder.Visible = true;
        }

        public void SwitchOutGame()
        {
            btnInstall.Visible = false;
            btnUninstall.Visible = false;
            lblGameName.Visible = false;
            pbVideo.Visible = false;
            lblStatus.Visible = false;
            pbDownload.Visible = false;
            lblVersion.Visible = false;
            lblNewVersion.Visible = false;
            btnGameFolder.Visible = false;
        }

        public Button btnInstall { get; private set; }
        public Button btnUninstall { get; private set; }
        public Label lblGameName { get; private set; }
        public ProgressBar pbDownload { get; private set; }
        public Label lblStatus { get; private set; }
        public PictureBox pbVideo { get; private set; }
        public Label lblVersion { get; private set; }
        public Label lblNewVersion { get; private set; }
        public Button btnGameFolder { get; private set; }

        public string GameName { get; }
        public string Executable { get; }
        public string CSVFileURL { get; }
        public int index { get; set; }
        public string CSVFile { get; }
        public string ZipFile { get; }
        public string DownloadURL = "";
        public string VideoURL { get; private set; }
        public Version NewVersion { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public string VersionFile { get; }
        public string GameFolder { get; }
        private bool GameInstalled { get { return File.Exists(VersionFile); } }
        public bool Downloading { get; private set; }
        public bool Extracting { get; private set; }
    }
}
