using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public partial class JeremieLauncher : Form
    {
        private List<Game> games = new List<Game>();

        private Version launcherVersion = new Version(1, 1, 5, 150);
        private string launcherInfoFile = Utils.ApplicationFolder+ "\\launcherInfo.csv";
        private string launcherSetupFile = Utils.ApplicationFolder+ "\\setup_";
        private string launcherInfoURL = "https://docs.google.com/spreadsheets/d/1Djgo8S3R5TaLjLsWBlw9LVL4VRiARuFLIeI67c1PoZ0/export?format=csv&gid=0";
        private string ChangeLogURL = "";
        private bool Updating = false;

        public static JeremieLauncher instance;

        public int index { get; private set; }
        private Timer checkUpdate_timer;
        private OptionsForm OptionsForm = new OptionsForm();

        public JeremieLauncher()
        {
            InitializeComponent();
            instance = this;
            Text += " " + launcherVersion.ToString();
        }

        public static void setText(string text)
        {
            instance.lblGameName.Text = text;
            instance.lblGameName.Left = (instance.ClientSize.Width - instance.lblGameName.Width) / 2;
        }

        private async void JeremieLauncher_Load(object sender, EventArgs e)
        {
            checkUpdate_timer = new Timer();
            checkUpdate_timer.Tick += timerUpdate_tick;
            Options.OptionsChanged += optionsChanged;
            Options.UpdateOptions();
            AddGame(new Game("IFSCL", "IFSCL", "https://docs.google.com/spreadsheets/d/1Fm1OlvmVw7n18MKRK2hoHZr0sBYpjWFSu0N9QooDW0w/export?format=csv&gid=0", "http://bit.ly/changelogIFSCL"));
            AddGame(new Game("Lyoko Conquerors", "", "https://docs.google.com/spreadsheets/d/1GeWj18I8amY7Vhm5LY16ChOah7t0qQTUTu5ZgQlfpkY/export?format=csv&gid=0", ""));
            SwitchGame(0, true);
            await checkUpdate();

            if (!Updating)
            {
                foreach (Game game in games)
                {
                    await game.loadData();
                }
            }

            //SwitchGame(0, true);
        }

        private void SwitchGame(int index, bool forced=false)
        {
            if(this.index!=index||forced)
            if (index < games.Count && index >= 0)
            {
                games[this.index].SwitchOutGame();

                games[index].SwitchToGame();
                this.index = index;
            }
        }

        private async void timerUpdate_tick(object sender, EventArgs e)
        {
            await checkUpdate();
        }

        private async Task checkUpdate()
        {
            FileDownload fw = new FileDownload(launcherInfoURL, launcherInfoFile);
            fw.DownloadCompleted += checkUpdateFinished;
            await fw.Start();
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
                if (item.Key == "doc.changelog")
                    ChangeLogURL = item.Value;
            }

            launcherSetupFile += newVersion + ".exe";

            if (ChangeLogURL != "")
            {
                btnChangelog.Enabled = true;
            }

            Version newVersionV = Version.CreateFromString(newVersion);

            string[] versionsS = newVersion.Split('.');
            int[] versions = new int[] { int.Parse(versionsS[0]), int.Parse(versionsS[1]), int.Parse(versionsS[2]), int.Parse(versionsS[3]) };

            if (newVersionV > launcherVersion)
            {
                if (MessageBox.Show("Theres an update for Jeremie Launcher, Do you want to download it?", "Update Available", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    FileDownload fw = new FileDownload(newVersionURL, launcherSetupFile);
                    fw.DownloadCompleted += setupFinishDownload;
                    fw.ProgressChanged += setupProgressChanged;
                    Updating = true;
                    fw.Start();
                }
            }
        }

        private void setupFinishDownload(object sender, EventArgs e)
        {
            Process.Start(launcherSetupFile, "/update=true");
            Environment.Exit(0);
        }

        private void setupProgressChanged(object sender, DownloadProgressChangeEventArgs e)
        {
            Game game = games[index];
            game.lblStatus.Text = "Progress: " + e.ConvertDownloadedBytesToString() + "/" + e.ConvertTotalBytesToString() + " (" + e.ProgressPercentage.ToString() + "%) " + e.ConvertBytesToString(e.DownloadSpeedBytes) + "/s " + e.getTimeRemaining();
            game.pbProgress.Value = (int)e.ProgressPercentage;
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

        private void AddGame(Game game)
        {
            game.index = games.Count;
            games.Add(game);
            GameMenuStrip.Items.Add(game.GameName);
        }

        private void GameMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!Updating)
            {
                int index = GameMenuStrip.Items.IndexOf(e.ClickedItem);
                SwitchGame(index);
            }
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            OptionsForm.FormClosed += new FormClosedEventHandler((object sender_, FormClosedEventArgs e_) => { OptionsForm = new OptionsForm(); });
            OptionsForm.Show();
        }

        private void btnChangelog_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangeLogURL))
            {
                Utils.OpenURL(ChangeLogURL);
            }
        }
    }
}
