using Autofac;
using BroadCapture;
using BroadCapture.Domain;
using BroadCapture.Extensions;
using BroadCapture.Helpers;
using BroadCapture.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using RDapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Utilities.Enum;

namespace AndroGETracker
{
    class Program
    {
        #region console-hook
        static ConsoleEventDelegate handler;

        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                File.WriteAllText("broadconfig.json", JsonConvert.SerializeObject(Config.Instance, Formatting.Indented));
            }
            return false;
        }
        #endregion

        static async Task Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            using (var scope = ConfigurationContainer.Configure().BeginLifetimeScope())
            {
                var app = scope.Resolve<Application>();
                await app.RunAsync();
            }
        }
    }
}
