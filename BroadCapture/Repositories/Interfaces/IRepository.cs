using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Repositories.Interfaces
{
    public interface IRepository<T> : IEnumerable<T>
    {
        /// <summary>
        /// Delete data from repository.
        /// </summary>
        /// <param name="data">Generic object.</param>
        void Delete(T data);

        /// <summary>
        /// Delete data from repository.
        /// </summary>
        /// <param name="key">Primary key of target object.</param>
        void Delete(object key);

        /// <summary>
        /// Delete data from repository in an hronous manner.
        /// </summary>
        /// <param name="data">Generic object.</param>
        Task DeleteAsync(T data);

        /// <summary>
        /// Delete data from repository in an hronous manner.
        /// </summary>
        /// <param name="key">Primary key of target object.</param>
        Task DeleteAsync(object key);

        /// <summary>
        /// Insert data into repository.
        /// </summary>
        /// <param name="data">Generic object.</param>
        void Insert(T data);

        /// <summary>
        /// Insert data into repository in an hronous manner.
        /// </summary>
        /// <param name="data">Generic object.</param>
        Task InsertManyAsync(IEnumerable<T> data);

        /// <summary>
        /// Insert data into repository.
        /// </summary>
        /// <param name="data">Generic object.</param>
        void InsertMany(IEnumerable<T> data);

        /// <summary>
        /// Insert data into repository in an hronous manner.
        /// </summary>
        /// <param name="data">Generic object.</param>
        Task InsertAsync(T data);

        /// <summary>
        /// Get all data from repository.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Query(bool buffered = false);

        /// <summary>
        /// Get data by specific condition from repository.
        /// </summary>
        /// <param name="predicate">Predicate condition.</param>
        /// <returns></returns>
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate, int? top = null);

        /// <summary>
        /// Get data from repository.
        /// </summary>
        /// <param name="key">Primary key of target object.</param>
        /// <returns></returns>
        T Query(object key);

        /// <summary>
        /// Get all data from repository in an hronous manner.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync();

        /// <summary>
        /// Get data by specific condition from repository in an hronous manner.
        /// </summary>
        /// <param name="predicate">Predicate condition.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get data from repository.
        /// </summary>
        /// <param name="key">Primary key of target object.</param>
        /// <returns></returns>
        Task<T> QueryAsync(object key);

        /// <summary>
        /// Update data in repository.
        /// </summary>
        /// <param name="data">Generic object.</param>
        void Update(T data);

        /// <summary>
        /// Update data in repository in an hronous manner.
        /// </summary>
        /// <param name="data">Generic object.</param>
        Task UpdateAsync(T data);

        /// <summary>
        /// Returns rows count from repository.
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Returns rows count that is satisfied specific condition from repository.
        /// </summary>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<T> Take(int count);

        /// <summary>
        /// Execute custom provider-dependent function.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object ExecuteDirectFunction(string function, params object[] args);

        /// <summary>
        /// Execute custom provider-dependent function in an asynchronous manner.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> ExecuteDirectFunctionAsync(string function, params object[] args);
    }
}
