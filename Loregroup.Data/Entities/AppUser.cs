using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class AppUser : BaseEntity
    {
        // personal information
        public String FirstName { get; set; }
        public String LastName { get; set; }
        [NotMapped]
        public String FullName
        {
            get { return FirstName + " " + LastName; }
        }
        /// <summary>
        /// based on enum Gender
        /// </summary>
        /// 
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public String CityName { get; set; }
        public int Gender { get; set; }
        public String Email { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Salt { get; set; }
        public Int64 ImageId { get; set; }
        public Int64 UserDesignationId { get; set; }
        // - personal information

        // company information
        public Int64 BusinessId { get; set; }
        //public String Affiliation { get; set; }
        //public Int64 CompanyDepartmentId { get; set; }
        public String PostalCode { get; set; }
        public String PhoneNumber { get; set; }
        // - company information

        public bool IsLocked { get; set; }

        /// <summary>
        /// based on enum UserLockReason
        /// </summary>
        public int LockedReason { get; set; }
        public Int64 LockedById { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsVerified { get; set; }
        public string VerificationCode { get; set; }
        public Int64 RoleId { get; set; }

        public virtual Role Role { get; set; }
        
        public virtual UserDesignation UserDesignation { get; set; }
        //public virtual City City { get; set; }

        public String MiddleName { get; set; }
        //public String EmployeeCode { get; set; }
        public DateTime? Dob { get; set; }
        public String Address { get; set; }
        public Int64? CityId { get; set; }
        public Int64? ZoneId { get; set; }
        //public string CustomerCode { get; set; }
        //public String Facebook { get; set; }
        //public String Website { get; set; }
        //public String Twitter { get; set; }
        //public String Providername { get; set; }
        public bool IsOTP { get; set; }
        //public byte[] ChequeImageId { get; set; }
        public String ProviderUserId { get; set; }
        //public virtual M_City City { get; set; }
        public byte[] ProfileImage { get; set; }
        //sulabh
        //public bool Ismall { get; set; }
        //public bool Isstore { get; set; }
        //public String LocalityId { get; set; }
        //public Int32? Occupation { get; set; }
        //public Int64 LoginCount { get; set; }
        public Int64 Id { get; set; }
        public Int64? DistricId { get; set; }
    }
}
