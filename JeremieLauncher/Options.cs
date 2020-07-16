using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public static class Options
    {
        private static string optionsFile = "JeremieOptions.json";

        public static Dictionary<string, Option> options = new Dictionary<string, Option>();

        public static readonly int[] TimeSelections = { 0, 1, 5, 10, 30, 60 };

        public static event OptionsChangedEventHandler OptionsChanged;

        public static void UpdateOptions()
        {
            if (!File.Exists(optionsFile))
            {
                UpdateOptionsFile(getDefaultOptions());
            }

            List<Option> options1 = JsonConvert.DeserializeObject<List<Option>>(File.ReadAllText(optionsFile));
            options = new Dictionary<string, Option>();
            foreach (Option option1 in options1)
            {
                options.Add(option1.Name, option1);
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
            List<Option> t = new List<Option>();
            if (File.Exists(optionsFile))
            {
                t = JsonConvert.DeserializeObject<List<Option>>(File.ReadAllText(optionsFile));
            }
            Dictionary<string, Option> dic = getDefaultOptions().Where(entry => !options.ContainsKey(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var pair in dic)
            {
                t.Add(new Option(pair.Key, pair.Value.Value));
            }
            string output = JsonConvert.SerializeObject(t, Formatting.Indented);
            File.WriteAllText(optionsFile, output);

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
                    if ((defaultOptions.TryGetValue(pair.Key, out value)))
                    {
                    }
                    else
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
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
            options.Add("discordRichPresence", new Option(true));
            options.Add("showCustomGameDiscord", new Option(true));
            return options;
        }

        public static void UpdateOptionsFile(Dictionary<string, Option> options)
        {
            if (!Utils.hasWriteAccessToFolder(Path.GetFullPath(optionsFile)))
            {
                Utils.StartApplicationInAdminMode();
            }
            List<Option> t = new List<Option>();
            foreach (KeyValuePair<string, Option> item in options)
            {
                t.Add(new Option(item.Key, item.Value.Value));
            }
            string output = JsonConvert.SerializeObject(t, Formatting.Indented);
            File.WriteAllText(optionsFile, output);
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
