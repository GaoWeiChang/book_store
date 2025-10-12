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
}
