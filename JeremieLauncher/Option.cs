using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public class Option
    {
        public Option(object value)
        {
            this.value = value;
        }

        private object value;
        public object Value { get { dynamic changedObj = Convert.ChangeType(value, value.GetType()); return changedObj; } }
        public Type Type { get { return value.GetType(); } }
    }
}
