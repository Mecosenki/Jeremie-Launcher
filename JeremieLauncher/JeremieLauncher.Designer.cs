using System;
using System.Windows.Forms;

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
            this.GameMenuStrip = new System.Windows.Forms.MenuStrip();
            this.addCustomGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblGameName = new System.Windows.Forms.Label();
            this.lblTrailer = new System.Windows.Forms.Label();
            this.btnChangelog = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.pb_gif = new System.Windows.Forms.PictureBox();
            this.btnBugs = new System.Windows.Forms.Button();
            this.GameMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_gif)).BeginInit();
            this.SuspendLayout();
            // 
            // GameMenuStrip
            // 
            this.GameMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCustomGameToolStripMenuItem});
            this.GameMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.GameMenuStrip.Name = "GameMenuStrip";
            this.GameMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.GameMenuStrip.TabIndex = 0;
            this.GameMenuStrip.Text = "menuStrip1";
            this.GameMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GameMenuStrip_ItemClicked);
            // 
            // addCustomGameToolStripMenuItem
            // 
            this.addCustomGameToolStripMenuItem.Name = "addCustomGameToolStripMenuItem";
            this.addCustomGameToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.addCustomGameToolStripMenuItem.Text = "Add Custom Game";
            // 
            // lblGameName
            // 
            this.lblGameName.AutoSize = true;
            this.lblGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameName.Location = new System.Drawing.Point(306, 70);
            this.lblGameName.Name = "lblGameName";
            this.lblGameName.Size = new System.Drawing.Size(0, 31);
            this.lblGameName.TabIndex = 1;
            // 
            // lblTrailer
            // 
            this.lblTrailer.AutoSize = true;
            this.lblTrailer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrailer.Location = new System.Drawing.Point(592, 307);
            this.lblTrailer.Name = "lblTrailer";
            this.lblTrailer.Size = new System.Drawing.Size(101, 20);
            this.lblTrailer.TabIndex = 3;
            this.lblTrailer.Text = "Latest Trailer";
            // 
            // btnChangelog
            // 
            this.btnChangelog.Enabled = false;
            this.btnChangelog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangelog.Location = new System.Drawing.Point(12, 120);
            this.btnChangelog.Name = "btnChangelog";
            this.btnChangelog.Size = new System.Drawing.Size(121, 53);
            this.btnChangelog.TabIndex = 1;
            this.btnChangelog.Text = "Launcher Changelog";
            this.btnChangelog.UseVisualStyleBackColor = true;
            this.btnChangelog.Click += new System.EventHandler(this.btnChangelog_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptions.Location = new System.Drawing.Point(656, 185);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(132, 53);
            this.btnOptions.TabIndex = 2;
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // pb_gif
            // 
            this.pb_gif.Location = new System.Drawing.Point(329, 291);
            this.pb_gif.Name = "pb_gif";
            this.pb_gif.Size = new System.Drawing.Size(191, 147);
            this.pb_gif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_gif.TabIndex = 4;
            this.pb_gif.TabStop = false;
            // 
            // btnBugs
            // 
            this.btnBugs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBugs.Location = new System.Drawing.Point(12, 238);
            this.btnBugs.Name = "btnBugs";
            this.btnBugs.Size = new System.Drawing.Size(121, 69);
            this.btnBugs.TabIndex = 5;
            this.btnBugs.Text = "Report Launcher Bugs";
            this.btnBugs.UseVisualStyleBackColor = true;
            this.btnBugs.Click += new System.EventHandler(this.btnBugs_Click);
            // 
            // JeremieLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnBugs);
            this.Controls.Add(this.pb_gif);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.btnChangelog);
            this.Controls.Add(this.lblTrailer);
            this.Controls.Add(this.lblGameName);
            this.Controls.Add(this.GameMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.GameMenuStrip;
            this.MaximizeBox = false;
            this.Name = "JeremieLauncher";
            this.Text = "Jeremie Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JeremieLauncher_FormClosing);
            this.Load += new System.EventHandler(this.JeremieLauncher_Load);
            this.GameMenuStrip.ResumeLayout(false);
            this.GameMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_gif)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip GameMenuStrip;
        private System.Windows.Forms.Label lblGameName;
        public System.Windows.Forms.Label lblTrailer;
        private System.Windows.Forms.Button btnChangelog;
        private System.Windows.Forms.Button btnOptions;
        public System.Windows.Forms.PictureBox pb_gif;
        private System.Windows.Forms.ToolStripMenuItem addCustomGameToolStripMenuItem;
        private System.Windows.Forms.Button btnBugs;
    }
}

