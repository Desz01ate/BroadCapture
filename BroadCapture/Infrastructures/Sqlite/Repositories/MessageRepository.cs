// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a ModelGenerator.
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using BroadCapture.Repositories.Based;
using BroadCapture.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RDapter.Extends;
using System.Data.Common;
using System;
using RDapter.Entities;
using RDapter;
using Utilities.Shared;

namespace BroadCapture.Infrastructures.Sqlite.Repositories
{
    ///<summary>
    /// Data contractor for Message
    ///</summary>
    public partial class SqliteMessageRepository : Repository<Message>
    {
        private int TotalMessages = -1;
        public SqliteMessageRepository(DbConnection service) : base(service)
        {
            this.TotalMessages = this.Count();
        }
        public override void Insert(Message data)
        {
            throw new NotImplementedException();
        }
        public override Task InsertAsync(Message data)
        {
            TotalMessages += 1;
            return ManualInsertAsync(data.content, data.type.Value, data.createby);
            //return base.InsertAsync(data);
        }
        public override void InsertMany(IEnumerable<Message> data)
        {
            TotalMessages += data.Count();
            base.InsertMany(data);
        }
        public override Task InsertManyAsync(IEnumerable<Message> data)
        {
            TotalMessages += data.Count();
            return base.InsertManyAsync(data);
        }
        public override int Count()
        {
            if (TotalMessages == -1)
            {
                TotalMessages = base.Count();
            }
            return TotalMessages;
        }

        public override async Task<object> ExecuteDirectFunctionAsync(string function, params object[] args)
        {
            switch (function)
            {
                case "search":
                    var searchResult = await ExecuteMessagesSearchingAsync((string)args[0], (string)args[1], (DateTime)args[2], (int)args[3], (List<DatabaseParameter>)args[4]);
                    return searchResult;
            }
            return Task.FromResult((object)-1);
        }

        private async Task<List<Message>> ExecuteMessagesSearchingAsync(string filter, string keywordsFilter, DateTime limitDate, int limit, IEnumerable<DatabaseParameter> databaseParameters)
        {
            var query = $@"SELECT L.*
                                   FROM Message L
                                   INNER JOIN (
                                   	SELECT id,max(createdate) AS Latest
                                   	FROM Message
                                   	WHERE {filter}
                                       {keywordsFilter}
                                       AND createdate >= '{limitDate.Year}-{limitDate.Month.ToString().PadLeft(2, '0')}-{limitDate.Day.ToString().PadLeft(2, '0')}'
                                   	GROUP BY id,createby
                                   ) R
                                   ON L.CreateDate = R.Latest AND L.Id = R.Id
                                   ORDER BY CreateDate DESC 
                                   LIMIT {limit}";
            var result = await this.Connector.ExecuteReaderAsync<Message>(new ExecutionCommand(query, databaseParameters));
            return result.GroupBy(row => row.content).Select(x => x.First()).AsList();
        }
    }
}

