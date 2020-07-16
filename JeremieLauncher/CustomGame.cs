using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeremieLauncher
{
    public class CustomGame
    {
        public CustomGame(string gameName, string execPath)
        {
            GameName = gameName;
            ExecPath = execPath;
        }

        public Game ToGame()
        {
            return new Game(GameName, ExecPath, custom:true);
        }

        public override string ToString()
        {
            return $"{GameName} {ExecPath}";
        }

        public override bool Equals(object other)
        {
            bool result;
            if (!(other is CustomGame))
            {
                result = false;
            }
            else
            {
                CustomGame custom = (CustomGame)other;
                result = (this.GameName == custom.GameName);
            }
            return result;
        }

        public string GameName { get; private set; }
        public string ExecPath { get; private set; }
    }
}
