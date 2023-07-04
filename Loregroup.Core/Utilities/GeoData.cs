using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Loregroup.Core.Utilities
{
    public class GeoData
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        
        public List<GeoData> GetCountryList()
        {
            List<GeoData> CountryList = new List<GeoData>();
            CountryList.Add(new GeoData { Id = 1, CountryName = "Ind" });
            CountryList.Add(new GeoData { Id = 2, CountryName = "Pak" });
            CountryList.Add(new GeoData { Id = 3, CountryName = "China" });
            CountryList.Add(new GeoData { Id = 4, CountryName = "Jap" });
            return CountryList;
        }
       
       
        
    }
    public class StateMaster
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }

        public List<StateMaster> GetStateList()
        {
            List<StateMaster> li = new List<StateMaster>();
            li.Add(new StateMaster { CountryId = 1, Id = 1, StateName = "MP" });
            li.Add(new StateMaster { CountryId = 1, Id = 2, StateName = "MH" });
            li.Add(new StateMaster { CountryId = 1, Id = 3, StateName = "Raj" });
            li.Add(new StateMaster { CountryId = 1, Id = 4, StateName = "Guj" });
            return li;
        }
        public List<SelectListItem> GetStateByCountryId(int CountryId)
        {
            var states = GetStateList().Where(m => m.CountryId == CountryId).ToList();
            List<SelectListItem> StateByCountry = new List<SelectListItem>();
            for (int i = 0; i < states.Count; i++)
            {
                StateByCountry.Add(new SelectListItem { Text = states[i].StateName, Value = states[i].Id.ToString() });
                
            }
            return StateByCountry;
            
        }
        
        
        
    }
    
}
