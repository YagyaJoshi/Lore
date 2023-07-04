using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
  public  class Block :DBaseEntity
    {    
        [Key]
        public Int64 Id { get; set; }
        public Int64 DistricId {get; set; }
        public string BlockName { get; set; }

       
    }
}
