namespace JeremieLauncher
{
    partial class JeremieLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JeremieLauncher));
            this.btnInstall = new System.Windows.Forms.Button();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.lblDownload = new System.Windows.Forms.Label();
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.GameMenuStrip = new System.Windows.Forms.MenuStrip();
            this.iFSCLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lyokoConquerorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOptions = new System.Windows.Forms.Button();
            this.lblGameName = new System.Windows.Forms.Label();
            this.btnGameFolder = new System.Windows.Forms.Button();
            this.btnChangelog = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.GameMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Enabled = false;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.Location = new System.Drawing.Point(290, 154);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(166, 50);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(136, 264);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(500, 17);
            this.pbDownload.TabIndex = 1;
            // 
            // lblDownload
            // 
            this.lblDownload.AutoSize = true;
            this.lblDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownload.Location = new System.Drawing.Point(147, 232);
            this.lblDownload.Name = "lblDownload";
            this.lblDownload.Size = new System.Drawing.Size(0, 20);
            this.lblDownload.TabIndex = 2;
            // 
            // pbVideo
            // 
            this.pbVideo.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbVideo.Location = new System.Drawing.Point(548, 303);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(240, 135);
            this.pbVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVideo.TabIndex = 3;
            this.pbVideo.TabStop = false;
            this.pbVideo.Click += new System.EventHandler(this.pbVideo_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(12, 421);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(135, 20);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Installed Version: ";
            // 
            // GameMenuStrip
            // 
            this.GameMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iFSCLToolStripMenuItem,
            this.lyokoConquerorsToolStripMenuItem});
            this.GameMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.GameMenuStrip.Name = "GameMenuStrip";
            this.GameMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.GameMenuStrip.TabIndex = 5;
            this.GameMenuStrip.Text = "menuStrip1";
            // 
            // iFSCLToolStripMenuItem
            // 
            this.iFSCLToolStripMenuItem.Name = "iFSCLToolStripMenuItem";
            this.iFSCLToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.iFSCLToolStripMenuItem.Text = "IFSCL";
            this.iFSCLToolStripMenuItem.Click += new System.EventHandler(this.iFSCLToolStripMenuItem_Click);
            // 
            // lyokoConquerorsToolStripMenuItem
            // 
            this.lyokoConquerorsToolStripMenuItem.Name = "lyokoConquerorsToolStripMenuItem";
            this.lyokoConquerorsToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.lyokoConquerorsToolStripMenuItem.Text = "Lyoko Conquerors";
            this.lyokoConquerorsToolStripMenuItem.Click += new System.EventHandler(this.lyokoConquerorsToolStripMenuItem_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptions.Location = new System.Drawing.Point(656, 194);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(132, 49);
            this.btnOptions.TabIndex = 7;
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // lblGameName
            // 
            this.lblGameName.AutoSize = true;
            this.lblGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameName.Location = new System.Drawing.Point(284, 81);
            this.lblGameName.Name = "lblGameName";
            this.lblGameName.Size = new System.Drawing.Size(0, 31);
            this.lblGameName.TabIndex = 8;
            // 
            // btnGameFolder
            // 
            this.btnGameFolder.Enabled = false;
            this.btnGameFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGameFolder.Location = new System.Drawing.Point(656, 139);
            this.btnGameFolder.Name = "btnGameFolder";
            this.btnGameFolder.Size = new System.Drawing.Size(132, 49);
            this.btnGameFolder.TabIndex = 9;
            this.btnGameFolder.Text = "Open Game Folder";
            this.btnGameFolder.UseVisualStyleBackColor = true;
            this.btnGameFolder.Click += new System.EventHandler(this.btnGameFolder_Click);
            // 
            // btnChangelog
            // 
            this.btnChangelog.Enabled = false;
            this.btnChangelog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangelog.Location = new System.Drawing.Point(12, 139);
            this.btnChangelog.Name = "btnChangelog";
            this.btnChangelog.Size = new System.Drawing.Size(132, 49);
            this.btnChangelog.TabIndex = 10;
            this.btnChangelog.Text = "Launcher Changelog";
            this.btnChangelog.UseVisualStyleBackColor = true;
            this.btnChangelog.Click += new System.EventHandler(this.btnChangelog_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUninstall.Location = new System.Drawing.Point(316, 210);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(126, 33);
            this.btnUninstall.TabIndex = 11;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Visible = false;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // JeremieLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.btnChangelog);
            this.Controls.Add(this.btnGameFolder);
            this.Controls.Add(this.lblGameName);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pbVideo);
            this.Controls.Add(this.lblDownload);
            this.Controls.Add(this.pbDownload);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.GameMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.GameMenuStrip;
            this.MaximizeBox = false;
            this.Name = "JeremieLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JeremieLauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JeremieLauncher_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.JeremieLauncher_FormClosed);
            this.Load += new System.EventHandler(this.JeremieLauncher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            this.GameMenuStrip.ResumeLayout(false);
            this.GameMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnInstall;
        public System.Windows.Forms.ProgressBar pbDownload;
        public System.Windows.Forms.Label lblDownload;
        public System.Windows.Forms.PictureBox pbVideo;
        public System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.MenuStrip GameMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem iFSCLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lyokoConquerorsToolStripMenuItem;
        private System.Windows.Forms.Button btnOptions;
        public System.Windows.Forms.Label lblGameName;
        public System.Windows.Forms.Button btnGameFolder;
        public System.Windows.Forms.Button btnChangelog;
        public System.Windows.Forms.Button btnUninstall;
    }
}

