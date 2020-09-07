using BroadCapture.Models;
using RDapter;
using RDapter.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture
{
    public partial class DatabaseContext
    {
        private void ServiceCheckUp()
        {
            //RDapter.Extends.Global.SetDefaultSqlTypeMap(type =>
            //{
            //    if (type == typeof(string))
            //    {
            //        return "TEXT";
            //    }
            //    else if (type == typeof(char) || type == typeof(char?))
            //    {
            //        return "CHARACTER(1)";
            //    }
            //    else if (type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?))
            //    {
            //        return "SMALLINT";
            //    }
            //    else if (type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?))
            //    {
            //        return "INTEGER";
            //    }
            //    else if (type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?))
            //    {
            //        return "BIGINT";
            //    }
            //    else if (type == typeof(float) || type == typeof(float?))
            //    {
            //        return "REAL";
            //    }
            //    else if (type == typeof(double) || type == typeof(double?))
            //    {
            //        return "DOUBLE PRECISION";
            //    }
            //    else if (type == typeof(bool) || type == typeof(bool?))
            //    {
            //        return "BOOLEAN";
            //    }
            //    else if (type == typeof(decimal) || type == typeof(decimal?))
            //    {
            //        return "MONEY";
            //    }
            //    else if (type == typeof(DateTime) || type == typeof(DateTime?))
            //    {
            //        return "timestamp";
            //    }
            //    else if (type == typeof(Guid) || type == typeof(Guid?))
            //    {
            //        return "UUID";
            //    }
            //    else if (type == typeof(byte) || type == typeof(byte?) || type == typeof(sbyte) || type == typeof(sbyte?))
            //    {
            //        return "SMALLINT";
            //    }
            //    else if (type == typeof(byte[]))
            //    {
            //        return "BYTEA";
            //    }
            //    throw new NotSupportedException();
            //});
            RDapter.Global.SetSchemaConstraint<Message>(x =>
            {
                x.SetPrimaryKey<Message>(y => y.id);
            });
            RDapter.Global.SetSchemaConstraint<Reservation>(x =>
            {
                x.SetPrimaryKey<Reservation>(y => y.OwnerId);
            });
            RDapter.Global.SetSchemaConstraint<Preferences>(x =>
            {
                x.SetPrimaryKey<Preferences>(y => y.UserId);
            });
            var messageTableCheck = CheckTableExists("Message");
            if (!messageTableCheck)
            {
                this.OfflineConnection.CreateTable<Message>();
            }
            var reservationTableCheck = CheckTableExists("Reservation");
            if (!reservationTableCheck)
            {
                this.OfflineConnection.CreateTable<Reservation>();
            }
            var errorLogTableCheck = CheckTableExists("ErrorLog");
            if (!errorLogTableCheck)
            {
                this.OfflineConnection.CreateTable<ErrorLog>();
            }
            var preferencesTableCheck = CheckTableExists("Preferences");
            if (!preferencesTableCheck)
            {
                this.OfflineConnection.CreateTable<Preferences>();
            }
            var botRequestLogCheck = CheckTableExists(nameof(BotRequestLog));
            if (!botRequestLogCheck)
            {
                this.OfflineConnection.CreateTable<BotRequestLog>();
            }
        }

        private bool CheckTableExists(string tableName)
        {
            var checker = this.OfflineConnection.ExecuteScalar($"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';");
            return checker != null;
        }
    }
}
