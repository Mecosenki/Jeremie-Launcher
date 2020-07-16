using DiscordRPC;
using DiscordRPC.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace KankrelatLauncher
{
    public partial class KankrelatLauncher : Form
    {
        private List<Game> games = new List<Game>();

        private Version launcherVersion = new Version(1, 2, 2, ignorebuild:true);
        private string launcherInfoFile = Utils.ApplicationFolder + "\\launcherInfo.csv";
        private string launcherSetupFile = Utils.ApplicationFolder + "\\setup_";
        private string launcherInfoURL = "https://docs.google.com/spreadsheets/d/1Djgo8S3R5TaLjLsWBlw9LVL4VRiARuFLIeI67c1PoZ0/export?format=csv&gid=0";
        private string ChangeLogURL = "";
        private bool Updating = false;
        public static string customGamesFile = "customGames.json";

        public static JeremieLauncher instance;

        public int index { get; private set; }
        private Timer checkUpdate_timer;
        private OptionsForm OptionsForm = new OptionsForm();

        public DiscordRpcClient client { get; private set; }
        private ulong startTime { get; }
        public RichPresence RichPresence;

        public KankrelatLauncher()
        {
            InitializeComponent();
            instance = this;
            Text += " " + launcherVersion.ToString();
            client = new DiscordRpcClient("718605351263535106");
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            client.Initialize();

            startTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            RichPresence = new RichPresence()
            {
                Details = "Kankrelat Launcher " + launcherVersion.ToString(),
                Assets = new Assets()
                {
                    LargeImageKey = "image"
                },
                Timestamps = new Timestamps()
                {
                    StartUnixMilliseconds = startTime
                }
            };

            client.SetPresence(RichPresence);

        }

        public static void setText(string text)
        {
            instance.lblGameName.Text = text;
            instance.lblGameName.Left = (instance.ClientSize.Width - instance.lblGameName.Width) / 2;
        }

        private async void KankrelatLauncher_Load(object sender, EventArgs e)
        {
            checkUpdate_timer = new Timer();
            checkUpdate_timer.Tick += timerUpdate_tick;
            Options.OptionsChanged += optionsChanged;
            Options.UpdateOptions();
            AddGame(new Game("IFSCL", "IFSCL", "https://docs.google.com/spreadsheets/d/1BgC6Pi7seuRMXFiAcqz65SOIdT88YI6Tr_LBBkAN61s/export?format=csv&gid=0", "http://bit.ly/changelogIFSCL"));
            AddGame(new Game("Lyoko Conquerors", "", "https://docs.google.com/spreadsheets/d/11fe4jmeoj-rbll9KH4cBme-a4SSYA09TTa4V_V-zz_s/export?format=csv&gid=0", ""));
            if (File.Exists(customGamesFile))
            {
                List<CustomGame> customGames = JsonConvert.DeserializeObject<List<CustomGame>>(File.ReadAllText(customGamesFile));
                foreach(CustomGame customGame in customGames) {
                    AddGame(customGame.ToGame());
                }
            }
            
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

        private void SwitchGame(int index, bool forced = false)
        {
            if (this.index != index || forced)
                if (index < games.Count && index >= 0)
                {
                    games[this.index].SwitchOutGame();

                    lblTrailer.Visible = !games[index].CustomGame;
                    pb_gif.Visible = !games[index].CustomGame;
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
            if (e.GetOption<bool>("discordRichPresence"))
            {
                if (!client.IsInitialized)
                {
                    client = new DiscordRpcClient("718605351263535106");
                    client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
                    client.Initialize();
                    client.SetPresence(RichPresence);
                }
            }
            else
            {
                if (client.IsInitialized)
                {
                    client.Deinitialize();
                }
            }
        }

        public void UpdateCustomGame(Game game)
        {
            GameMenuStrip.Items.RemoveAt(game.index);
            games[game.index].Clear();
            games.RemoveAt(game.index);
            games.Insert(game.index, game);
            ToolStripMenuItem GameMenuItem = new ToolStripMenuItem();
            GameMenuItem.AutoSize = true;
            GameMenuItem.Text = game.GameName;
            GameMenuStrip.Items.Insert(game.index, GameMenuItem);
            SwitchGame(game.index, true);
        }

        private void AddGame(Game game)
        {
            game.index = games.Count;
            games.Add(game);
            ToolStripMenuItem GameMenuItem = new ToolStripMenuItem();
            GameMenuItem.AutoSize = true;
            GameMenuItem.Text = game.GameName;
            //GameMenuStrip.Items.Add(game.GameName);
            GameMenuStrip.Items.Insert(GameMenuStrip.Items.Count - 1, GameMenuItem);
        }

        private void GameMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!Updating)
            {
                int index = GameMenuStrip.Items.IndexOf(e.ClickedItem);
                SwitchGame(index);
                if (index == GameMenuStrip.Items.Count - 1)
                {
                    AddCustomGame();
                }
            }
        }

        void AddCustomGame(string defaultName="", string defaultLocation="")
        {
            CustomGameData customGameData = Prompt.ShowDialog("Select Game Location", "Custom Game", defaultName, defaultLocation);
            if (customGameData.dialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(customGameData.gameName) || string.IsNullOrEmpty(customGameData.gameLocation))
                {
                    MessageBox.Show("Input fields must not be empty!");
                    AddCustomGame(customGameData.gameName, customGameData.gameLocation);
                    return;
                }
                if (!Utils.hasWriteAccessToFolder(Path.GetFullPath(Directory.GetCurrentDirectory())))
                {
                    Utils.StartApplicationInAdminMode();
                }
                CustomGame d = new CustomGame(customGameData.gameName, customGameData.gameLocation);
                List<CustomGame> g = new List<CustomGame>();
                if (File.Exists(customGamesFile))
                    g = JsonConvert.DeserializeObject<List<CustomGame>>(File.ReadAllText(customGamesFile));
                if (g.FindIndex(delegate(CustomGame custom)
                {
                    return custom.Equals(d);
                })>=0) {
                    MessageBox.Show("Custom game with same name already exists");
                    AddCustomGame(customGameData.gameName, customGameData.gameLocation);
                    return;
                }
                g.Add(d);
                string output = JsonConvert.SerializeObject(g, Formatting.Indented);
                File.WriteAllText(customGamesFile, output);
                AddGame(new Game(d.GameName, d.ExecPath, custom: true));
            }
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            OptionsForm.FormClosed += new FormClosedEventHandler((object sender_, FormClosedEventArgs e_) => { OptionsForm = new OptionsForm(); });
            OptionsForm.ShowDialog();
        }

        private void btnChangelog_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangeLogURL))
            {
                Utils.OpenURL(ChangeLogURL);
            }
        }

        private void btnBugs_Click(object sender, EventArgs e)
        {
            Utils.OpenURL("https://github.com/App24/Jeremie-Launcher/issues");
        }

        private void KankrelatLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Dispose();
        }
    }
}
