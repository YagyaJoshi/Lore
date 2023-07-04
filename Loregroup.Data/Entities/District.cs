using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class District : DBaseEntity
    {
        [Key]
        //Add By 
        public Int64 Id { get; set; }
        public Int64 StateId { get; set; }
        public string DistricName { get; set; }

    }
}
