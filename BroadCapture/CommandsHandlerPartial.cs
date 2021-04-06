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
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Utilities.Enum;
using Utilities.Shared;

namespace AndroGETracker
{
    public partial class CommandsHandler : BaseCommandModule
    {
        //[Command("stat")]
        public async Task GetStatAsync(CommandContext ctx)
        {
            var generalQuery = @"SELECT
                                    COUNT(*) AS TOTAL_MESSAGES,
                                    CAST(julianday(MAX(createdate)) - julianday(MIN(createdate)) AS INTEGER) AS TOTAL_ACTIVE_DAY,
                                	COUNT(*) / CAST(julianday(MAX(createdate)) - julianday(MIN(createdate)) AS INTEGER) AS AVERAGE_MESSAGE_PER_DAY,
                                	(SELECT COUNT(*) FROM MESSAGE WHERE Type = 1) AS SELL,
                                	(SELECT COUNT(*) FROM MESSAGE WHERE Type = 2) AS BUY,
                                	(SELECT COUNT(*) FROM MESSAGE WHERE Type = 3) AS TRADE
                                FROM 
                                	Message;";
            var topBroaderQuery = @"SELECT CreateBy,COUNT(*) AS TOTAL_MESSAGES  
                                    FROM Message
                                    GROUP BY CreateBy
                                    ORDER BY COUNT(*) DESC
                                    LIMIT 1";
            var generalData = (await this._dbContext.Connection.ExecuteReaderAsync(generalQuery)).First();
            var topBroader = (await this._dbContext.Connection.ExecuteReaderAsync(topBroaderQuery)).First();
            var responseEmbed = new DiscordEmbedBuilder()
            {
                Title = $"Statistics for broad bot since {this._dbContext.Messages.Min(x => x.createdate).Value.ToString("MMMM, dd yyyy")}",
            };
            responseEmbed.AddField("Total Messages : ", $"{Utilities.String.NumberFormat((int)generalData.TOTAL_MESSAGES, FormatSpecifier.General)} messages.");
            responseEmbed.AddField("Days since first introduce : ", $"{generalData.TOTAL_ACTIVE_DAY} days.");
            responseEmbed.AddField("Average message per day : ", $"{generalData.AVERAGE_MESSAGE_PER_DAY} messages.");
            responseEmbed.AddField("Player with most broad of all time : ", $"{topBroader.CreateBy} with {topBroader.TOTAL_MESSAGES} messages.");
            responseEmbed.Color = DiscordColorHelpers.GetRandomColor();
            await ctx.RespondAsync(embed: responseEmbed);
        }
        [Command("dump")]
        [RequireOwner]
        public async Task DumpDatabaseFileAsync(CommandContext ctx)
        {
            File.Copy("Local.db", "Copy_of_Local.db", true);

            if (ctx.Member == null)
            {
                await ctx.RespondWithFileAsync("Copy_of_Local.db");
            }
            else
            {
                var dm = await ctx.Member.CreateDmChannelAsync();
                await dm.SendFileAsync("Copy_of_Local.db");
            }
            File.Delete("Copy_of_Local.db");
        }
    }
}