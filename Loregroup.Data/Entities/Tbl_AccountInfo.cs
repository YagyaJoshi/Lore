using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class Tbl_AccountInfo : BaseEntity
    {
        public string PaidApplicationNumber { get; set; }
        public string UnPaidApplicationNo { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? PayDate { get; set; }

    }
}
