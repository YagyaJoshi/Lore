using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Enumerations
{
    //class Enumerations
    //{
    //}
    public class Enumerations
    {

    }

    public enum BookingStatus
    {      
        Delivered = 1,
        [Display(Name = "Under Delivery")]
        UnderDelivery = 2,
        Open = 3,
    }

    public enum UserRole
    {
        [Display(Name = "--Select User Role--")]
        select = 0,
        [Display(Name = "Super Admin")]
        SuperAdmin = 1,
        [Display(Name = "Admin")]
        Admin = 2,
        [Display(Name = "Shop")]
        Shop = 3,
        [Display(Name = "Staff")]
        Staff = 4,
        [Display(Name = "Agent")]
        Agent = 5,
        [Display(Name = "Public")]
        Public = 6,
        [Display(Name = "Supplier")]
        Supplier = 7,
    }

    public enum CustomerType
    {
        [Display(Name = "--Select Customer Type--")]
        select = 0,
        Shop = 1,
        Distributor = 2,
        //Retail = 3
        Public = 3
    }

    public enum Currency
    {
        [Display(Name = "--Select Currency--")]
        select = 0,
        USD = 1,
        EURO = 2,
        GBP = 3
    }


    public enum PurchaseOrderStatus
    {
        [Display(Name = "--Select Status--")]
        Select = 0,
        New = 1, // blue
        Sent = 2,//Orange
        [Display(Name = "Partially Received")]
        PartiallyReceived = 3,//purple
        Completed = 4,//Green
        Cancelled = 5,//red
        Confirmed = 6,//Medium Green #27AE5D
        [Display(Name = "China Warehouse")]
        ChinaWarehouse = 7,//Brown #795C32
        Ready = 8,  //Red Orange color #FF3F3F
    }


    public enum OrderStatus
    {
        [Display(Name = "--Select Status--")]
        Select = 0,
        New = 1, //blue 
        [Display(Name = "In Progress")]
        InProgress = 2, //Orange
        [Display(Name = "Payment Failed")]
        PaymentFailed = 3, //red
        Completed = 4,//green
        Cancelled = 5,//purple
        [Display(Name = "CN-ST")]
        CN_ST = 6,//yellow
        [Display(Name = "Partially Dispatched")]
        PartiallyDispatched = 7,//Brown #795C32
        [Display(Name = "Awaiting Confirmation")]
        AwaitingConfirmation = 8,//Light Green #6da56b 
        [Display(Name = "In Stock")]
        InStock = 9,//Red Orange color #FF3F3F
    }


    public enum DistributionPoint
    {
        [Display(Name = "--Select Distribution Point--")]
        select = 0,
        USA = 1,
        UK = 2,
        EU=3,
        //CN= 10003
        CN = 4
    }

    public enum Gender
    {
        [Display(Name = "--Select Gender--")]
        select = 0,
        Male = 1,
        Female = 2
    }

    public enum categoryType
    {
        [Display(Name = "--Select Category type--")]
        select = 0,
        Offer = 1,
        MobileApp = 2,
        Classifieds = 3
    }

    public enum CheckingStatus
    {
        Done = 1,
        Pending = 2,
        Download = 3,
        Cancel = 4,
        Due = 5
    }

    public enum UserType
    {
        [Display(Name = "--Select UserType--")]
        Select = 0,
        User = 1,
        Store = 2,
        Mall = 3
    }

    public enum StatusNew
    {
        [Display(Name = "--Select Status--")]
        select = 0,
        Pending = 3,
        Deleted = 4,
        Approved = 5,
        DisApproved = 6,        
    }

    public enum ApplicationType
    {
        [Display(Name = "--Select ApplicationType--")]
        select = 0,
        Free = 1,
        Paid = 2
    }

    public enum HttpRequestType
    {
        Redirect = 1,
        Ajax = 2,
        Nothing = 3,
        Overlay = 4,
        Alert = 5
    }

    public enum PermissionType
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
        Publish = 4,
        View = 5
    }

    public enum NotificationType
    {
        None = 1,
        Message = 2,
        Notification = 3,
        Task = 4,
    }

    public enum Status
    {
        [Display(Name = "--Select Status--")]
        select = 0,
        Active = 1,
        InActive = 2,
        Pending = 3,
        Deleted = 4,
        Approved = 5,
        DisApproved = 6,
        //Add New Status 
        KYCPending = 7,
        KYCApproved = 8,
        KYCDisApproved = 9
    }

    //public enum Status
    //{
    //    none = 0,
    //    Active = 1,
    //    InActive = 2,
    //    Pending = 3,
    //    Deleted = 4,
    //    Read = 5,
    //    Unread = 6,
    //    Seed = 7
    //}

    public enum UserLockReason
    {
        [Display(Name = "--Select Reason--")]
        select = 0,
        NoLock = 1,
        Attemts = 2,
        ByUser = 3,
    }

    public enum FileType
    {
        Jpeg = 1,
        Png = 2,
        Gig = 3,
        Bmp = 4,
        Csv = 5,
        Xls = 6,
        Pdf = 7,
        Doc = 8,
        Txt = 9,
        Anonymous = 10,
    }

    public enum FileRelation
    {
        UserImage = 1,
        TestCase = 2,
        ProductImage = 3,
        WorkOrders = 4,
        WorkOrderUpdate = 5,
        GeneralQuery = 6,
        GeneralQueryUpdate = 7
    }

    public enum ConfigSetting
    {
        Storage = 1
    }

    public enum WorkOrderType
    {
        TestCaseStudy = 1,
        FeasibilityStudy = 2
    }

    public enum AnalysisTechnique
    {
        A,
        B
    }

    public enum WorkStatus
    {
        New = 1,
        InProcess = 2,
        Verify = 3,
        Pending = 4,
        Roadblocked = 5,
        UAT = 6,
        Done = 7
    }

    public enum Department
    {
        Manugacturing = 1,
        Purchas = 2,
        Sales = 3

    }
    public enum Designation
    {
        Manager = 1,
        Operator = 2
    }

    public enum MaterialType
    {
        Solid = 1,
        Liquid = 2,
        Semisolid = 3
    }

    public enum GeneralQueryType
    {
        Normal = 1,
        Hardware = 2
    }

    public enum PCAMethod
    {
        SVD = 1,
        EVD = 2,
        NIPALS = 3
    }

    public enum NotificationTypes
    {
        Higher = 1,
        Message = 2,
        Notification = 3,
        Task = 4,
        Alert = 5,
    }

    public enum TypeHolding
    {
        Nav,
        SubNav,
        ZNav,
        roleid,
        widget
    }

    public enum Questiontype
    {
        [Display(Name = "--Select Questiontype--")]
        select = 0,
        Critical = 1,
        noncritical = 2
    }

    public enum ResultStatusNew
    {
        [Display(Name = "--Select Status--")]
        select = 0,

        Pass = 1,
        Fail = 2
    }

    public enum AppUserstatus
    {
        Active = 1,
        Deactive = 2,
        Blocked = 3
    }
    public enum StateMaster
    {
        [Display(Name = "Andhra Pradesh")]
        Andhra_Pradesh = 1,
        [Display(Name = "Arunachal Pradesh")]
        Arunachal_Pradesh = 2,
        Assam = 3,
        Bihar = 4,
        Chhattisgarh = 5,
        Goa = 6,
        Gujarat = 7,
        Haryana = 8,
        [Display(Name = "Himachal Pradesh")]
        Himachal_Pradesh = 9,
        [Display(Name = "Jammu & Kashmir")]
        Jammu_Kashmir = 10,
        Jharkhand = 11,
        Karnataka = 12,
        Kerala = 13,
        [Display(Name = "Madhya Pradesh")]
        Madhya_Pradesh = 14,
        Maharashtra = 15,
        Manipur = 16,
        Meghalaya = 17,
        Mizoram = 18,
        Nagaland = 19,
        Orissa = 20,
        Punjab = 21,
        Rajasthan = 22,
        Sikkim = 23,
        [Display(Name = "Tamil Nadu")]
        Tamil_Nadu = 24,
        Telangana = 25,
        Tripura = 26,
        [Display(Name = "Uttar Pradesh")]
        Uttar_Pradesh = 27,
        Uttarakhand = 28,
        [Display(Name = "West Bengal")]
        West_Bengal = 29
    }
    public enum PolicyStatus
    {
        Expired = 1,
        Live = 2
    }
    public enum EmployeeStatus
    {
        Active = 1,
        Deactive = 2,
        Blocked = 3
    }
    public enum AgentStatus
    {
        //Active = 1,
        //Deactive = 2,
        //Blocked = 3
        select = 0,
        Active = 1,
        InActive = 2,
        Pending = 3,
        Deleted = 4,
        Approved = 5,
        DisApproved = 6,
        //Add New Status 
        KYCPending = 7,
        KYCApproved = 8,
        KYCDisApproved = 9
    }
    public enum MaritialStatus
    {
        Single = 1,
        Married = 2,
        Widow = 3

    }
    public enum Nationality
    {
        Indian = 1,
        NRI = 2,
        Others = 3
    }
    public enum RelationShip
    {
        Father = 1,
        Mother = 2,
        Son = 3,
        Daughter = 4,
        Husband = 5,
        Wife = 6,
        Other = 7

    }
    public enum Location
    {
        Rural = 1,
        Urban = 2
    }
    //public enum Category
    //{
    //    General = 1,
    //    Sc = 2,
    //    St = 3,
    //    OBC = 4
    //}

    public enum AddressProofSelection
    {
        [Display(Name = "--Select Status--")]
        select = 0,
        BankCertificate = 1,
        DrivingLicense = 2,
        ElectricityORTelephoneBill = 3,
        Passport = 4,
        EmployerCertification = 5,
        SocietyMaintenanceBill = 6,
        Others = 7
    }

    public enum IdentityProofSelection
    {
        [Display(Name = "--Select Status--")]
        select = 0,
        DrivingLicense = 1,
        VoterIDCard = 2,
        Passport = 3,
        PANCard = 4,
        BankCertification = 5,
        DefenceIDCard = 6,
        EmployerCertification = 7,
        Others = 8
    }

    public enum AgeProofSelection
    {
        [Display(Name = "--Select Status--")]
        select = 0,
        SchoolCertTransferCertMarkSheet = 1,
        BaptismCert = 2,
        MarriageCert = 3,
        EmployerCert = 4,
        ValidPassport = 5,
        DefenceIDCard = 6,
        AadharCard = 7,
        GovtPensionOrders = 8,
        DrivingLicense = 9,
        MunicipalBirthCert = 10,
        PANCard = 11,
        others = 12
    }

    public enum PaymentModes
    {
        [Display(Name = "--Select PaymentMode--")]
        select = 0,
        Cheque = 1,
        DD = 2,
        PAYCREDITORDEBITCARD = 3,
        NEFTOnlineFundTransfer = 4,
    }

    public enum RlpStages
    {
        [Display(Name = "--Select User Role--")]
        Select = 0,
        [Display(Name = "Associate Club")]
        AssociateClub = 1,
        [Display(Name = "Silver Club")]
        SilverClub = 2,
        [Display(Name = "Gold Club")]
        GoldClub = 3,
        [Display(Name = "Emerald Club")]
        EmeraldClub = 4,
        [Display(Name = "Diamond Club")]
        DiamondClub = 5,
        [Display(Name = "Platinum Club")]
        PlatinumClub = 6,
        [Display(Name = "Winners Club")]
        WinnersClub = 7
    }

    public enum SortingOption
    {
        [Display(Name = "Popular")]
        Popular = 1,
        [Display(Name = "Latest")]
        Latest = 2,
        [Display(Name = "Ending Soon")]
        EndingSoon = 3
    }

    public enum SortBy
    {
        [Display(Name = "Most Popular")]
        Popular = 1,
        [Display(Name = "Recently Added")]
        Latest = 2,
    }

    public enum EventType
    {
        [Display(Name = "--Select Event--")]
        Select = 0,
        [Display(Name = "Trunk Shows")]
        TrunkShows = 1, //blue
        [Display(Name = "Trade Shows")]
        TradeShows = 2, //Orange        
    }

    public enum GalleryType
    {
        [Display(Name = "--Select Type--")]
        Select = 0,
        Image = 1, // blue
        Video = 2,//Orange       
    }

}
