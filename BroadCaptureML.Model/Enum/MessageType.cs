using System;
using System.Collections.Generic;
using System.Text;

namespace BroadCaptureML.Model.Enum
{
    public enum MessageType
    {
        Sell = 1,
        Buy = 2,
        Trade = 3,
        BuyAndSell = 4,
        SellOrTrade = 5,
        LookingForMember = 6,
        Other = -1
    }
}
