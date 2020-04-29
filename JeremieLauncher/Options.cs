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

        public static void UpdateOptions()
        {
            if (!File.Exists(optionsFile))
            {
                CreateOptions();
            }

            string[] lines = File.ReadAllLines(optionsFile);
            foreach (string line in lines)
            {
                string[] components = line.Split(':');
                options.Add(components[0], components[1]);
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
            options.Add("closeOnLaunch",false);
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
}
