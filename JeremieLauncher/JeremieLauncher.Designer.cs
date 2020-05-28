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
            this.lblGameName = new System.Windows.Forms.Label();
            this.lblTrailer = new System.Windows.Forms.Label();
            this.btnChangelog = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GameMenuStrip
            // 
            this.GameMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.GameMenuStrip.Name = "GameMenuStrip";
            this.GameMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.GameMenuStrip.TabIndex = 0;
            this.GameMenuStrip.Text = "menuStrip1";
            this.GameMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GameMenuStrip_ItemClicked);
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
            // JeremieLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jeremie Launcher";
            this.Load += new System.EventHandler(this.JeremieLauncher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip GameMenuStrip;
        private System.Windows.Forms.Label lblGameName;
        private System.Windows.Forms.Label lblTrailer;
        private System.Windows.Forms.Button btnChangelog;
        private System.Windows.Forms.Button btnOptions;
    }
}

