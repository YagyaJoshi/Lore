using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Size: DBaseEntity
    {
        [Key]
        public Int64 Id { get; set; }
        public string SizeNameUS { get; set; }
        public string SizeNameUK { get; set; }
        public string SizeNameEU { get; set; }
        public string Description { get; set; }
    }
}
