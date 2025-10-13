using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book_store.Utility
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ServiceResult Ok(string message)
        {
            return new ServiceResult { Success = true, Message = message };
        }

        public static ServiceResult Fail(string message) 
        { 
            return new ServiceResult { Success = false, Message = message };
        }
    }

    // For return status and data
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public static ServiceResult<T> Ok(T data, string message)
            => new() { Success = true, Message = message, Data = data };

        public new static ServiceResult<T> Fail(string message)
            => new() { Success = false, Message = message };
    }
}
