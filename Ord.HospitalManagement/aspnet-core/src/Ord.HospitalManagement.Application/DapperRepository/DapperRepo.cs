using Dapper;
using Ord.HospitalManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Ord.HospitalManagement.DapperRepo
{
    public class DapperRepo : DapperRepository<HospitalManagementDbContext>, ITransientDependency
    {
        public DapperRepo(IDbContextProvider<HospitalManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<IEnumerable<T>> QueryGetAsync<T>(string sql)
        {
            var connection = await GetDbConnectionAsync();
            var queryResult = await connection.QueryAsync<T>(
                sql,
                transaction: await GetDbTransactionAsync()
            );
            return queryResult;
        }
    }
}
