using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Loregroup.Core.ViewModels
{
    public class RefillRateViewModel : BaseViewModel
    {

        
        [Display (Name = "Enter New Refill Rate here")]
        public decimal NewRefillRate { get; set; }

        [Display(Name = "Old Refill Rate")]
        public decimal OldRefillRate { get; set; }

        [Display(Name = "Date From")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateFrom { get; set; }
        public string datefrm { get; set; }

        public string Msg { get; set; }

        public Int64 PackageId { get; set; }


      

    }
}
