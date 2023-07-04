using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class MasterUser : BaseEntity
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        [NotMapped]
        public String FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public int Gender { get; set; }

        public String Email { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }

        public string Keyword { get; set; }

        public String Fax { get; set; }

        public String Mobile { get; set; }

        public Int64? StateId { get; set; }

        public Int64? DistrictId { get; set; }

        public Int64? CityId { get; set; }

        public Int64 RoleId { get; set; }

        //public string RoleName { get; set; }

        public String ShopName { get; set; }

        public String Salt { get; set; }

        public Int64 PackageId { get; set; }

        public string District { get; set; }

        public string CompanyName { get; set; }

        public string CompanyTaxId { get; set; }

        public Int64? CountryId { get; set; }

        public string ZipCode { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string StateName { get; set; }

        public string City { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string TelephoneNo { get; set; }

        public Int64? CurrencyId { get; set; }

        public Int64? DistributionPointId { get; set; }

        public bool TaxEnabled { get; set; }

        public Int64? TaxId { get; set; }
        public bool ShowOnMap { get; set; }

        public string ShopWebsite { get; set; }
        public Int64? territoryId { get; set; }

    }
}
