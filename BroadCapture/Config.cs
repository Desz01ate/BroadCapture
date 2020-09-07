using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BroadCapture
{
    public class Config
    {
        private const string CONFIG_FILE = "broadconfig.json";
        public string DiscordBotToken { get; set; }
        public string CommandPrefix { get; set; }
        public static Config Instance { get; private set; }
        public bool Maintenance { get; set; }
        public bool DisabledOtherMessage { get; set; }
        public ulong[] Blocklisting { get; set; }
        public int Interval { get; set; }
        public string NpgsqlConnectionString { get; set; }
        static Config()
        {
            InitInstance();
            SetupConfigWatcher();
        }
        public bool IsInBlocklisting(ulong uid)
        {
            return Blocklisting.Contains(uid);
        }
        private static void InitInstance()
        {
            var retries = 0;
        retry:
            try
            {
                var content = File.ReadAllText(CONFIG_FILE);
                var inst = JsonConvert.DeserializeObject<Config>(content);
                Instance = inst;
            }
            catch (IOException)
            {
                if (retries++ < 10)
                {
                    Thread.Sleep(300);
                    goto retry;
                }
                throw;
            }

        }

        public Config()
        {

        }
        private static void SetupConfigWatcher()
        {
            var watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Path = Directory.GetCurrentDirectory();
            watcher.Filter = CONFIG_FILE;
            watcher.Changed += FileChangedConfigHandler;
            watcher.EnableRaisingEvents = true;
        }

        private static void FileChangedConfigHandler(object sender, FileSystemEventArgs e)
        {
            InitInstance();
            Console.WriteLine($"[{DateTime.Now}] Configuration file changed detected.");
        }
    }
}
