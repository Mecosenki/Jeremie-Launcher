namespace JeremieLauncher
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.chkCloseOnLeave = new System.Windows.Forms.CheckBox();
            this.btnSaveOptions = new System.Windows.Forms.Button();
            this.lblCheckUpdate = new System.Windows.Forms.Label();
            this.cbCheckUpdate = new System.Windows.Forms.ComboBox();
            this.chkRichPresence = new System.Windows.Forms.CheckBox();
            this.chkShowCustomGame = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkCloseOnLeave
            // 
            this.chkCloseOnLeave.AutoSize = true;
            this.chkCloseOnLeave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCloseOnLeave.Location = new System.Drawing.Point(12, 12);
            this.chkCloseOnLeave.Name = "chkCloseOnLeave";
            this.chkCloseOnLeave.Size = new System.Drawing.Size(269, 24);
            this.chkCloseOnLeave.TabIndex = 0;
            this.chkCloseOnLeave.Text = "Close Launcher On Game Startup";
            this.chkCloseOnLeave.UseVisualStyleBackColor = true;
            this.chkCloseOnLeave.CheckStateChanged += new System.EventHandler(this.chkCloseOnLeave_CheckStateChanged);
            // 
            // btnSaveOptions
            // 
            this.btnSaveOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveOptions.Location = new System.Drawing.Point(87, 284);
            this.btnSaveOptions.Name = "btnSaveOptions";
            this.btnSaveOptions.Size = new System.Drawing.Size(236, 60);
            this.btnSaveOptions.TabIndex = 1;
            this.btnSaveOptions.Text = "Save Options";
            this.btnSaveOptions.UseVisualStyleBackColor = true;
            this.btnSaveOptions.Click += new System.EventHandler(this.btnSaveOptions_Click);
            // 
            // lblCheckUpdate
            // 
            this.lblCheckUpdate.AutoSize = true;
            this.lblCheckUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckUpdate.Location = new System.Drawing.Point(8, 45);
            this.lblCheckUpdate.Name = "lblCheckUpdate";
            this.lblCheckUpdate.Size = new System.Drawing.Size(188, 20);
            this.lblCheckUpdate.TabIndex = 2;
            this.lblCheckUpdate.Text = "Check For Updates every";
            // 
            // cbCheckUpdate
            // 
            this.cbCheckUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheckUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckUpdate.FormattingEnabled = true;
            this.cbCheckUpdate.Items.AddRange(new object[] {
            "Off",
            "1 Minute",
            "5 Minutes",
            "10 Minutes",
            "30 Minutes",
            "60 Minutes"});
            this.cbCheckUpdate.Location = new System.Drawing.Point(202, 42);
            this.cbCheckUpdate.Name = "cbCheckUpdate";
            this.cbCheckUpdate.Size = new System.Drawing.Size(176, 28);
            this.cbCheckUpdate.TabIndex = 3;
            this.cbCheckUpdate.SelectedIndexChanged += new System.EventHandler(this.cbCheckUpdate_SelectedIndexChanged);
            // 
            // chkRichPresence
            // 
            this.chkRichPresence.AutoSize = true;
            this.chkRichPresence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRichPresence.Location = new System.Drawing.Point(12, 76);
            this.chkRichPresence.Name = "chkRichPresence";
            this.chkRichPresence.Size = new System.Drawing.Size(233, 24);
            this.chkRichPresence.TabIndex = 4;
            this.chkRichPresence.Text = "Show Discord Rich Presence";
            this.chkRichPresence.UseVisualStyleBackColor = true;
            this.chkRichPresence.CheckStateChanged += new System.EventHandler(this.chkRichPresence_CheckStateChanged);
            // 
            // chkShowCustomGame
            // 
            this.chkShowCustomGame.AutoSize = true;
            this.chkShowCustomGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowCustomGame.Location = new System.Drawing.Point(12, 106);
            this.chkShowCustomGame.Name = "chkShowCustomGame";
            this.chkShowCustomGame.Size = new System.Drawing.Size(249, 24);
            this.chkShowCustomGame.TabIndex = 5;
            this.chkShowCustomGame.Text = "Show Custom Game in Discord";
            this.chkShowCustomGame.UseVisualStyleBackColor = true;
            this.chkShowCustomGame.CheckedChanged += new System.EventHandler(this.chkShowCustomGame_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 356);
            this.Controls.Add(this.chkShowCustomGame);
            this.Controls.Add(this.chkRichPresence);
            this.Controls.Add(this.cbCheckUpdate);
            this.Controls.Add(this.lblCheckUpdate);
            this.Controls.Add(this.btnSaveOptions);
            this.Controls.Add(this.chkCloseOnLeave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCloseOnLeave;
        private System.Windows.Forms.Button btnSaveOptions;
        private System.Windows.Forms.Label lblCheckUpdate;
        private System.Windows.Forms.ComboBox cbCheckUpdate;
        private System.Windows.Forms.CheckBox chkRichPresence;
        private System.Windows.Forms.CheckBox chkShowCustomGame;
    }
}