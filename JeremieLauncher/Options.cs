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

        private static Dictionary<string, Option> options = new Dictionary<string, Option>();

        public static readonly int[] TimeSelections = { 0, 1, 5, 10, 30, 60 };

        public static event OptionsChangedEventHandler OptionsChanged;

        public static void UpdateOptions()
        {
            if (!File.Exists(optionsFile))
            {
                CreateOptions();
            }

            string[] lines = File.ReadAllLines(optionsFile);
            options = new Dictionary<string, Option>();
            foreach (string line in lines)
            {
                string[] components = line.Split(':');
                Option option = new Option(components[1]);
                Option optionType;
                if (getDefaultOptions().TryGetValue(components[0], out optionType))
                {
                    options.Add(components[0], new Option(Convert.ChangeType(option.Value, optionType.Type)));
                }
            }
            if (!checkOptions())
            {
                writeMissing();
                return;
            }

            if (OptionsChanged != null)
            {
                OptionsChanged?.Invoke(JeremieLauncher.instance, new OptionsChangedEventArgs(options));
            }
        }

        private static void writeMissing()
        {
            Dictionary<string, Option> dic = getDefaultOptions().Where(entry => !options.ContainsKey(entry.Key)).ToDictionary(entry=>entry.Key, entry=>entry.Value);
            foreach (var pair in dic)
            {
                File.AppendAllText(optionsFile, $"{pair.Key}:{pair.Value.Value}\n");
            }
            UpdateOptions();
        }

        private static bool checkOptions()
        {
            bool equal = false;
            Dictionary<string, Option> defaultOptions = getDefaultOptions();
            if (options.Count == defaultOptions.Count)
            {
                equal = true;
                foreach (var pair in options)
                {
                    Option value;
                    if ((defaultOptions.TryGetValue(pair.Key, out value))){
                    } else {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }

        public static void UpdateOptionsFile()
        {
            string text = "";
            foreach (KeyValuePair<string, Option> item in options)
            {
                text += $"{item.Key}:{item.Value.Value}\n";
            }
            File.WriteAllText(optionsFile, text);
        }

        public static void UpdateOption(string name, object value)
        {
            if (options.ContainsKey(name))
                options.Remove(name);
            options.Add(name, new Option(value));
        }

        public static T GetOption<T>(string name)
        {
            Option value;
            options.TryGetValue(name, out value);
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value.Value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value.Value);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)Convert.ToString(value.Value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)Convert.ToDecimal(value.Value);
            }
            return default;
        }

        private static Dictionary<string, Option> getDefaultOptions()
        {
            Dictionary<string, Option> options = new Dictionary<string, Option>();
            options.Add("closeOnLaunch", new Option(true));
            options.Add("checkUpdateTime", new Option(3));
            return options;
        }

        private static void CreateOptions()
        {
            string text = "";
            foreach (KeyValuePair<string, Option> item in getDefaultOptions())
            {
                text += $"{item.Key}:{item.Value.Value}\n";
            }
            File.WriteAllText(optionsFile, text);
        }
    }

    public class OptionsChangedEventArgs : EventArgs
    {
        public OptionsChangedEventArgs(Dictionary<string, Option> options)
        {
            Options = options;
        }

        public T GetOption<T>(string name)
        {
            Option value;
            Options.TryGetValue(name, out value);
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value.Value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value.Value);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)Convert.ToString(value.Value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)Convert.ToDecimal(value.Value);
            }
            return default;
        }

        public Dictionary<string, Option> Options { get; }
    }

    public delegate void OptionsChangedEventHandler(object sender, OptionsChangedEventArgs e);
}
