using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public static class Options
    {
        private static string optionsFile = "JeremieOptions.txt";

        private static Dictionary<string, object> options = new Dictionary<string, object>();

        public static readonly int[] TimeSelections= {0,1,5,10,30,60 };

        public static event OptionsChangedEventHandler OptionsChanged;

        public static void UpdateOptions()
        {
            if (!File.Exists(optionsFile))
            {
                CreateOptions();
            }

            string[] lines = File.ReadAllLines(optionsFile);
            options = new Dictionary<string, object>();
            foreach (string line in lines)
            {
                string[] components = line.Split(':');
                options.Add(components[0], components[1]);
            }

            if (OptionsChanged != null)
            {
                OptionsChanged?.Invoke(JeremieLauncher.instance, new OptionsChangedEventArgs(options));
            }
        }

        public static void UpdateOptionsFile()
        {
            string text = "";
            foreach (KeyValuePair<string, object> item in options)
            {
                text += $"{item.Key}:{item.Value}\n";
            }
            text = text.Substring(0, text.Length - 1);
            File.WriteAllText(optionsFile, text);
        }

        public static void UpdateOption(string name, object value)
        {
            if (options.ContainsKey(name))
                options.Remove(name);
            options.Add(name, value);
        }

        public static T GetOption<T>(string name)
        {
            object value;
            options.TryGetValue(name, out value);
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)Convert.ToString(value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)Convert.ToDecimal(value);
            }
            return default;
        }

        private static Dictionary<string, object> getDefaultOptions()
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("closeOnLaunch", true);
            options.Add("checkUpdateTime", 3);
            return options;
        }

        private static void CreateOptions()
        {
            string text = "";
            foreach(KeyValuePair<string, object> item in getDefaultOptions()) {
                text += $"{item.Key}:{item.Value}\n";
            }
            text=text.Substring(0, text.Length - 1);
            File.WriteAllText(optionsFile, text);
        }
    }

    public class OptionsChangedEventArgs : EventArgs
    {
        public OptionsChangedEventArgs(Dictionary<string, object> options)
        {
            Options = options;
        }

        public T GetOption<T>(string name)
        {
            object value;
            Options.TryGetValue(name, out value);
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)Convert.ToString(value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)Convert.ToDecimal(value);
            }
            return default;
        }

        public Dictionary<string, object> Options { get; }
    }

    public delegate void OptionsChangedEventHandler(object sender, OptionsChangedEventArgs e);
}
