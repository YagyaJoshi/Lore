using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.BusinessEntities
{
    public class XmlListModel<T>
    {
        public XmlListModel()
        {
            List = new List<T>();
            TableRecords = new DataTable();
        }

        public Boolean Result { get; set; }
        public List<T> List { get; set; }
        public String Message { get; set; }
        public String ZipFilePath { get; set; }
        public DataTable TableRecords { get; set; }
    }

    public class SymbolObjectModel<T>
    {
        public List<T> symbolsList { get; set; }
        public String ZipPath { get; set; }
    }

    public class TagObjectModel<T>
    {
        public List<T> tagsList { get; set; }
        public String ZipPath { get; set; }
    }

    public class XmlObjectModel<T>
    {
        public Int64 Value { get; set; }
        public Boolean Result { get; set; }
        public T Object { get; set; }
        public String Message { get; set; }
    }

    public class XmlSingleObjectModel
    {
        public Int64 Value { get; set; }
        public Boolean Result { get; set; }
        public String Message { get; set; }
        public String TextData { get; set; }
    }

    public class TemplateObjectModel<T>
    {
        public List<T> TemplateList { get; set; }
        public String ZipPath { get; set; }
    }
}
