using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Helpers
{
    public static class DiscordColorHelpers
    {
        public static DiscordColor GetColorForMessage(BroadCaptureML.Model.Enum.MessageType type)
        {
            DiscordColor color;
            switch (type)
            {
                case BroadCaptureML.Model.Enum.MessageType.Buy:
                    color = new DiscordColor(255, 196, 93);
                    break;
                case BroadCaptureML.Model.Enum.MessageType.Sell:
                    color = new DiscordColor(6, 194, 88);
                    break;
                case BroadCaptureML.Model.Enum.MessageType.Trade:
                    color = new DiscordColor(55, 146, 203);
                    break;

                case BroadCaptureML.Model.Enum.MessageType.BuyAndSell:
                    color = new DiscordColor(255, 255, 255);
                    break;
                case BroadCaptureML.Model.Enum.MessageType.SellOrTrade:
                    color = new DiscordColor(0, 0, 0);
                    break;
                case BroadCaptureML.Model.Enum.MessageType.Other:
                default:
                    color = new DiscordColor(255, 0, 0);
                    break;
            }
            return color;
        }
        readonly static Random rand = new Random();
        static byte Random(byte min, byte max)
        {
            return (byte)((rand.NextDouble() * (max - min)) + min);
        }
        public static DiscordColor GetRandomColor()
        {
            var r = Random(0, 255);
            var g = Random(0, 255);
            var b = Random(0, 255);
            return new DiscordColor(r, g, b);
        }
    }
}
