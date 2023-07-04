using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class AccountInfoViewModel :BaseViewModel
    {
       [Display(Name="Paid Applications")]
       public string PaidApplicationNumber { get; set; }

       [Display(Name = "UnPaid Applications")]
       public string UnPaidApplicationNo { get; set; }

       [Display(Name = "Paid Amount")]
       public decimal? PaidAmount { get; set; }

       [Display(Name = "Due Amount")]
       public decimal? DueAmount { get; set; }

       [Display(Name = "Total Amount")]
       public decimal? TotalAmount { get; set; }


       public DateTime? PayDate { get; set; }


    }
}
