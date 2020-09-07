using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Helpers
{
    public static class DiscordEmbedHelpers
    {
        public static DiscordEmbedBuilder GenerateEmbedMessage(string title, string description, string footer, string footerIconUrl = null, DiscordColor? color = null)
        {
            var embed = new DiscordEmbedBuilder();
            if (color != null)
            {
                embed.Color = new Optional<DiscordColor>(color.Value);
            }
            embed.Title = title;
            embed.Description = description;
            embed.Footer = new DiscordEmbedBuilder.EmbedFooter()
            {
                Text = footer,
                IconUrl = footerIconUrl
            };
            return embed;
        }
    }
}
