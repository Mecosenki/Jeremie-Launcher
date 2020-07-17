using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public class Game
    {
        public string GameName { get; }
        public string GameFolder { get; }
        public string GameExec { get; }
        private string TrailerURL { get; set; }
        private string ChangelogURL { get; }
        private string CSVFileURL { get; }
        private string CSVFile { get; }
        private string VersionFile { get; }
        private bool GameInstalled { get { return File.Exists(VersionFile); } }
        private string ZipFile { get; }
        private string DownloadURL = "";
        private bool Downloading = false;
        private bool Extracting = false;

        public int index { get; set; }

        private Version NewVersion = new Version(-100, 0, 0, 0);
        public bool CustomGame { get; private set; }
        public bool HasTrailer { get; private set; }

        private ButtonType MainButtonType = ButtonType.NONE;
        private ButtonType SecondButtonType = ButtonType.NONE;
        private int secondButtonIndex = 0;
        private List<ButtonType> availableSecondButtonTypes = new List<ButtonType>();
        private GameStatus gameStatus;

        private List<Control> controls = new List<Control>();

        #region Controls
        public Label lblStatus;
        public ProgressBar pbProgress;
        private PictureBox pbTrailer;
        private Button btnMain;
        private Button btnSecond;
        private Button btnNext, btnPrevious;
        private Label lblVersion, lblNewVersion;
        private Button btnGameFolder;
        private Button btnChangelog;
        private Button btnEditCustomGame;
        private Button btnRemoveCustomGame;
        #endregion

        public Game(string gameName, string execName, string csvFileURL = "", string changelogUrl = "", bool custom = false)
        {
            GameName = gameName;
            if (!custom)
                GameFolder = Utils.GamesFolder + "\\" + gameName;
            else
                GameFolder = Path.GetDirectoryName(execName);
            if (!custom)
                GameExec = GameFolder + "\\" + execName + ".exe";
            else
                GameExec = execName;
            CSVFileURL = csvFileURL;
            ChangelogURL = changelogUrl;
            VersionFile = GameFolder + "\\" + execName + "_Data\\StreamingAssets\\dataV.dat";
            CSVFile = Utils.ApplicationFolder + "\\" + GameName + ".csv";
            ZipFile = Utils.GamesFolder + "\\" + GameName + ".zip";
            CustomGame = custom;
            HasTrailer = false;

            setupUI();
        }

        private void setupUI()
        {
            Font font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblStatus = new Label();
            pbProgress = new ProgressBar();
            pbTrailer = new PictureBox();
            btnMain = new Button();
            btnSecond = new Button();
            btnNext = new Button();
            btnPrevious = new Button();
            lblVersion = new Label();
            lblNewVersion = new Label();
            btnGameFolder = new Button();
            btnChangelog = new Button();
            btnEditCustomGame = new Button();
            btnRemoveCustomGame = new Button();

            lblStatus.Font = font;
            lblStatus.AutoSize = true;

            pbProgress.Location = new Point(0, 260);
            pbProgress.Size = new Size(500, 20);

            pbTrailer.Location = new Point(596, 330);
            pbTrailer.Size = new Size(192, 108);
            pbTrailer.SizeMode = PictureBoxSizeMode.StretchImage;
            pbTrailer.Click += pbTrailer_Click;

            btnMain.Location = new Point(0, 120);
            btnMain.Size = new Size(160, 50);
            btnMain.Text = "Install";
            btnMain.Font = font;
            btnMain.Click += btnMain_click;

            btnSecond.Location = new Point(0, 180);
            btnSecond.Font = font;
            btnSecond.Text = "Uninstall";
            btnSecond.Size = new Size(120, 40);
            btnSecond.Click += btnSecond_click;

            btnNext.Font = font;
            btnNext.Text = ">";
            btnNext.Click += btnNext_click;
            btnNext.Size = new Size(20, 40);

            btnPrevious.Font = font;
            btnPrevious.Text = "<";
            btnPrevious.Click += btnPrevious_click;
            btnPrevious.Size = new Size(20, 40);

            lblVersion.AutoSize = true;
            lblVersion.Font = font;
            lblVersion.Location = new Point(12, 421);
            lblVersion.Text = "Installed Version: ";

            lblNewVersion.AutoSize = true;
            lblNewVersion.Font = font;
            lblNewVersion.Location = new Point(12, 421 - font.Height - 5);
            lblNewVersion.Text = "";

            btnGameFolder.Font = font;
            btnGameFolder.Text = "Open Game Folder";
            btnGameFolder.Location = new Point(656, 120);
            btnGameFolder.Size = new Size(132, 59);
            btnGameFolder.Click += btnGameFolder_click;

            btnChangelog.Font = font;
            btnChangelog.Text = "Changelog";
            btnChangelog.Size = new Size(121, 53);
            btnChangelog.Location = new Point(12, 179);
            btnChangelog.Click += btnChangelog_click;

            btnEditCustomGame.Font = font;
            btnEditCustomGame.Text = "Edit Game Info";
            btnEditCustomGame.Size = new Size(121, 53);
            btnEditCustomGame.Location = new Point(12, 179);
            btnEditCustomGame.Click += btnEditGameInfo_Click;

            btnRemoveCustomGame.Font = font;
            btnRemoveCustomGame.Text = "Remove Game";
            btnRemoveCustomGame.Size = new Size(132, 53);
            btnRemoveCustomGame.Location = new Point(656, 244);
            btnRemoveCustomGame.Click += BtnRemoveCustomGame_Click;

            JeremieLauncher.instance.Controls.Add(lblStatus);
            JeremieLauncher.instance.Controls.Add(pbProgress);
            JeremieLauncher.instance.Controls.Add(btnMain);
            JeremieLauncher.instance.Controls.Add(btnSecond);
            JeremieLauncher.instance.Controls.Add(btnNext);
            JeremieLauncher.instance.Controls.Add(btnPrevious);
            if (!CustomGame)
            {
                JeremieLauncher.instance.Controls.Add(lblVersion);
                JeremieLauncher.instance.Controls.Add(lblNewVersion);
                JeremieLauncher.instance.Controls.Add(pbTrailer);
                JeremieLauncher.instance.Controls.Add(btnChangelog);
            }
            if (CustomGame)
            {
                JeremieLauncher.instance.Controls.Add(btnEditCustomGame);
                JeremieLauncher.instance.Controls.Add(btnRemoveCustomGame);
            }
            JeremieLauncher.instance.Controls.Add(btnGameFolder);

            CenterControl(pbProgress);
            CenterControl(btnMain);
            CenterControl(btnSecond);
            lblStatus.Location = new Point(pbProgress.Left, pbProgress.Top - pbProgress.Height - ((int)Math.Round(lblStatus.Font.Size)) / 2);
            btnNext.Location = new Point(btnSecond.Right + 5, btnSecond.Top);
            btnPrevious.Location = new Point(btnSecond.Left - btnPrevious.Width - 5, btnSecond.Top);

            controls.Add(lblStatus);
            controls.Add(pbProgress);
            controls.Add(pbTrailer);
            controls.Add(btnMain);
            controls.Add(btnSecond);
            controls.Add(btnPrevious);
            controls.Add(btnNext);
            controls.Add(lblVersion);
            controls.Add(lblNewVersion);
            controls.Add(btnGameFolder);
            controls.Add(btnChangelog);
            controls.Add(btnEditCustomGame);
            controls.Add(btnRemoveCustomGame);

            foreach (Control c in controls)
            {
                c.TabStop = false;
                c.Visible = false;
            }
        }

        #region Click Events

        #region Button
        private void btnNext_click(object sender, EventArgs e)
        {
            secondButtonIndex++;
            if (secondButtonIndex >= availableSecondButtonTypes.Count)
                secondButtonIndex = 0;
            UpdateUI();
        }

        private void btnPrevious_click(object sender, EventArgs e)
        {
            secondButtonIndex--;
            if (secondButtonIndex < 0)
                secondButtonIndex = availableSecondButtonTypes.Count - 1;
            UpdateUI();
        }

        private void btnMain_click(object sender, EventArgs e)
        {
            switch (MainButtonType)
            {
                case ButtonType.INSTALL:
                    downloadGame();
                    break;
                case ButtonType.PLAY:
                    var startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                    var programPath = Path.Combine(startupPath, GameExec);
                    Process p = Process.Start(programPath);
                    p.EnableRaisingEvents = true;
                    p.Exited += Game_Exited;
                    JeremieLauncher.instance.RichPresence.State = "Playing: " + GameName;
                    JeremieLauncher.instance.client.SetPresence(JeremieLauncher.instance.RichPresence);
                    if (Options.GetOption<bool>("closeOnLaunch"))
                        Environment.Exit(0);
                    break;
            }
            if (CustomGame)
            {
                Process p = Process.Start(GameExec);
                p.EnableRaisingEvents = true;
                p.Exited += Game_Exited;
                if (Options.GetOption<bool>("showCustomGameDiscord"))
                {
                    JeremieLauncher.instance.RichPresence.State = "Playing: "+GameName;
                    JeremieLauncher.instance.client.SetPresence(JeremieLauncher.instance.RichPresence);
                }
                if (Options.GetOption<bool>("closeOnLaunch"))
                    Environment.Exit(0);
            }
        }

        private void Game_Exited(object sender, EventArgs e)
        {
            JeremieLauncher.instance.RichPresence.State = "";
            JeremieLauncher.instance.client.SetPresence(JeremieLauncher.instance.RichPresence);
        }

        private void btnChangelog_click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ChangelogURL))
            {
                Utils.OpenURL(ChangelogURL);
            }
        }

        private void btnEditGameInfo_Click(object sender, EventArgs e)
        {
            EditCustomGame(GameName, GameExec);
        }

        public void EditCustomGame(string defaultName = "", string defaultLocation = "")
        {
            CustomGameData customGameData = Prompt.ShowDialog("Select Game Location", "Custom Game", defaultName, defaultLocation);
            if (customGameData.dialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(customGameData.gameName) || string.IsNullOrEmpty(customGameData.gameLocation))
                {
                    MessageBox.Show("Input fields must not be empty!");
                    EditCustomGame(customGameData.gameName, customGameData.gameLocation);
                    return;
                }
                if (!Utils.HasWriteAccessToFolder(Path.GetFullPath(Directory.GetCurrentDirectory())))
                {
                    Utils.StartApplicationInAdminMode();
                }
                CustomGame customGame = new CustomGame(customGameData.gameName, customGameData.gameLocation);
                List<CustomGame> g = new List<CustomGame>();
                if (File.Exists(JeremieLauncher.customGamesFile))
                    g = JsonConvert.DeserializeObject<List<CustomGame>>(File.ReadAllText(JeremieLauncher.customGamesFile));
                CustomGame currentCustomGame = ToCustomGame();
                int index = g.FindIndex(delegate (CustomGame custom)
                {
                    return custom.Equals(currentCustomGame);
                });
                if (index < 0)
                {
                    MessageBox.Show("Cannot edit game!");
                    return;
                }
                g.RemoveAt(index);
                g.Insert(index, customGame);
                string output = JsonConvert.SerializeObject(g, Formatting.Indented);
                File.WriteAllText(JeremieLauncher.customGamesFile, output);
                Game game = customGame.ToGame();
                game.index = this.index;
                JeremieLauncher.instance.UpdateCustomGame(game);
            }
        }

        private void BtnRemoveCustomGame_Click(object sender, EventArgs e)
        {
            RemoveGame();
        }

        public void RemoveGame()
        {
            List<CustomGame> g = new List<CustomGame>();
            if (File.Exists(JeremieLauncher.customGamesFile))
                g = JsonConvert.DeserializeObject<List<CustomGame>>(File.ReadAllText(JeremieLauncher.customGamesFile));
            CustomGame cur = ToCustomGame();
            int index = g.FindIndex(delegate (CustomGame custom)
            {
                return custom.Equals(cur);
            });
            if (index < 0)
            {
                MessageBox.Show("Cannot remove game!");
                return;
            }
            if (MessageBox.Show($"You are about to delete {GameName} are you sure you want to do it?", "Deleting Game", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                g.RemoveAt(index);
                string output = JsonConvert.SerializeObject(g, Formatting.Indented);
                File.WriteAllText(JeremieLauncher.customGamesFile, output);
                JeremieLauncher.instance.RemoveGame(this.index);
            }
        }

        private void btnGameFolder_click(object sender, EventArgs e)
        {
            if (GameInstalled || CustomGame)
            {
                Process.Start("explorer.exe", Path.GetFullPath(GameFolder));
            }
        }

        private void btnSecond_click(object sender, EventArgs e)
        {
            switch (SecondButtonType)
            {
                case ButtonType.UPDATE:
                    downloadGame();
                    break;
                case ButtonType.REPAIR:
                    downloadGame();
                    break;
                case ButtonType.UNINSTALL:
                    if (!Utils.HasWriteAccessToFolder(Directory.GetCurrentDirectory()))
                    {
                        Utils.StartApplicationInAdminMode();
                    }
                    btnMain.Enabled = false;
                    btnSecond.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    Directory.Delete(GameFolder, true);
                    UpdateUI();
                    break;
            }
        }

        #endregion

        #region Misc
        private void pbTrailer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TrailerURL))
            {
                Utils.OpenURL("https://youtu.be/" + TrailerURL);
            }
        }
        #endregion

        #endregion

        public async Task loadData()
        {
            if (!CustomGame)
            {
                FileDownload fw = new FileDownload(CSVFileURL, CSVFile);
                fw.DownloadCompleted += CSVDownloadComplete;
                await fw.Start();
            }
        }

        private void downloadCompleted(object sender, EventArgs e)
        {
            Extracting = true;
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
            lblStatus.Text = $"Finished Installing {GameName}!";
            //updateButton();
            Downloading = false;
            //Repairing = false;
            UpdateUI();
            //SetupGame();
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
                        os += "";
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
                    TrailerURL = item.Value;
                }
            }

            HasTrailer = !string.IsNullOrEmpty(TrailerURL);

            if (HasTrailer)
            {
                pbTrailer.LoadAsync("http://img.youtube.com/vi/" + TrailerURL + "/0.jpg");
                pbTrailer.Cursor = Cursors.Hand;
                if (index == JeremieLauncher.instance.index)
                {
                    JeremieLauncher.instance.lblTrailer.Visible = true;
                }
            }

            UpdateUI();
            if (hasUpdate())
            {
                //MessageBox.Show(Localisation.GetLocalisation("msg.newUpdate", GameName, NewVersion, getVersionInstalled()));
                MessageBox.Show("New Update for " + GameName + "\nNew Version: " + NewVersion.ToString() + "\nCurrent Installed Version: " + getVersionInstalled().ToString());

            }
            if (JeremieLauncher.instance.index == index)
            {
                if (UpToDate())
                {
                    JeremieLauncher.instance.pb_gif.Image = Properties.Resources.no_update;
                }
                else
                {
                    JeremieLauncher.instance.pb_gif.Image = Properties.Resources.update;
                }
            }
        }

        private void gameExtractProgressChanged(object sender, ExtractProgressChangedEventArgs e)
        {
            lblStatus.Text = $"Extracting... {e.Progress}%";
            pbProgress.Value = e.Progress;
        }

        private void downloadGame()
        {
            if (!string.IsNullOrEmpty(DownloadURL))
            {
                if (!Utils.HasWriteAccessToFolder(Path.GetFullPath(Utils.GamesFolder)))
                {
                    Utils.StartApplicationInAdminMode();
                }
                Downloading = true;
                btnMain.Enabled = false;
                btnSecond.Enabled = false;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                FileDownload fw = new FileDownload(DownloadURL, ZipFile);
                fw.ProgressChanged += downloadProgressChanged;
                fw.DownloadCompleted += downloadCompleted;
                fw.Start();
            }
        }

        private void downloadProgressChanged(object sender, DownloadProgressChangeEventArgs e)
        {
            if (JeremieLauncher.instance.index == index)
            {
                //Version tempVer = new Version(NewVersion.VersionMajor, NewVersion.VersionMinor, NewVersion.VersionPatch, NewVersion.VersionBuild, true);
                //JeremieLauncher.instance.RichPresence.State = $"Downloading {GameName} {tempVer}";
                //DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
                //dto.AddSeconds(e.getTimeRemainingLong());
                //JeremieLauncher.instance.RichPresence.Timestamps.EndUnixMilliseconds = (ulong) startdownloadtime + (ulong)(dto.ToUnixTimeMilliseconds()-startdownloadtime);
                //JeremieLauncher.instance.client.SetPresence(JeremieLauncher.instance.RichPresence);
            }
            lblStatus.Text = "Downloading: " + e.ConvertDownloadedBytesToString() + "/" + e.ConvertTotalBytesToString() + " (" + e.ProgressPercentage.ToString() + "%) " + e.ConvertBytesToString(e.DownloadSpeedBytes) + "/s " + e.getTimeRemaining();
            pbProgress.Value = e.ProgressPercentage;
        }

        private void CheckGameStatus()
        {
            availableSecondButtonTypes = new List<ButtonType>();
            FlagsHelper.Unset(ref gameStatus, GameStatus.INSTALLED);
            FlagsHelper.Unset(ref gameStatus, GameStatus.UPDATED);
            FlagsHelper.Unset(ref gameStatus, GameStatus.UNAVAILABLE);
            FlagsHelper.Unset(ref gameStatus, GameStatus.NONE);
            if (!string.IsNullOrEmpty(DownloadURL))
            {
                if (GameInstalled)
                {
                    MainButtonType = ButtonType.PLAY;
                    FlagsHelper.Set(ref gameStatus, GameStatus.INSTALLED);
                    if (!UpToDate())
                    {
                        availableSecondButtonTypes.Add(ButtonType.UPDATE);
                    }
                    else
                    {
                        FlagsHelper.Set(ref gameStatus, GameStatus.UPDATED);
                    }
                }
                else
                {
                    MainButtonType = ButtonType.INSTALL;
                    FlagsHelper.Set(ref gameStatus, GameStatus.NONE);
                }
            }
            else
            {
                MainButtonType = ButtonType.NONE;
                FlagsHelper.Set(ref gameStatus, GameStatus.UNAVAILABLE);
            }
            availableSecondButtonTypes.Add(ButtonType.UNINSTALL);
            availableSecondButtonTypes.Add(ButtonType.REPAIR);
        }

        private void UpdateUI()
        {
            CheckGameStatus();
            SecondButtonType = availableSecondButtonTypes[secondButtonIndex];
            lblVersion.Text = $"Installed Version: {getVersionInstalled()}";
            lblNewVersion.Text = "";
            btnGameFolder.Enabled = GameInstalled;
            btnChangelog.Enabled = !string.IsNullOrEmpty(ChangelogURL);
            switch (MainButtonType)
            {
                case ButtonType.INSTALL:
                    btnMain.Text = "Install";
                    btnSecond.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    lblStatus.Text = $"{GameName} needs to be installed!";
                    if (!Downloading && !Extracting)
                        btnMain.Enabled = true;
                    break;
                case ButtonType.PLAY:
                    btnMain.Text = "Play";
                    if (!Downloading && !Extracting)
                    {
                        lblStatus.Text = $"{GameName} is ready to be played!";
                        btnMain.Enabled = true;
                        btnSecond.Enabled = true;
                        btnPrevious.Enabled = true;
                        btnNext.Enabled = true;
                    }
                    break;
                case ButtonType.NONE:
                    btnMain.Enabled = false;
                    btnMain.Text = "Install";
                    btnSecond.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    lblStatus.Text = $"{GameName} is not available for your OS!";
                    break;
            }
            if (hasUpdate())
            {
                lblNewVersion.Text = $"New Version Available: {NewVersion}";
                lblStatus.Text = $"{GameName} needs to be updated!";
            }
            switch (SecondButtonType)
            {
                case ButtonType.UPDATE:
                    btnSecond.Text = "Update";
                    break;
                case ButtonType.UNINSTALL:
                    btnSecond.Text = "Uninstall";
                    break;
                case ButtonType.REPAIR:
                    btnSecond.Text = "Repair";
                    break;
            }
            if (CustomGame)
            {
                btnMain.Text = "Play";
                btnMain.Enabled = true;
                btnGameFolder.Enabled = true;
                lblStatus.Text = "";
            }
        }

        public void SwitchToGame()
        {
            UpdateUI();
            //if (!CustomGame)
            //JeremieLauncher.instance.RichPresence.State = GameName;
            //else
            //JeremieLauncher.instance.RichPresence.State = "";
            //JeremieLauncher.instance.client.SetPresence(JeremieLauncher.instance.RichPresence);
            JeremieLauncher.setText(GameName);
            foreach (Control c in controls)
            {
                c.Visible = true;
            }
            if (UpToDate())
            {
                JeremieLauncher.instance.pb_gif.Image = Properties.Resources.no_update;
            }
            else
            {
                JeremieLauncher.instance.pb_gif.Image = Properties.Resources.update;
            }
        }

        private bool hasUpdate()
        {
            return FlagsHelper.IsSet(gameStatus, GameStatus.INSTALLED) && !FlagsHelper.IsSet(gameStatus, GameStatus.UPDATED);
        }

        public void SwitchOutGame()
        {
            foreach (Control c in controls)
            {
                c.Visible = false;
            }
        }

        private bool UpToDate()
        {
            if (getVersionInstalled() >= NewVersion)
            {
                return true;
            }
            return false;
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

        public void Clear()
        {
            JeremieLauncher.instance.Controls.Remove(lblStatus);
            JeremieLauncher.instance.Controls.Remove(pbProgress);
            JeremieLauncher.instance.Controls.Remove(btnMain);
            JeremieLauncher.instance.Controls.Remove(btnSecond);
            JeremieLauncher.instance.Controls.Remove(btnNext);
            JeremieLauncher.instance.Controls.Remove(btnPrevious);
            if (!CustomGame)
            {
                JeremieLauncher.instance.Controls.Remove(lblVersion);
                JeremieLauncher.instance.Controls.Remove(lblNewVersion);
                JeremieLauncher.instance.Controls.Remove(pbTrailer);
                JeremieLauncher.instance.Controls.Remove(btnChangelog);
            }
            if (CustomGame)
            {
                JeremieLauncher.instance.Controls.Remove(btnEditCustomGame);
                JeremieLauncher.instance.Controls.Remove(btnRemoveCustomGame);
            }
            JeremieLauncher.instance.Controls.Remove(btnGameFolder);
        }

        private void CenterControl(Control c)
        {
            c.Left = (JeremieLauncher.instance.ClientSize.Width - c.Width) / 2;
        }

        public CustomGame ToCustomGame()
        {
            return new CustomGame(GameName, GameExec);
        }
    }

    [Flags]
    public enum GameStatus
    {
        NONE = 0,
        INSTALLED = 1,
        UPDATED = 2,
        UNAVAILABLE = 4
    }

    public enum ButtonType
    {
        INSTALL,
        UPDATE,
        UNINSTALL,
        REPAIR,
        PLAY,
        NONE
    }
}
