using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loregroup.Core.ViewModels
{
    public class WishlistViewModel : BaseViewModel
    {
        public WishlistViewModel()
        {
            WishlistList = new List<WishlistViewModel>();
        }

        public List<WishlistViewModel> WishlistList { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

         [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Price")]
        public Int64 Price { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "StockStatus")]
        public string StockStatus { get; set; }

        public Int64 ProductId { get; set; }
        public string ProductImage { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 UserId { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }
             

    }
}
