using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.UpdateOptionsFile();
        }

        private void chkCloseOnLeave_CheckStateChanged(object sender, EventArgs e)
        {
            Options.UpdateOption("closeOnLaunch", chkCloseOnLeave.Checked);
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            btnSaveOptions.Left = (ClientSize.Width - btnSaveOptions.Width) / 2;
            chkCloseOnLeave.Checked = Options.GetOption<bool>("closeOnLaunch");
        }

        private void btnSaveOptions_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
