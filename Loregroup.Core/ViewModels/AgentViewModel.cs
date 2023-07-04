
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Loregroup.Core.ViewModels
    {
        public class AgentViewModel : BaseViewModel
        {
            public AgentViewModel()
            {
                AgentList = new List<AgentViewModel>();
            CategoryList = new List<CategoryViewModel>();
            CustomerList = new List<CustomerViewModel>();
        }
        public List<CustomerViewModel> CustomerList { get; set; }
        [Required]
        public Int64? CustomerId { get; set; }
        
        public List<CategoryViewModel> CategoryList { get; set; }
        public Int64? CategoryId { get; set; }

        [Required]
            [Display(Name = "Territory  Name")]
            public string territory { get; set; }
        [Display(Name = "Agents  Name")]
        public string AgentsName { get; set; }

        [Display(Name = "Description")]
            public string Description { get; set; }
 
        public Int64 AgentId { get; set; }
            public string Edit { get; set; }
            public string Delete { get; set; }

            public List<AgentViewModel> AgentList { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsActive { get; set; }
        }
    }


