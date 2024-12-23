using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DataResult
{
    public class DataResult<T>
    {
        public bool IsOk { get; set; }

        public string Msg { get; set; } = string.Empty;

        public T? Data { get; set; }
        public List<T>? ListData { get; set; }
        public List<T>? ErrorData { get; set; } // Dữ liệu bị lỗi

        public static DataResult<T> GetResult(bool isOk, string msg, T data = default(T))
        {
            return new DataResult<T>
            {
                IsOk = isOk,
                Msg = msg,
                Data = data
            };
        }
        public static DataResult<T> GetResult(bool isOk, string msg, List<T>? listData = null, List<T>? errorData = null)
        {
            return new DataResult<T>
            {
                IsOk = isOk,
                Msg = msg,
                ListData = listData,
                ErrorData = errorData
            };
        }

    }
}
