using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public partial class City : DBaseEntity
    {
        [Key]
        //Add By 
        public Int64 Id { get; set; }
        public string CityName { get; set; }
        public Int64 CountryId { get; set; }
        public Int64 DistricId { get; set; }
        public Int64 StateId { get; set; }
        

     
    }
}
