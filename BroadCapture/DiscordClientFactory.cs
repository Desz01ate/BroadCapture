using AndroGETracker;
using BroadCapture.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture
{
    public class DiscordClientFactory
    {
        public readonly DiscordClient Client;
        private readonly List<DiscordChannel> channels;

        private DiscordClientFactory()
        {
            channels = new List<DiscordChannel>();
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Config.Instance.DiscordBotToken,
                TokenType = TokenType.Bot,
                LogLevel = LogLevel.Info,
                UseInternalLogHandler = true,
                AutoReconnect = true
            });
            Client.GuildDownloadCompleted += DiscordClient_GuildDownloadCompleted;
            Client.GuildCreated += DiscordClient_GuildCreatedCompleted;
            Client.GuildDeleted += DiscordClient_GuildDeletedCompleted;
            Client.ChannelCreated += DiscordClient_ChannelCreated;
            Client.ChannelUpdated += DiscordClient_ChannelUpdated;
            Client.ChannelDeleted += DiscordClient_ChannelDeleted;
            var Commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { Config.Instance.CommandPrefix },
                EnableDms = true,
                EnableMentionPrefix = true,
            });
            Commands.RegisterCommands<CommandsHandler>();
            Commands.CommandErrored += CommandErrorHandler;
        }
        public static async Task<DiscordClientFactory> CreateAsync()
        {
            var fac = new DiscordClientFactory();
            await fac.Client.ConnectAsync();
            return fac;
        }
        private Task DiscordClient_ChannelDeleted(ChannelDeleteEventArgs e)
        {
            var channel = e.Channel;
            var existingChannel = channels.SingleOrDefault(x => x.Id == channel.Id);
            if (existingChannel != null)
            {
                lock (channels) channels.Remove(existingChannel);
            }
            return Task.CompletedTask;
        }

        private Task DiscordClient_ChannelUpdated(ChannelUpdateEventArgs e)
        {
            var channel = e.ChannelAfter;
            if (channel.Type == ChannelType.Text && channel.Name.Contains("broad") && !channels.Any(x => x.Id == channel.Id))
            {
                lock (channels)
                    channels.Add(channel);
            }
            return Task.CompletedTask;
        }

        private Task DiscordClient_ChannelCreated(ChannelCreateEventArgs e)
        {
            var channel = e.Channel;
            if (channel.Type == ChannelType.Text && channel.Name.Contains("broad"))
            {
                lock (channels)
                    channels.Add(channel);
            }
            return Task.CompletedTask;
        }

        private Task DiscordClient_GuildDeletedCompleted(GuildDeleteEventArgs e)
        {
            var guild = e.Guild;
            var channel = channels.SingleOrDefault(x => x.GuildId == guild.Id);
            lock (channels)
                channels.Remove(channel);
            return Task.CompletedTask;
        }

        private Task DiscordClient_GuildCreatedCompleted(GuildCreateEventArgs e)
        {
            var guild = e.Guild;
            var channel = guild.Channels.Where(x => x.Value.Name.Contains("broad")).Select(x => x.Value).FirstOrDefault();
            if (channel != null && !channels.Any(x => x.Id == channel.Id))
            {
                lock (channels)
                {
                    lock (channels) channels.Add(channel);
                }
            }

            return Task.CompletedTask;
        }

        private Task DiscordClient_GuildDownloadCompleted(DSharpPlus.EventArgs.GuildDownloadCompletedEventArgs e)
        {
            var guilds = e.Client.Guilds;
            foreach (var guild in guilds)
            {
                var channel = guild.Value.Channels.Where(x => x.Value.Name.Contains("broad")).Select(x => x.Value).FirstOrDefault();
                if (channel != null && !channels.Any(x => x.Id == channel.Id))
                {
                    lock (channels)
                        channels.Add(channel);
                }
            }
            return Task.CompletedTask;
        }

        private async Task CommandErrorHandler(CommandErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
            DatabaseContext.Instance.ErrorLog.Insert(new ErrorLog(e.Exception.ToString()));
        }
        public IEnumerable<DiscordChannel> GetDiscordChannels()
        {
            lock (channels)
            {
                foreach (var ch in channels)
                {
                    yield return ch;
                }
            }
        }
    }
}
