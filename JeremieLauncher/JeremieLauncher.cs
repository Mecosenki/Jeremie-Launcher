using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JeremieLauncher
{
    public partial class JeremieLauncher : Form
    {

        public static bool is64BitProcess = (IntPtr.Size == 8);
        public static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

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

        public static string applicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\JeremieLauncher";

        public static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static string gamesFolder = "games\\";

        public static readonly string[] timeSuffixes = { "Seconds", "Minutes", "Hours" };

        private string launcherInfoURL = "https://docs.google.com/spreadsheets/d/1Djgo8S3R5TaLjLsWBlw9LVL4VRiARuFLIeI67c1PoZ0/export?format=csv&gid=0";

        private int launcherVersionMajor = 1;
        private int launcherVersionMinor = 0;
        private int launcherVersionPatch = 1;
        private int launcherVersionBuild = 3;

        private string launcherSetupFile = applicationFolder+"\\setup_";

        private string newVersion = "";

        private string newVersionURL = "";

        private string launcherInfoFile = "launcherInfo.csv";

        private Game currentGame;

        private Game IFSCLGame = new Game("IFSCL", "IFSCL.exe", "IFSCL", "ifsclVer", "https://docs.google.com/spreadsheets/d/1Fm1OlvmVw7n18MKRK2hoHZr0sBYpjWFSu0N9QooDW0w/export?format=csv&gid=0", "ifsclInfo", "ifscl");

        private Game LyokoConquerorsGames = new Game("Lyoko Conquerors","","Lyoko Conquerors", "clVer", "https://docs.google.com/spreadsheets/d/1GeWj18I8amY7Vhm5LY16ChOah7t0qQTUTu5ZgQlfpkY/export?format=csv&gid=0", "clInfo", "cl");

        private GameTypes selectedGame=GameTypes.NONE;

        private long nowBytes = 0;
        private long lastBytes = 0;
        private long downloadSpeedBytes = 0;

        public JeremieLauncher()
        {
            InitializeComponent();
            instance = this;
        }

        private string getLauncherVersion()
        {
            return launcherVersionMajor.ToString() + "." + launcherVersionMinor.ToString() + "." + launcherVersionPatch.ToString() + "." + launcherVersionBuild.ToString();
        }

        private void JeremieLauncher_Load(object sender, EventArgs e)
        {
            lblLauncherVersion.Text = "Launcher Version: "+ getLauncherVersion();
            btnInstall.Left = (ClientSize.Width - btnInstall.Width) / 2;
            pbDownload.Left = (ClientSize.Width - pbDownload.Width) / 2;
            lblDownload.Location = new Point(pbDownload.Left, pbDownload.Top-pbDownload.Height-((int)Math.Round(lblDownload.Font.Size))/2);
            if (!Directory.Exists(applicationFolder))
            {
                Directory.CreateDirectory(applicationFolder);
            }
            checkUpdate();
        }

        private void checkUpdate()
        {
            Utils.downloadFile(launcherInfoURL, launcherInfoFile, null, checkUpdateFinished);
        }

        private void checkUpdateFinished(object sender, AsyncCompletedEventArgs e)
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
                    if (is64BitOperatingSystem)
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
                    newVersion = item.Value;
                    values.TryGetValue("URL." + os, out newVersionURL);
                }
            }

            launcherSetupFile += newVersion + ".exe";

            string[] versionsS = newVersion.Split('.');
            int[] versions = new int[] { int.Parse(versionsS[0]), int.Parse(versionsS[1]), int.Parse(versionsS[2]), int.Parse(versionsS[3]) };

            if (!launcherUpToDate(versions))
            {
                if (MessageBox.Show("Theres an update for Jeremie Launcher, download?", "Update", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Timer timer = new Timer();
                    timer.Tick += new EventHandler(timer_tick);
                    timer.Interval = 1000;
                    timer.Start();
                    Utils.downloadFile(newVersionURL, launcherSetupFile, setupProgressChanged, setupFinishDownload);
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

        private void setupFinishDownload(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(launcherSetupFile, "/update=true");
            Environment.Exit(0);
        }

        private void setupProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            nowBytes = e.BytesReceived;
            lblDownload.Text = "Progress: " + convertBytes(e.BytesReceived) + "/" + convertBytes(e.TotalBytesToReceive) + " (" + e.ProgressPercentage.ToString() + "%) " + convertBytes(downloadSpeedBytes) + "/s " + getTimeRemaing(e.TotalBytesToReceive - e.BytesReceived);
            pbDownload.Value = e.ProgressPercentage;
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

        private void switchGame(GameTypes game)
        {
            if (game != selectedGame)
            {
                if (currentGame != null)
                {
                    if (currentGame.Downloading)
                    {
                        MessageBox.Show("Downloading Game, Cannot switch game!");
                        return;
                    }
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

        private bool launcherUpToDate(int[] versions)
        {
            if (versions[0] > launcherVersionMajor)
            {
                return false;
            }
            if (versions[1] > launcherVersionMinor)
            {
                return false;
            }
            if (versions[2] > launcherVersionPatch)
            {
                return false;
            }
            if (versions[3] > launcherVersionBuild)
            {
                return false;
            }
            return true;
        }
    }
}
