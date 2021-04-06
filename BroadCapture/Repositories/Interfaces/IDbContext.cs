using BroadCapture.Models;
using BroadCapture.Repositories;
using BroadCapture.Repositories.Based;
using BroadCapture.Repositories.Interfaces;
using System;
using System.Data.Common;

namespace BroadCapture.Repositories.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbConnection Connection { get; }
        IRepository<Message> Messages { get; }
        IRepository<Reservation> Reservations { get; }
        IRepository<ErrorLog> ErrorLogs { get; }
        IRepository<Preferences> Preferences { get; }
        IRepository<BotRequestLog> BotRequestLogs { get; }
    }
}