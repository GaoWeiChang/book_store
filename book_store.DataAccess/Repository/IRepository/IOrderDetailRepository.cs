using book_store.DataAccess.Data;
using book_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book_store.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
