using BroadCapture.Models;
using BroadCapture.Repositories.Based;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Infrastructures.Sqlite.Repositories
{
    public class SqliteErrorLogRepository : Repository<ErrorLog>
    {
        public SqliteErrorLogRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
