using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loregroup.Core.Enumerations;

namespace Loregroup.Core.BusinessEntities
{
    public class FileUploadDetailModel {
        public Int64 Id { get; set; }
        public FileRelation Relation { get; set; }
        public Int64 UploadedForId { get; set; }
    }
    public class JsonUploadModel
    {
        public Boolean Result { get; set; }
        public String Message { get; set; }
        public String File { get; set; }
        public String RealFile { get; set; }
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public String ImageId { get; set; }
        public String Weight { get; set; }
    }

    public class JsonMultipleUploadModel
    {
        public JsonMultipleUploadModel()
        {
            this.Files = new List<JsonFileModel>();
        }

        public Boolean Result { get; set; }
        public String Message { get; set; }
        public List<JsonFileModel> Files { get; set; }
    }

    public class JsonFileModel
    {
        public Boolean IsOk { get; set; }
        public String File { get; set; }
        public String RealFile { get; set; }
        public String Thumbnail { get; set; }
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public String ImageId { get; set; }
        public String FileSize { get; set; }
    }
}
