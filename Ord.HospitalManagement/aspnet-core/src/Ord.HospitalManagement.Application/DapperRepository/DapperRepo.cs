using Dapper;
using Ord.HospitalManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ord.HospitalManagement.DapperRepo
{
    public class DapperRepo : DapperRepository<HospitalManagementDbContext>, ITransientDependency
    {
        public DapperRepo(IDbContextProvider<HospitalManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<IEnumerable<T>> QueryGetAsync<T>(string sql, object? parameters = null)
        {
            var connection = await GetDbConnectionAsync();
            var transaction = await GetDbTransactionAsync();
            var queryResult = await connection.QueryAsync<T>(
                    sql,
                    param: parameters,
                    transaction: transaction
                );
            return queryResult;
        }
        public async Task<(int total, IEnumerable<T> lists)> QueryMultiGetAsync<T>(string sql, object? parameters = null)
        {
            var connection = await GetDbConnectionAsync();
            var transaction = await GetDbTransactionAsync(); 
            try
            {
                var multi = await connection.QueryMultipleAsync(sql, parameters, transaction);
                var total = await multi.ReadSingleAsync<int>();
                var lists = await multi.ReadAsync<T>();
                return (total, lists);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null)
        {
            var connection = await GetDbConnectionAsync();
            var transaction = await GetDbTransactionAsync();
            var queryResult = await connection.QuerySingleOrDefaultAsync<T>(
               sql,
               param: parameters,
               transaction: transaction
               );
            return queryResult;
        }
    }
}
