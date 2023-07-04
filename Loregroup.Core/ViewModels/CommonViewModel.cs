using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class CommonViewModel {
        
    }

    public class FileViewModel {
        public Int64 Id { get; set; }
        public String FileUrl { get; set; }
        public String ThumbnailImageUrl { get; set; }
        public FileType FileType { get; set; }
        public FileType ThumbType { get; set; }
        public String FileName { get; set; }
        public String ThumbName { get; set; }
    }

    public class UserBriefViewModel {
        public Int64 Id { get; set; }
        public String UserName { get; set; }
        public String FullName { get; set; }
    
        public UserRole Role { get; set; }
        public DateTime DateJoined { get; set; }
        public Int64 DisplayImageId { get; set; }
        public Int64 RoleId { get; set; }
        public byte[] Profilepic { get; set; }
        public string Profimage { get; set; }

        public String OTP { get; set; }
        public Int64 UserRoleID { get; set; }
        public bool Ismall { get; set; }
        public bool Isstore { get; set; }
               public String FirstName { get; set; }
       // public Int64 DistributionPointId { get; set; }

    }
}
