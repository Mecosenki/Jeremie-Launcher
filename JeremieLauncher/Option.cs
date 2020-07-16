using Newtonsoft.Json;
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
            this._value = value;
        }

        [JsonConstructor]
        public Option(string name, object value)
        {
            Name = name;
            this._value = value;
        }

        private object _value;
        public string Name { get; }
        public object Value { get { dynamic changedObj = Convert.ChangeType(_value, _value.GetType()); return changedObj; } }
        //public Type Type { get { return value.GetType(); } }
    }
}
