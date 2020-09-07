using BroadCapture.Repositories.Based;
using BroadCapture.Models;
using System;
using RDapter;
using BroadCaptureML.Model.Enum;
using System.Threading.Tasks;
using RDapter.Extends;

namespace BroadCapture.Repositories
{
    ///<summary>
    /// Data contractor for Message
    ///</summary>
    public partial class MessageRepository : Repository<Message>
    {
        public Task<int> ManualInsertAsync(string currentMessage, int type, string CreateBy)
        {
            TotalMessages += 1;
            var (isBuy, isSell, isTrade) = ValidateFlagType(type);
            var parameter = new
            {
                content = currentMessage,
                type,
                createDate = DateTime.Now,
                createBy = CreateBy,
                isBuy,
                isSell,
                isTrade
            };
            var sql = "INSERT INTO Message(Content,Type,CreateDate,CreateBy,IsBuy,IsSell,IsTrade) VALUES(@content,@type,@createDate,@createBy,@isBuy,@isSell,@isTrade)";
            Service.OfflineConnection.ExecuteNonQuery(sql, parameter);
            return this.Connector.ExecuteNonQueryAsync(sql, parameter);

        }

        private (bool isBuy, bool isSell, bool isTrade) ValidateFlagType(int type)
        {
            bool isBuy = false, isSell = false, isTrade = false;
            switch ((MessageType)type)
            {
                case MessageType.Buy:
                    isBuy = true;
                    break;
                case MessageType.Sell:
                    isSell = true;
                    break;
                case MessageType.Trade:
                    isTrade = true;
                    break;
                case MessageType.BuyAndSell:
                    isBuy = true;
                    isSell = true;
                    break;
                case MessageType.SellOrTrade:
                    isSell = true;
                    isTrade = true;
                    break;
            }
            return (isBuy, isSell, isTrade);
        }
    }
}

