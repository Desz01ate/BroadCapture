using BroadCapture.Repositories.Based;
using BroadCapture.Models;
using System;
using RDapter;
using BroadCaptureML.Model.Enum;
using System.Threading.Tasks;
using RDapter.Extends;

namespace BroadCapture.Infrastructures.Sqlite.Repositories
{
    ///<summary>
    /// Data contractor for Message
    ///</summary>
    public partial class SqliteMessageRepository : Repository<Message>
    {
        private Task<int> ManualInsertAsync(string currentMessage, int type, string CreateBy)
        {
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

