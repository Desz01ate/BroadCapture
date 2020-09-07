using BroadCaptureML.Model.Enum;
using BroadCapture;
using BroadCapture.Extensions;
using BroadCapture.Helpers;
using BroadCapture.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RDapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Utilities.Shared;

namespace AndroGETracker
{
    public partial class CommandsHandler : BaseCommandModule
    {
        public override Task BeforeExecutionAsync(CommandContext ctx)
        {
            var author = ctx.Message.Author;
            if (Config.Instance.IsInBlocklisting(author.Id))
            {
                ctx.RespondAsync("You're not allowed to use this bot.");
                throw new Exception($"{author.Username} is forbidden from using the bot.");
            }
            return base.BeforeExecutionAsync(ctx);
        }
        [Command("sell")]
        [Description("search for specific sell from all broads available.")]
        public async Task Sell(CommandContext ctx, [RemainingText] string keyword)
        {
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "SELL",
                FullCommand = $"!sell {keyword}"
            });
            await performOperationTask(ctx, keyword, MessageType.Sell);
        }
        [Command("buy")]
        [Description("search for specific buy from all broads available.")]
        public async Task Buy(CommandContext ctx, [RemainingText] string keyword)
        {
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "BUY",
                FullCommand = $"!buy {keyword}"
            });
            await performOperationTask(ctx, keyword, MessageType.Buy);
        }
        [Command("trade")]
        [Description("search for specific trade from all broads available.")]
        public async Task Trade(CommandContext ctx, [RemainingText] string keyword)
        {
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "TRADE",
                FullCommand = $"!trade {keyword}"
            });
            await performOperationTask(ctx, keyword, MessageType.Trade);
        }
        [Command("search")]
        [Description("search for specific broads within search keyword criteria")]
        public async Task Search(CommandContext ctx, [RemainingText] string keyword)
        {
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "SEARCH",
                FullCommand = $"!search {keyword}"
            });
            await performOperationTask(ctx, keyword, MessageType.Other);
        }
        [Command("raid")]
        [Description("search for raid party from broads.")]
        public async Task Raid(CommandContext ctx, [RemainingText] string raidName)
        {
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "RAID",
                FullCommand = $"!raid {raidName}"
            });
            await performOperationTask(ctx, raidName, MessageType.LookingForMember);
        }
        [Command("subscribe")]
        [Description("Subscribe to specific keyword and notify when related item is showing up in broad.")]
        public async Task Reserve(CommandContext ctx, [RemainingText] string keyword)
        {
            Action<string> replyAction;
            if (ctx.Member == null) //currently chatting in DM
            {
                replyAction = async (s) => await ctx.RespondAsync(s);
            }
            else //chatting via text channel
            {
                var dm = await ctx.Member.CreateDmChannelAsync();
                replyAction = async (s) => await dm.SendMessageAsync(s);
            }
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "SUBSCRIBE",
                FullCommand = $"!subscribe {keyword}"
            });
            var author = ctx.Message.Author;
            var id = author.Id;
            int expireIn = 180;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                replyAction("subscribe item keyword must not be empty.");
                return;
            }
            keyword = keyword.ToLower();
            if (keyword == "cancel")
            {
                DatabaseContext.Instance.Reservation.Delete(id);
                replyAction("Previous subscription has been cancelled.");
                return;
                //else
                //{
                //    replyAction($"There is already existing subscription in queue, please cancel it first and try again later.");
                //}
                //return;
            }
            DatabaseContext.Instance.Reservation.Insert(new BroadCapture.Models.Reservation()
            {
                OwnerId = (long)id,
                Keyword = keyword,
                CreateDate = DateTime.Now,
                ExpireInMinute = expireIn
            });
            replyAction($"{keyword} has been registered, subscription will be expires in {expireIn} minutes.");
        }
        [Command("preference")]
        [Description("Set preferences that is fit for your personal usage.")]
        public async Task Preferences(CommandContext ctx, string key = null, string value = null)
        {
            await DatabaseContext.Instance.BotRequestLogs.InsertAsync(new BotRequestLog()
            {
                Uid = (long)ctx.Message.Author.Id,
                Username = ctx.Message.Author.Username,
                CommandType = "PREFERENCES",
                FullCommand = $"!preference {key} {value}"
            });
            try
            {
                var lookupId = (long)ctx.Message.Author.Id;
                Preferences preferences = DatabaseContext.Instance.Preferences.Query(x => x.UserId == lookupId).FirstOrDefault();
                if (preferences == null)
                {
                    preferences = new Preferences();
                    preferences.UserId = (long)ctx.Message.Author.Id;
                    DatabaseContext.Instance.Preferences.Insert(preferences);
                }
                switch (key)
                {
                    case "range":
                        preferences.SearchRange = int.Parse(value);
                        break;
                    case "limit":
                        preferences.SearchLimit = int.Parse(value);
                        break;
                    default:
                        await ctx.RespondAsync($"Your current preference is {preferences.SearchLimit} messages within {preferences.SearchRange} days.");
                        return;
                }
                DatabaseContext.Instance.Preferences.Update(preferences);
                await ctx.RespondAsync("Preferences saved.");
            }
            catch (Exception ex)
            {
                await DatabaseContext.Instance.ErrorLog.InsertAsync(new ErrorLog(ex.ToString()));
                await ctx.RespondAsync($"There is an error occured when processing command.");
            }
        }
        private async Task performOperationTask(CommandContext ctx, string keyword, MessageType type)
        {
            var author = ctx.Message.Author;
            var id = author.Id;
            string appendMessage = string.Empty;
            try
            {
                var stw = new System.Diagnostics.Stopwatch();
                stw.Start();
                string filter, broadType;
                System.Text.StringBuilder keywordsFilter = new System.Text.StringBuilder();
                switch (type)
                {
                    case MessageType.Sell:
                        filter = $"Type IN ({(long)MessageType.Sell},{(long)MessageType.SellOrTrade},{(long)MessageType.BuyAndSell})";
                        broadType = "sell messages";
                        break;
                    case MessageType.Buy:
                        filter = $"Type IN ({(long)MessageType.Buy},{(long)MessageType.BuyAndSell})";
                        broadType = "buy messages";
                        break;
                    case MessageType.Trade:
                        filter = $"Type IN ({(long)MessageType.Trade},{(long)MessageType.SellOrTrade})";
                        broadType = "trade messages";
                        break;
                    default:
                        filter = "1 = 1";
                        broadType = "everything";
                        break;
                }
                var lookupId = (long)author.Id;
                var preference = DatabaseContext.Instance.Preferences.Query(x => x.UserId == lookupId).FirstOrDefault();
                var keywords = keyword.Split(',').Select(x => x.Trim()).ToArray();
                var range = preference?.SearchRange ?? 1;
                var limit = preference?.SearchLimit ?? 10;
                var param = new List<RDapter.Entities.DatabaseParameter>();
                var firstAppend = true;
                for (var i = 0; i < keywords.Length; i++)
                {
                    var bindingKey = string.Empty;
                    if (firstAppend)
                    {
                        firstAppend = false;
                        bindingKey = "AND";
                    }
                    else
                    {
                        bindingKey = "OR";
                    }
                    var kw = keywords[i].Trim();
                    keywordsFilter.AppendLine($" {bindingKey} LOWER(content) LIKE @keyword{i}");
                    param.Add(new RDapter.Entities.DatabaseParameter($"keyword{i}", $"%{kw.ToLower()}%"));
                }
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    await ctx.RespondAsync("search keyword must not be empty.");
                    return;
                }
                var retry = 0;
                List<Message> result;
                var limitDate = DateTime.Today.AddDays(-range);
                var query = $@"SELECT L.*
                                   FROM Message L
                                   INNER JOIN (
                                   	SELECT id,max(createdate) AS Latest
                                   	FROM Message
                                   	WHERE {filter}
                                       {keywordsFilter}
                                       AND createdate >= '{limitDate.Year}-{limitDate.Month}-{limitDate.Day}'
                                   	GROUP BY id,createby
                                   ) R
                                   ON L.CreateDate = R.Latest AND L.Id = R.Id
                                   ORDER BY CreateDate DESC 
                                   LIMIT {limit}";
                do
                {
                    var copyParam = param.ConvertAll(x => x);
                    range += (retry * 30);
                    copyParam.Add(new RDapter.Entities.DatabaseParameter("date", DateTime.Today.AddDays(-range)));
                    result = DatabaseContext.Instance.OnlineConnection.ExecuteReader<Message>(new RDapter.Entities.ExecutionCommand(query, copyParam)).GroupBy(x => x.content).Select(x => x.First()).AsList();
                } while (result.Count == 0 && retry++ < 2);
                if (retry == 1)
                {
                    appendMessage = " (Automatic Extended Range)";
                }
                if (result.Count() == 0)
                {
                    await ctx.Channel.SendDisposableMessageAsync($"No data related to '{keyword}' found.");
                    return;
                }
                stw.Stop();
                Action<DiscordEmbedBuilder> replyAction;
                if (ctx.Member != null)
                {
                    var dmChannel = await ctx.Member.CreateDmChannelAsync();
                    replyAction = async (s) =>
                    {
                        try
                        {
                            await dmChannel.SendMessageAsync("", embed: s);
                        }
                        catch
                        {
                            await ctx.RespondAsync("", embed: s);
                        }
                    };
                }
                else
                {
                    replyAction = async (s) =>
                    {
                        await ctx.RespondAsync("", embed: s);
                    };
                }
                var now = DateTime.Now;
                var embed = DiscordEmbedHelpers.GenerateEmbedMessage($"Result for {broadType} related to {keyword} :",
                    null,
                    $"Brought to you by Coalescense with love <3 ({(float)stw.ElapsedMilliseconds / 1000} seconds processed)\nwith {range} days{appendMessage} {limit} messages preset.",
                    (await ctx.Client.GetOwnerAsync()).AvatarUrl,
                    DiscordColorHelpers.GetRandomColor());
                foreach (var broad in result)
                {
                    var prefixTime = string.Empty;
                    var diff = now.Subtract(broad.createdate.Value);
                    if (diff.Days > 0)
                    {
                        prefixTime = $"{diff.Days} days ago";
                    }
                    else if (diff.Hours > 0)
                    {
                        prefixTime = $"{diff.Hours} hours ago";
                    }
                    else
                    {
                        prefixTime = $"{diff.Minutes} minutes ago";
                    }
                    if (embed.Fields.Count == 25)
                    {
                        replyAction(embed);
                        embed = DiscordEmbedHelpers.GenerateEmbedMessage($"Result for {broadType} related to {keyword} :",
                                                                         null,
                                                                         $"Brought to you by Coalescense with love <3 ({(float)stw.ElapsedMilliseconds / 1000} seconds processed)\nwith {range} days{appendMessage} {limit} messages preset.",
                                                                         (await ctx.Client.GetOwnerAsync()).AvatarUrl,
                                                                         DiscordColorHelpers.GetRandomColor());
                    }
                    embed.AddField($"{prefixTime} by {broad.createby}", broad.content);
                }
                replyAction(embed);
            }
            finally
            {
                //await ctx.Message.DeleteAsync();
            }
        }
    }
}