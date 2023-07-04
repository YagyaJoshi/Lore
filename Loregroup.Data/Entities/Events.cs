using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Events : DBaseEntity
    {
        public Int64 CustomerId { get; set; }
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string city { get; set; }
        public string mobileno { get; set; }
        public string image { get; set; }
        public int EventTypeId { get; set; }
        public string BoothNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public string Zipcode { get; set; }
        public string AddressOthr { get; set; }
    }
}
