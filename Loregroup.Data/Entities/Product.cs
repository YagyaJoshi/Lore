using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Product : DBaseEntity
    {
        [Key]      
        public Int64 Id { get; set; }
        public string ProductName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Int64? BrandId { get; set; }
        public Int64? ColourId { get; set; }
        public Int64? CutOfDressId { get; set; }
        public Int64? CollectionYearId { get; set; }
        public Int64? CategoryId { get; set; }
        public Int64? FabricId { get; set; }
        public string Style { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal PriceEURO { get; set; }
        public decimal PriceGBP { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }

        public string SeletedBrandIds { get; set; }
        public string SeletedBrandNames { get; set; }

        public string SeletedColorIds { get; set; }
        public string SeletedColorNames { get; set; }

        public string SeletedCategoryIds { get; set; }
        public string SeletedCategoryNames { get; set; }

        public string SelectedFabricIds { get; set; }
        public string SelectedFabricNames { get; set; }

        public bool Publish { get; set; }
        public int? PublishId { get; set; }
        public string VideoImage { get; set; }
        public string VideoLink { get; set; }

        public string PurchasePrice { get; set; }
    }
}
