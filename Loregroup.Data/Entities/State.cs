using Loregroup.Data.Entities;
//using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Loregroup.Data.Entities
{
    public partial class State : DBaseEntity
    {
        //Add by 
        [Key]
        public Int64 Id { get; set; }
        public Int64 CountryId { get; set; }
        public string CountryName { get; set; }
        public string Statename { get; set; }

    }
}
