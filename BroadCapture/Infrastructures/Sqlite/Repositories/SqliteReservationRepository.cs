using BroadCapture.Models;
using BroadCapture.Repositories.Based;
using RDapter;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Infrastructures.Sqlite.Repositories
{
    public class SqliteReservationRepository : Repository<Reservation>
    {
        public SqliteReservationRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }
        public IEnumerable<Reservation> GetReservationByKeyword(string keyword)
        {
            var param = keyword.ToLower();
            var query = $@"SELECT * FROM Reservation WHERE lower(Keyword) LIKE @keyword";
            return this.Connector.ExecuteReader<Reservation>(query, new
            {
                keyword = $"%{param}%"
            });
        }
    }
}
