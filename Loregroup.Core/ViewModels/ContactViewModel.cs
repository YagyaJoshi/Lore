using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
       public ContactViewModel() 
        {
           ContactList = new List<ContactViewModel>();
        }
       
       [Required]
       [Display(Name = "OfficeName")]
        public string OfficeName { get; set; }      

       [Required]
       [Display(Name = "Address")]
        public string Address { get; set; }

       [Required]
       [Display(Name = "ContactNo.")]
       public string Contactno { get; set; }

       [Required]
       [Display(Name = "Email")]
        public string Email { get; set; }         

        public string Edit { get; set; }
        public string Delete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<ContactViewModel> ContactList { get; set; }
       
    }
}

    
    
    
   


