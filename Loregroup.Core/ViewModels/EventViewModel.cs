using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;

namespace Loregroup.Core.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        public EventViewModel()
        {
            EventList = new List<EventViewModel>();
            CustomerModel = new CustomerViewModel();
        }
        public CustomerViewModel CustomerModel { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public string FromDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Address 2")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "City")]
        public string city { get; set; }

        [Required]
        [Display(Name = "mobileno")]
        public string mobileno { get; set; }
        
        [Required]
        [Display(Name = "Product Image")]
        public string image { get; set; }

        [Required]
        [Display(Name = "EventType")]
        public EventType EventTypeId { get; set; }

        [Display(Name = "CustomerId")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Booth Number")]
        public string BoothNumber { get; set; }

        [Display(Name = "WebsiteUrl")]
        public string WebsiteUrl { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<EventViewModel> EventList { get; set; }

        public string EventType { get; set; }
        public string Zipcode { get; set; }

    }
}





