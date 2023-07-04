using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class UserLocation
    {
        public string ShopName { get; set; }
        public string UserName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Statename { get; set; }
        public Int64 CountryId { get; set; }

        public int isonmap { get; set; }
    }

}
