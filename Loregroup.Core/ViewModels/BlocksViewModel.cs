using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class BlocksViewModel
    {
       public BlocksViewModel()
       {
           DistrictsViewModel = new DistrictsViewModel();
           BlockList = new List<BlocksViewModel>();
       }
       public Int64 Id { get; set; }

       [Display(Name = "District")]
       [Required(ErrorMessage = "Please Select District")]
       public Int64 DistricId { get; set; }

       [Display(Name = "Block Name")]
       [Required(ErrorMessage = "Please Enter Block Name")]
       public string BlockName { get; set; }

       public DistrictsViewModel DistrictsViewModel { get; set; }
       public List<BlocksViewModel> BlockList { get; set; }
       public int? StatusId { get; set; }
    }
}
