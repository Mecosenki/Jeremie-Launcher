using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public static class Prompt
    {
        public static CustomGameData ShowDialog(string text, string caption, string defaultName="", string defaultLocation="")
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Font font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label selectLabel = new Label() { Left = 30, Top = 20, AutoSize = true, Text = text, Font = font };
            Label nameLabel = new Label() { Left = 30, Top = 80, AutoSize = true, Text = "Game Name", Font = font };
            TextBox gameLocationTextBox = new TextBox() { Left = 30, Top = 50, Width = 400, Text= defaultLocation };
            TextBox gameNameTextBox = new TextBox() { Left = 30, Top = 110, Width = 400, Text= defaultName };
            Button getGameLocationButton = new Button() { Left=gameLocationTextBox.Left+gameLocationTextBox.Width, Top=gameLocationTextBox.Top, Width=30, Height=gameLocationTextBox.Height, Text="..." };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 140, DialogResult = DialogResult.OK };
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Exe files|*.exe|All Files|*.*";
            fileDialog.FileOk += (sender, e)=> { var execFile = fileDialog.FileName; var list = execFile.Split('\\'); var gameNameExec= list[list.Length-1]; var gameName = gameNameExec.Substring(0, gameNameExec.Length-4); gameLocationTextBox.Text = execFile; gameNameTextBox.Text = gameName; };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            //textBox.Click += (sender, e) => { d.ShowDialog(); };
            getGameLocationButton.Click += (sender, e) => {fileDialog.ShowDialog(); };
            prompt.Controls.Add(gameLocationTextBox);
            prompt.Controls.Add(nameLabel);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(selectLabel);
            prompt.Controls.Add(getGameLocationButton);
            prompt.Controls.Add(gameNameTextBox);
            prompt.AcceptButton = confirmation;

            CustomGameData customGameData = new CustomGameData(prompt.ShowDialog(), gameNameTextBox.Text, gameLocationTextBox.Text);

            return customGameData;
        }
    }

    public struct CustomGameData
    {
        public readonly DialogResult dialogResult;
        public readonly string gameName, gameLocation;

        public CustomGameData(DialogResult dialogResult, string gameName, string gameLocation)
        {
            this.dialogResult = dialogResult;
            this.gameName = gameName;
            this.gameLocation = gameLocation;
        }
    }
}
