using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Tax: DBaseEntity
    {
        [Key]
        public Int64 Id { get; set; }
        public decimal TaxPercentage { get; set; }
        public string Description { get; set; }
    }
}
