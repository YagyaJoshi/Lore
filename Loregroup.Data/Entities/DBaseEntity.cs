using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class DBaseEntity
    {
       public DBaseEntity()
        {
            StatusId = 1;
            CreatedById = 1;
            CreationDate = DateTime.UtcNow;
            ModifiedById = 1;
            ModificationDate = DateTime.UtcNow;
        }
        public int? StatusId { get; set; }

        public Int64? CreatedById { get; set; }
        public Int64? ModifiedById { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
