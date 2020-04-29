using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace JeremieLauncher
{
    public partial class JeremieLauncher : Form
    {

        private string launcherInfoURL = "https://docs.google.com/spreadsheets/d/1Djgo8S3R5TaLjLsWBlw9LVL4VRiARuFLIeI67c1PoZ0/export?format=csv&gid=0";

        private Version launcherVersion = new Version(1, 0, 3, 35);

        private string launcherSetupFile = Utils.ApplicationFolder+"\\setup_";

        private string launcherInfoFile = "launcherInfo.csv";

        private Game currentGame;

        private Game IFSCLGame = new Game("IFSCL", "IFSCL.exe", "IFSCL", "ifsclVer", "https://docs.google.com/spreadsheets/d/1Fm1OlvmVw7n18MKRK2hoHZr0sBYpjWFSu0N9QooDW0w/export?format=csv&gid=0", "ifsclInfo", "ifscl");

        private Game LyokoConquerorsGames = new Game("Lyoko Conquerors","","Lyoko Conquerors", "clVer", "https://docs.google.com/spreadsheets/d/1GeWj18I8amY7Vhm5LY16ChOah7t0qQTUTu5ZgQlfpkY/export?format=csv&gid=0", "clInfo", "cl");

        private GameTypes selectedGame=GameTypes.NONE;

        private long nowBytes = 0;
        private long lastBytes = 0;
        private long downloadSpeedBytes = 0;

        private OptionsForm optionsForm= new OptionsForm();

        private Timer checkUpdate_timer;

        public JeremieLauncher()
        {
            InitializeComponent();
            instance = this;
        }

        private void JeremieLauncher_Load(object sender, EventArgs e)
        {
            checkUpdate_timer = new Timer();
            checkUpdate_timer.Tick += timerUpdate_tick;
            Options.OptionsChanged += optionsChanged;
            Options.UpdateOptions();
            lblLauncherVersion.Text = "Launcher Version: "+ launcherVersion.ToString();
            btnInstall.Left = (ClientSize.Width - btnInstall.Width) / 2;
            pbDownload.Left = (ClientSize.Width - pbDownload.Width) / 2;
            lblDownload.Location = new Point(pbDownload.Left, pbDownload.Top-pbDownload.Height-((int)Math.Round(lblDownload.Font.Size))/2);
            if (!Directory.Exists(Utils.ApplicationFolder))
            {
                Directory.CreateDirectory(Utils.ApplicationFolder);
            }
            checkUpdate();
        }

        private void checkUpdate()
        {
            //Utils.DownloadFile(launcherInfoURL, launcherInfoFile, null, checkUpdateFinished);
            FileDownload fw = new FileDownload(launcherInfoURL, launcherInfoFile);
            fw.DownloadCompleted += checkUpdateFinished;
            fw.Start();
        }

        private void checkUpdateFinished(object sender, EventArgs e)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            String[] lines = File.ReadAllLines(launcherInfoFile);
            foreach (string line in lines)
            {
                var lineValues = line.Split(',');

                values.Add(lineValues[0], lineValues[1]);
            }
            File.Delete(launcherInfoFile);

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

            string newVersion = "";
            string newVersionURL = "";

            foreach (KeyValuePair<string, string> item in values)
            {
                if (item.Key == "curVerNum." + os)
                {
                    newVersion = item.Value;
                    values.TryGetValue("URL." + os, out newVersionURL);
                }
            }

            launcherSetupFile += newVersion + ".exe";

            Version newVersionV = Version.CreateFromString(newVersion);

            string[] versionsS = newVersion.Split('.');
            int[] versions = new int[] { int.Parse(versionsS[0]), int.Parse(versionsS[1]), int.Parse(versionsS[2]), int.Parse(versionsS[3]) };

            if (newVersionV>launcherVersion)
            {
                if (MessageBox.Show("Theres an update for Jeremie Launcher, Do you want to download it?", "Update Available", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Timer timer = new Timer();
                    timer.Tick += new EventHandler(timer_tick);
                    timer.Interval = 1000;
                    timer.Start();
                    //Utils.DownloadFile(newVersionURL, launcherSetupFile, setupProgressChanged, setupFinishDownload);
                    FileDownload fw = new FileDownload(newVersionURL, launcherSetupFile);
                    fw.DownloadCompleted += setupFinishDownload;
                    fw.ProgressChanged += setupProgressChanged;
                    fw.Start();
                }
                else
                {
                    switchGame(GameTypes.IFSCL);
                }
            }
            else
            {
                switchGame(GameTypes.IFSCL);
            }
        }

        private void setupFinishDownload(object sender, EventArgs e)
        {
            Process.Start(launcherSetupFile, "/update=true");
            Environment.Exit(0);
        }

        private void setupProgressChanged(object sender, DownloadProgressChangeEventArgs e)
        {
            nowBytes = e.BytesReceived;
            lblDownload.Text = "Progress: " + e.ConvertDownloadedBytesToString() + "/" + e.ConvertTotalBytesToString() + " (" + e.ProgressPercentage.ToString() + "%) " + convertBytes(downloadSpeedBytes) + "/s " + getTimeRemaing(e.RemainingBytes);
            pbDownload.Value = (int)e.ProgressPercentage;
        }

        private void timer_tick(object sender, EventArgs e)
        {
            downloadSpeedBytes = nowBytes - lastBytes;
            lastBytes = nowBytes;
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
            return string.Format("{0:n" + (counter == 0 ? "0" : "2") + "} {1}", time, Utils.TimeSuffixes[counter]);
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
            return string.Format("{0:n2} {1}", number, Utils.FileSuffixes[counter]);
        }

        private void switchGame(GameTypes game)
        {
            if (game != selectedGame)
            {
                if (currentGame != null)
                {
                    /*if (currentGame.Downloading)
                    {
                        //MessageBox.Show("Downloading Game, Cannot switch game!");
                        //return;
                    }*/
                    currentGame.PauseDownload();
                }

                selectedGame = game;
                switch (game)
                {
                    case GameTypes.IFSCL:
                        currentGame = IFSCLGame;
                        break;
                    case GameTypes.LYOKOCONQUERORS:
                        currentGame = LyokoConquerorsGames;
                        break;
                    case GameTypes.NONE:
                        currentGame = null;
                        break;
                }

                if (currentGame != null)
                {
                    currentGame.SwitchGame();
                    currentGame.ResumeDownload();
                }
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            currentGame.downloadGame();
        }

        private void JeremieLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void JeremieLauncher_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        public static JeremieLauncher instance;

        private void pbVideo_Click(object sender, EventArgs e)
        {
            if (currentGame.VidURL != "")
            {
                Process.Start("https://youtu.be/"+currentGame.VidURL);
            }
        }

        private void iFSCLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchGame(GameTypes.IFSCL);
        }

        private void lyokoConquerorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchGame(GameTypes.LYOKOCONQUERORS);
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            //Very crude way of only allowing one options form open TODO: change this
            optionsForm.FormClosed += new FormClosedEventHandler((object sender_, FormClosedEventArgs e_)=> { optionsForm = new OptionsForm(); });
            optionsForm.Show();
        }

        private void timerUpdate_tick(object sender, EventArgs e)
        {
            checkUpdate();
        }

        private void optionsChanged(object sender, OptionsChangedEventArgs e)
        {
            if (e.GetOption<int>("checkUpdateTime") != 0)
            {
                checkUpdate_timer.Interval = Options.TimeSelections[e.GetOption<int>("checkUpdateTime")] * 60000;
                checkUpdate_timer.Start();
            }
            else
                checkUpdate_timer.Stop();
        }
    }
}
