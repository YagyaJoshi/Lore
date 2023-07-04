using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loregroup.Core.BusinessEntities
{
    public class JsonListModel<T>
    {
        public JsonListModel()
        {
            List = new List<T>();
            offerList = new List<T>();
            mallList = new List<T>();
            storeList = new List<T>();
            classifiedList = new List<T>();
        }

        public Boolean Result { get; set; }
        public List<T> List { get; set; }
        public List<T> offerList { get; set; }
        public List<T> mallList { get; set; }
        public List<T> storeList { get; set; }
        public List<T> classifiedList { get; set; }
        public String Message { get; set; }
    }

    public class JsonSingleObjectModel 
    {
        public Int64 Value { get; set; }
        public Boolean Result { get; set; }
        public String Message { get; set; }
        public String TextData { get; set; }
    
    }
   

    public class JsonObjectModel<T>
    {
        public Boolean Result { get; set; }
        public T Object { get; set; }
        public String Message { get; set; }
        public Boolean ChangeLocation { get; set; }
        public String RelativeUrl { get; set; }
    }
    public class ReturnModel
    {
        public Int64 id { get; set; }

    }
}
