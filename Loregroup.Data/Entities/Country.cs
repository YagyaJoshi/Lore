using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Country : BaseEntity
    {
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
