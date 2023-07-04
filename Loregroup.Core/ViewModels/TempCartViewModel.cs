using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{

    public class TempCartViewModel : BaseViewModel
    {
        public TempCartViewModel()
        {
            Productmodel = new ProductViewModel();
            CartList = new List<TempCartViewModel>();
        }

        public ProductViewModel Productmodel { get; set; }
        public List<TempCartViewModel> CartList { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }

        public Int64 Id { get; set; }
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceUSD { get; set; }
        public string Picture1 { get; set; }
        public decimal Total { get; set; }
        public Int64 UserId { get; set; }
        public int SizeUK { get; set; }
        public Int64 ColourId { get; set; }
        public string ColourName { get; set; }

    }
}
