using BroadCapture.Infrastructures.Sqlite.Repositories;
using BroadCapture.Models;
using BroadCapture.Repositories;
using BroadCapture.Repositories.Based;
using BroadCapture.Repositories.Interfaces;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace BroadCapture.Infrastructures.Sqlite
{
    public partial class SqliteDbContext : IDbContext
    {
        private bool disposed;
        private readonly DbConnection _dbConnection;
        public DbConnection Connection => _dbConnection;

        public SqliteDbContext()
        {
            _dbConnection = new SQLiteConnection($@"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Local.db")};Version=3;");
            ServiceCheckUp();
        }

        private SqliteMessageRepository _Message { get; set; }
        public IRepository<Message> Messages
        {
            get
            {
                return _Message ??= new SqliteMessageRepository(this._dbConnection);
            }
        }
        private SqliteReservationRepository _Reservation { get; set; }
        public IRepository<Reservation> Reservations
        {
            get
            {
                return _Reservation ??= new SqliteReservationRepository(this._dbConnection);
            }
        }
        private SqliteErrorLogRepository _ErrorLog { get; set; }
        public IRepository<ErrorLog> ErrorLogs
        {
            get
            {
                return _ErrorLog ??= new SqliteErrorLogRepository(this._dbConnection);
            }
        }
        private SqlitePreferenceRepository _Preferences { get; set; }
        public IRepository<Preferences> Preferences
        {
            get
            {
                return _Preferences ??= new SqlitePreferenceRepository(this._dbConnection);
            }
        }
        private SqliteBotRequestLogRepository _BotLog { get; set; }
        public IRepository<BotRequestLog> BotRequestLogs
        {
            get
            {
                return _BotLog ??= new SqliteBotRequestLogRepository(this._dbConnection);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                this.Connection.Dispose();
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
