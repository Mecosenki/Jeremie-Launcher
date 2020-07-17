using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public class GameMenuItem : MenuItem
    {
        public Game Game { get; set; }
        public int GameIndex { get; set; }
    }
}
