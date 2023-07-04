using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
  public class State_MasterViewModel
    {
      public State_MasterViewModel()
        {

            ZoneList = new List<State_MasterViewModel>();
        }
        public int ID { get; set; }
        public String StateName { get; set; }
        public String StateShortName { get; set; }
        public int IsDel { get; set; }
        public List<State_MasterViewModel> ZoneList { get; set; }
    }
}
