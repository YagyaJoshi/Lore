using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Loregroup.Core.ViewModels
{
   public class FileUploadViewModel
    {

       [Required]
       public HttpPostedFileBase file { get; set; }

       public BookingStatus StatusList { get; set; }


       [Display(Name = "Start Date")]
       public DateTime StartDate { get; set; }

       [Display(Name = "Start Date")]
       public DateTime EndDate { get; set; }

       public string UploadMsg { get; set; }
    }
}
