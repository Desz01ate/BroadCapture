using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Extensions
{
    public static class DiscordClientExtensions
    {
        public static Task<DiscordUser> GetOwnerAsync(this DiscordClient client)
        {
            return client.GetUserAsync(322051347505479681);
        }
    }
}
