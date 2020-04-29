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
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        FileDownload fw = new FileDownload("https://docs.google.com/spreadsheets/d/1Fm1OlvmVw7n18MKRK2hoHZr0sBYpjWFSu0N9QooDW0w/export?format=csv&gid=0", "test.csv");

        private void Test_Load(object sender, EventArgs e)
        {
            fw.DownloadCompleted += dd;
            fw.ProgressChanged += tt;
        }

        private void dd(object sender, EventArgs e)
        {
            MessageBox.Show("te");
        }

        private void tt(object sender, DownloadProgressChangeEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ConvertDownloadedBytesToString()+"/"+e.ConvertTotalBytesToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fw.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fw.Pause();
        }
    }
}
