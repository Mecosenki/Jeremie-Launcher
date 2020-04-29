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
            this.lblLauncherVersion = new System.Windows.Forms.Label();
            this.btnOptions = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            this.GameMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Enabled = false;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.Location = new System.Drawing.Point(288, 132);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(166, 50);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(153, 226);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(500, 17);
            this.pbDownload.TabIndex = 1;
            // 
            // lblDownload
            // 
            this.lblDownload.AutoSize = true;
            this.lblDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownload.Location = new System.Drawing.Point(149, 203);
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
            // lblLauncherVersion
            // 
            this.lblLauncherVersion.AutoSize = true;
            this.lblLauncherVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLauncherVersion.Location = new System.Drawing.Point(12, 24);
            this.lblLauncherVersion.Name = "lblLauncherVersion";
            this.lblLauncherVersion.Size = new System.Drawing.Size(63, 20);
            this.lblLauncherVersion.TabIndex = 6;
            this.lblLauncherVersion.Text = "Version";
            // 
            // btnOptions
            // 
            this.btnOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptions.Location = new System.Drawing.Point(656, 27);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(132, 49);
            this.btnOptions.TabIndex = 7;
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // JeremieLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.lblLauncherVersion);
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
        private System.Windows.Forms.Label lblLauncherVersion;
        private System.Windows.Forms.Button btnOptions;
    }
}

