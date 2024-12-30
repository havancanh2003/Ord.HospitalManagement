using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.IServices
{
    public interface IDapperRepo
    {
        Task<IEnumerable<T>> QueryGetAsync<T>(string sql, object? parameters = null);
        Task<(int total, IEnumerable<T> lists)> QueryMultiGetAsync<T>(string sql, object? parameters = null);
        Task<T> QuerySingleAsync<T>(string sql, object? parameters = null);
    }
}
