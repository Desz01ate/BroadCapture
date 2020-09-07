using BroadCapture.Models;
using BroadCapture.Repositories;
using BroadCapture.Repositories.Based;
using Npgsql;
using System;
using System.Data.SQLite;
using System.IO;

namespace BroadCapture
{
    public partial class DatabaseContext : IDisposable
    {
        internal protected readonly NpgsqlConnection OnlineConnection;
        internal protected readonly SQLiteConnection OfflineConnection;
        public static DatabaseContext Instance { get; } = new DatabaseContext();
        public DatabaseContext(string offlineConnectionString, string onlineConnectionString)
        {
            OnlineConnection = new NpgsqlConnection(offlineConnectionString);
            OfflineConnection = new SQLiteConnection(onlineConnectionString);
            ServiceCheckUp();
        }
        public DatabaseContext()
        {
            OnlineConnection = new NpgsqlConnection(Config.Instance.NpgsqlConnectionString);
            OfflineConnection = new SQLiteConnection($@"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Local.db")};Version=3;");
            ServiceCheckUp();
        }
        private MessageRepository _Message { get; set; }
        public MessageRepository Message
        {
            get
            {
                if (_Message == null)
                {
                    _Message = new MessageRepository(this);
                }
                return _Message;
            }
        }
        private ReservationRepository _Reservation { get; set; }
        public ReservationRepository Reservation
        {
            get
            {
                if (_Reservation == null)
                {
                    _Reservation = new ReservationRepository(this);
                }
                return _Reservation;
            }
        }
        private ErrorLogRepository _ErrorLog { get; set; }
        public ErrorLogRepository ErrorLog
        {
            get
            {
                if (_ErrorLog == null)
                {
                    _ErrorLog = new ErrorLogRepository(this);
                }
                return _ErrorLog;
            }
        }
        private Repository<Preferences> _Preferences { get; set; }
        public Repository<Preferences> Preferences
        {
            get
            {
                return (_Preferences ?? new Repository<Preferences>(this.OfflineConnection));
            }
        }
        private Repository<BotRequestLog> _BotLog { get; set; }
        public Repository<BotRequestLog> BotRequestLogs
        {
            get
            {
                return (_BotLog ?? new Repository<BotRequestLog>(this.OfflineConnection));
            }
        }
        public void Dispose()
        {
            OnlineConnection?.Dispose();
        }
    }
}
