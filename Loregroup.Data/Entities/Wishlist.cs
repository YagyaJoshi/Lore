using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Wishlist : DBaseEntity
    {
        public Int64 Id { get; set; }
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }       
        public string StockStatus { get; set; }
        public decimal Price { get; set; }
        public Int64 UserId { get; set; }

    }
}
