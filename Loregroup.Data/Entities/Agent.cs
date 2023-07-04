using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Agent : DBaseEntity
    {
        [Key]
        public Int64 Id { get; set; }
        public string territory { get; set; }
        public string Description { get; set; }
        public Int64 AgentsId { get; set; }
    }
}
