using BroadCaptureML.Model.Enum;
using System;
namespace BroadCapture.Models
{
    public partial class Message
    {
        internal void UpdateFlag()
        {
            if (!this.type.HasValue) return;
            switch ((MessageType)this.type)
            {
                case MessageType.Buy:
                    this.isbuy = true;
                    break;
                case MessageType.Sell:
                    this.issell = true;
                    break;
                case MessageType.Trade:
                    this.istrade = true;
                    break;
                case MessageType.BuyAndSell:
                    this.isbuy = true;
                    this.issell = true;
                    break;
                case MessageType.SellOrTrade:
                    this.issell = true;
                    this.istrade = true;
                    break;
                default:
                    return;
            }
        }
    }
}

