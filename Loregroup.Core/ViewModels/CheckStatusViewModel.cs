using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Loregroup.Core.ViewModels
{
   public class CheckStatusViewModel : BaseViewModel
    {

       public CheckStatusViewModel() {

           dslog = new DSLogViewModel();

       }

      [Required]
       [Display(Name = "CONSUMER NO: ")]
       public string ConsumerNo { get; set; }
       public string bstatus { get; set; }      
       public string MsgAccToStatus { get; set; }

       //to show reciept

       [Display(Name = "NAME: ")]
       public string Name { get; set; }

       [Display(Name = "BOOKING DATE: ")]
       public DateTime? BookingDate { get; set; }

       [Display(Name = "BOOKING NO: ")]
       public string BookingNo { get; set; }

       [Display(Name = "AMOUNT: ")]
       public decimal Amount { get; set; }

       public string UserFullName { get; set; }

       public string UserCity { get; set; }

       public string Username { get; set; }

       public string declaration { get; set; }

       public string forgap { get; set; }

       [Display(Name = "SIGN ")]
       public string signaure { get; set; }

       public string onlydate { get; set; }

       public string message { get; set; }

       public string number { get; set; }

       public string TimeStamp { get; set; }

       public DSLogViewModel dslog { get; set; } 
    }
}
