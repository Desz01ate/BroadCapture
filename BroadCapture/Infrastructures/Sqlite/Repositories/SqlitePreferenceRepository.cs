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
    public class SqlitePreferenceRepository : Repository<Preferences>
    {
        public SqlitePreferenceRepository(DbConnection databaseConnector) : base(databaseConnector)
        {
        }
    }
}
