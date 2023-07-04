namespace Loregroup.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTablesForDeevoir : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        APIKey = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        FileLibraryId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibraryId)
                .Index(t => t.FileLibraryId);
            
            CreateTable(
                "dbo.FileLibraries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsInFileSystem = c.Boolean(nullable: false),
                        FileName = c.String(),
                        ThumbnailName = c.String(),
                        FilePath = c.String(),
                        ThumbnailPath = c.String(),
                        FileType = c.Int(nullable: false),
                        ThumbnailFileType = c.Int(nullable: false),
                        FileRelation = c.Int(nullable: false),
                        FileBytes = c.Binary(),
                        ThumbnailBytes = c.Binary(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GeneralQueries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        BusinessId = c.Long(nullable: false),
                        Email = c.String(),
                        UserDesignationId = c.Long(nullable: false),
                        Title = c.String(),
                        QueryTypeId = c.Long(nullable: false),
                        Description = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QueryTypes", t => t.QueryTypeId)
                .ForeignKey("dbo.UserDesignations", t => t.UserDesignationId)
                .Index(t => t.UserDesignationId)
                .Index(t => t.QueryTypeId);
            
            CreateTable(
                "dbo.GeneralQueryUpdates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReplyById = c.Long(nullable: false),
                        Description = c.String(),
                        GeneralQueryId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GeneralQueries", t => t.GeneralQueryId)
                .Index(t => t.GeneralQueryId);
            
            CreateTable(
                "dbo.QueryTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDesignations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AgentAnswers",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Agentid = c.Long(),
                        Questionid = c.Long(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AgentRegistrations", t => t.Agentid)
                .ForeignKey("dbo.AgentScoreQuestions", t => t.Questionid)
                .Index(t => t.Agentid)
                .Index(t => t.Questionid);
            
            CreateTable(
                "dbo.AgentRegistrations",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Status = c.String(),
                        branchCode = c.Int(),
                        regioncode = c.Int(),
                        zonecode = c.Int(),
                        fullname = c.String(),
                        fathersHusbandName = c.String(),
                        gender = c.Int(),
                        dob = c.DateTime(),
                        maritialstatus = c.Int(),
                        nationality = c.Int(),
                        nominee = c.String(),
                        relationship = c.Int(),
                        Age = c.Int(),
                        bankName = c.String(),
                        accountNumber = c.String(),
                        Bank = c.String(),
                        pancardnumber = c.String(),
                        polySalusBranchName = c.String(),
                        location = c.Int(),
                        presentAddress = c.String(),
                        permanentAddress = c.String(),
                        phoneOffice = c.String(),
                        fax = c.String(),
                        phoneRes = c.String(),
                        mobile = c.String(),
                        email = c.String(),
                        classX = c.Boolean(),
                        classXII = c.Boolean(),
                        graduate = c.Boolean(),
                        postGraduate = c.Boolean(),
                        boardName = c.String(),
                        rollNo = c.String(),
                        year = c.String(),
                        category = c.Int(),
                        Profession = c.String(),
                        parttime = c.String(),
                        fultime = c.String(),
                        primaryRef = c.String(),
                        secondaryRef = c.String(),
                        isSelected = c.Boolean(),
                        educationProof = c.String(),
                        photographs = c.Boolean(),
                        selectedBy = c.Int(),
                        verifiedby = c.Int(),
                        verifiedOn = c.DateTime(),
                        approvedBy = c.DateTime(),
                        imagePath = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AgentScoreQuestions",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Question = c.String(),
                        Criteria = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AgentEmpDetails",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        AgentResid = c.Long(),
                        DteFrom = c.DateTime(),
                        DteTo = c.DateTime(),
                        NameAdd = c.String(),
                        LastPosition = c.String(),
                        AnnualIncome = c.Single(nullable: false),
                        ResonForLeaving = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AgentRegistrations", t => t.AgentResid)
                .Index(t => t.AgentResid);
            
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        AgentResid = c.Long(),
                        branchCode = c.Long(),
                        regioncode = c.Long(),
                        zonecode = c.Long(),
                        fullname = c.String(),
                        fathersHusbandName = c.String(),
                        gender = c.Long(),
                        dob = c.DateTime(),
                        maritialstatus = c.Long(),
                        nationality = c.Long(),
                        nominee = c.String(),
                        relationship = c.Long(),
                        Age = c.Long(),
                        bankName = c.String(),
                        accountNumber = c.String(),
                        Bank = c.String(),
                        pancardnumber = c.String(),
                        polySalusBranchName = c.String(),
                        location = c.Long(),
                        presentAddress = c.String(),
                        permanentAddress = c.String(),
                        phoneOffice = c.String(),
                        fax = c.String(),
                        phoneRes = c.String(),
                        mobile = c.String(),
                        email = c.String(),
                        status = c.Long(),
                        classX = c.Boolean(),
                        classXII = c.Boolean(),
                        graduate = c.Boolean(),
                        postGraduate = c.Boolean(),
                        boardName = c.String(),
                        rollNo = c.String(),
                        year = c.String(),
                        category = c.Long(),
                        Profession = c.String(),
                        parttime = c.String(),
                        fultime = c.String(),
                        primaryRef = c.String(),
                        secondaryRef = c.String(),
                        isSelected = c.Boolean(),
                        educationProof = c.String(),
                        photographs = c.Boolean(),
                        selectedBy = c.Long(),
                        verifiedby = c.Long(),
                        verifiedOn = c.DateTime(),
                        approvedBy = c.DateTime(),
                        imagePath = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AgentRegistrations", t => t.AgentResid)
                .Index(t => t.AgentResid);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AuthorName = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        branchName = c.String(),
                        city = c.String(),
                        address = c.String(),
                        phone = c.String(),
                        fax = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        StateId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CountryId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        LicenseKey = c.String(nullable: false),
                        LicenseExpireDate = c.String(),
                        LicenseTypeId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        CountryId = c.Long(nullable: false),
                        DatabaseName = c.String(nullable: false),
                        DatabaseUserName = c.String(),
                        DataBasePassword = c.String(),
                        Salt = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.CityId)
                .Index(t => t.StateId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.CompanyGeneralDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IndustryId = c.Long(nullable: false),
                        CompanyId = c.Long(nullable: false),
                        CountryId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        ZipCode = c.String(),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        PrimaryEmail = c.String(),
                        SecondaryEmail = c.String(),
                        PrimaryPhone = c.String(),
                        SecondaryPhone = c.String(),
                        AboutCompany = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.Industries", t => t.IndustryId)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.IndustryId)
                .Index(t => t.CountryId)
                .Index(t => t.StateId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IndustryType = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyPackageDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        LicenseId = c.Long(nullable: false),
                        LicenseSubscriptionId = c.Long(nullable: false),
                        PaymentModeId = c.Long(nullable: false),
                        DeploymentCost = c.String(),
                        SitewiseBillAmount = c.String(),
                        PerProcedureCharge = c.String(),
                        FreeProcedure = c.String(),
                        WhiteLabelSystem = c.Boolean(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Licenses", t => t.LicenseId)
                .ForeignKey("dbo.LicenseSubscriptions", t => t.LicenseSubscriptionId)
                .ForeignKey("dbo.PaymentModes", t => t.PaymentModeId)
                .Index(t => t.CompanyId)
                .Index(t => t.LicenseId)
                .Index(t => t.LicenseSubscriptionId)
                .Index(t => t.PaymentModeId);
            
            CreateTable(
                "dbo.Licenses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Details = c.String(),
                        AutoDisable = c.Boolean(nullable: false),
                        Reminderdays = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LicenseSubscriptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LicenseId = c.Long(nullable: false),
                        SchemeName = c.String(),
                        Durations = c.String(),
                        ServiceCharges = c.String(),
                        FreeProcedure = c.String(),
                        ProcedureCharge = c.String(),
                        Scale = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Licenses", t => t.LicenseId)
                .Index(t => t.LicenseId);
            
            CreateTable(
                "dbo.PaymentModes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PaymentModeName = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerFHistrories",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Customerid = c.Long(),
                        CustomerName = c.String(),
                        FamilyMember = c.String(),
                        CauseOfDeath = c.String(),
                        IfAlive = c.Boolean(),
                        IfDeceased = c.Boolean(),
                        AgeOfDeath = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CustomerRegistrations", t => t.Customerid)
                .Index(t => t.Customerid);
            
            CreateTable(
                "dbo.CustomerRegistrations",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        status = c.String(),
                        annualDeposite = c.Single(),
                        bankCode = c.String(),
                        ddChqNo = c.String(),
                        ifscCode = c.String(),
                        branch = c.String(),
                        urban = c.Boolean(),
                        Mr = c.Boolean(),
                        MS = c.Boolean(),
                        FullName = c.String(),
                        FathersName = c.String(),
                        gender = c.Int(),
                        dob = c.DateTime(),
                        maritialStatus = c.Int(),
                        postGraduate = c.Boolean(),
                        graduate = c.Boolean(),
                        diploma = c.Boolean(),
                        hsc = c.Boolean(),
                        ssc = c.Boolean(),
                        belowSsc = c.Boolean(),
                        uneducated = c.Boolean(),
                        others = c.Boolean(),
                        annualIncome = c.Single(),
                        incomeSource = c.String(),
                        purposeOfInsurance = c.String(),
                        bankAccount = c.String(),
                        bank = c.String(),
                        cancelledCheque = c.Boolean(),
                        passbookCopy = c.Boolean(),
                        nationality = c.Int(),
                        occupations = c.String(),
                        jobDescription = c.String(),
                        addressProof = c.String(),
                        ageProof = c.String(),
                        identyProof = c.String(),
                        Co = c.String(),
                        houseNo = c.String(),
                        buildingName = c.String(),
                        streetName = c.String(),
                        landmark = c.String(),
                        district = c.String(),
                        city = c.String(),
                        state = c.Int(),
                        pincode = c.String(),
                        mobile = c.String(),
                        email = c.String(),
                        phone = c.String(),
                        nominee = c.String(),
                        appointDate = c.DateTime(),
                        relationWithNomonee = c.Int(),
                        appointeesName = c.String(),
                        appointeeDob = c.DateTime(),
                        appointeeAddressProof = c.String(),
                        appointeeIdentyProof = c.String(),
                        appointeeAddress = c.String(),
                        appointeeCity = c.String(),
                        appointeePinCode = c.String(),
                        appointeeState = c.Int(),
                        electronicCommunication = c.Boolean(),
                        familyMemberdeathBelowSixty = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.CustomerLSQAnswers",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Customerid = c.Long(),
                        QId = c.Int(),
                        Ans = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Customers", t => t.Customerid)
                .Index(t => t.Customerid);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        CustomerRegid = c.Long(),
                        annualDeposite = c.Single(),
                        bankCode = c.String(),
                        ddChqNo = c.String(),
                        ifscCode = c.String(),
                        branch = c.String(),
                        urban = c.Boolean(),
                        Mr = c.Boolean(),
                        MS = c.Boolean(),
                        FullName = c.String(),
                        FathersName = c.String(),
                        gender = c.Int(),
                        dob = c.DateTime(),
                        maritialStatus = c.Int(),
                        postGraduate = c.Boolean(),
                        graduate = c.Boolean(),
                        diploma = c.Boolean(),
                        hsc = c.Boolean(),
                        ssc = c.Boolean(),
                        belowSsc = c.Boolean(),
                        uneducated = c.Boolean(),
                        others = c.Boolean(),
                        annualIncome = c.Single(),
                        incomeSource = c.String(),
                        purposeOfInsurance = c.String(),
                        bankAccount = c.String(),
                        bank = c.String(),
                        cancelledCheque = c.Boolean(),
                        passbookCopy = c.Boolean(),
                        nationality = c.Int(),
                        occupations = c.String(),
                        jobDescription = c.String(),
                        addressProof = c.String(),
                        ageProof = c.String(),
                        identyProof = c.String(),
                        Co = c.String(),
                        houseNo = c.String(),
                        buildingName = c.String(),
                        streetName = c.String(),
                        landmark = c.String(),
                        district = c.String(),
                        city = c.String(),
                        state = c.Int(),
                        pincode = c.String(),
                        mobile = c.String(),
                        email = c.String(),
                        phone = c.String(),
                        nominee = c.String(),
                        appointDate = c.DateTime(),
                        relationWithNomonee = c.Int(),
                        appointeesName = c.String(),
                        appointeeDob = c.DateTime(),
                        appointeeAddressProof = c.String(),
                        appointeeIdentyProof = c.String(),
                        appointeeAddress = c.String(),
                        appointeeCity = c.String(),
                        appointeePinCode = c.String(),
                        appointeeState = c.Int(),
                        electronicCommunication = c.Boolean(),
                        familyMemberdeathBelowSixty = c.Boolean(),
                        familyHistory = c.String(),
                        lifestyleQuestions = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CustomerRegistrations", t => t.CustomerRegid)
                .Index(t => t.CustomerRegid);
            
            CreateTable(
                "dbo.CustomerLSQMasters",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        QId = c.Int(nullable: false),
                        Question = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.CustomerPolicyDatails",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Customerid = c.Long(),
                        Packageid = c.Int(),
                        StartDate = c.DateTime(),
                        Package_id = c.Long(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Customers", t => t.Customerid)
                .ForeignKey("dbo.Packages", t => t.Package_id)
                .Index(t => t.Customerid)
                .Index(t => t.Package_id);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        PackageName = c.String(),
                        Days = c.Int(),
                        Amount = c.Single(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        address = c.String(),
                        mobile = c.String(),
                        email = c.String(),
                        status = c.Int(),
                        imagePath = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.LicenseRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        NumeberofUses = c.String(),
                        RoleStatus = c.Boolean(nullable: false),
                        LicenseId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Licenses", t => t.LicenseId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.LicenseId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LicenseServices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ServiceStatus = c.Boolean(nullable: false),
                        LicenseId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Licenses", t => t.LicenseId)
                .Index(t => t.LicenseId);
            
            CreateTable(
                "dbo.LogoSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Logo_Text = c.String(),
                        Logo_Slogen = c.String(),
                        FileLibrariesId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        FileLibrary_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibrary_Id)
                .Index(t => t.FileLibrary_Id);
            
            CreateTable(
                "dbo.MessageSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Host = c.String(),
                        Port = c.Long(),
                        UserNameEmail = c.String(),
                        PasswordMail = c.String(),
                        RequireSSL = c.Boolean(nullable: false),
                        UserId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        Email = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        ImageId = c.Long(nullable: false),
                        UserDesignationId = c.Long(nullable: false),
                        BusinessId = c.Long(nullable: false),
                        Affiliation = c.String(),
                        CompanyDepartmentId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        PostalCode = c.String(),
                        PhoneNumber = c.String(),
                        IsLocked = c.Boolean(nullable: false),
                        LockedReason = c.Int(nullable: false),
                        LockedById = c.Long(nullable: false),
                        FailedLoginAttempts = c.Int(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        VerificationCode = c.String(),
                        RoleId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.FileLibraries", t => t.ImageId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.UserDesignations", t => t.UserDesignationId)
                .Index(t => t.ImageId)
                .Index(t => t.UserDesignationId)
                .Index(t => t.CityId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MYOBSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        APIKey = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        FileLibraryId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibraryId)
                .Index(t => t.FileLibraryId);
            
            CreateTable(
                "dbo.Navigations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HasSubMenu = c.Boolean(nullable: false),
                        Icon = c.String(),
                        Text = c.String(),
                        ActionUrl = c.String(),
                        ActionUrlRequestType = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        SenderId = c.Long(nullable: false),
                        NotificationType = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.PackageDetails",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Packageid = c.Long(),
                        Serviceid = c.Long(),
                        Apply = c.Boolean(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Packages", t => t.Packageid)
                .ForeignKey("dbo.PolicyServices", t => t.Serviceid)
                .Index(t => t.Packageid)
                .Index(t => t.Serviceid);
            
            CreateTable(
                "dbo.PolicyServices",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        ServiceName = c.String(),
                        description = c.String(),
                        isAvaileble = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PaymentCardTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PaymentCardTypeName = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionMatrices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PermissionId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        PermissionStatus = c.Boolean(nullable: false),
                        TypeHold = c.String(),
                        AllNAvigationsId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.PermissionId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.PermissionId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Procedures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcedureName = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionMasters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcedureId = c.Long(nullable: false),
                        AutherId = c.Long(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                        QuestionType = c.Int(nullable: false),
                        Obj1 = c.String(),
                        Obj2 = c.String(),
                        Obj3 = c.String(),
                        Obj4 = c.String(),
                        IsRecurring = c.Boolean(nullable: false),
                        RetakenTime = c.Int(nullable: false),
                        FailRetakenTime = c.Int(nullable: false),
                        ForCorrect = c.String(),
                        InCorrect = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QuestionId = c.Long(nullable: false),
                        QuestionText = c.String(),
                        obj1 = c.String(),
                        obj2 = c.String(),
                        obj3 = c.String(),
                        obj4 = c.String(),
                        correct = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionWheels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IndustryTypeId = c.Long(nullable: false),
                        QuestionText = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubNavigations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Icon = c.String(),
                        Text = c.String(),
                        ActionUrl = c.String(),
                        ActionUrlRequestType = c.Int(nullable: false),
                        NavigationId = c.Long(nullable: false),
                        Order = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Navigations", t => t.NavigationId)
                .Index(t => t.NavigationId);
            
            CreateTable(
                "dbo.SymbolLibraries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SymbolMeaning = c.String(),
                        IndustryId = c.Long(nullable: false),
                        Description = c.String(),
                        FileLibrariesId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.IndustryId)
                .Index(t => t.IndustryId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TemplateCode = c.String(nullable: false),
                        Title = c.String(),
                        FileLibraryId = c.Long(nullable: false),
                        Description = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibraryId)
                .Index(t => t.FileLibraryId);
            
            CreateTable(
                "dbo.TestNotifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcedureId = c.Long(nullable: false),
                        EndUserId = c.Long(nullable: false),
                        Notification = c.String(),
                        NotificationDate = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcedureId = c.Long(nullable: false),
                        EndUserId = c.Long(nullable: false),
                        ReattemptType = c.Int(nullable: false),
                        ResultStatus = c.Int(nullable: false),
                        TestAttemptNo = c.Int(nullable: false),
                        TestTimeDuration = c.String(),
                        Suggestion = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestRecords_Detail",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TestId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        Answer = c.String(),
                        AnswerStatus = c.Int(nullable: false),
                        FeedBack = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestRecordsDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TestId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        Answer = c.String(),
                        AnswerStatus = c.Int(nullable: false),
                        FeedBack = c.String(),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TranningVideos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        VideoTitle = c.String(),
                        Description = c.String(),
                        FileLibraryId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibraryId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.FileLibraryId);
            
            CreateTable(
                "dbo.DashBoradWidgets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WidgetName = c.String(),
                        DisplayonDashboard = c.Boolean(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        ModifiedById = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GeneralQueryFileLibraries",
                c => new
                    {
                        GeneralQuery_Id = c.Long(nullable: false),
                        FileLibrary_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.GeneralQuery_Id, t.FileLibrary_Id })
                .ForeignKey("dbo.GeneralQueries", t => t.GeneralQuery_Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibrary_Id)
                .Index(t => t.GeneralQuery_Id)
                .Index(t => t.FileLibrary_Id);
            
            CreateTable(
                "dbo.GeneralQueryUpdateFileLibraries",
                c => new
                    {
                        GeneralQueryUpdate_Id = c.Long(nullable: false),
                        FileLibrary_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.GeneralQueryUpdate_Id, t.FileLibrary_Id })
                .ForeignKey("dbo.GeneralQueryUpdates", t => t.GeneralQueryUpdate_Id)
                .ForeignKey("dbo.FileLibraries", t => t.FileLibrary_Id)
                .Index(t => t.GeneralQueryUpdate_Id)
                .Index(t => t.FileLibrary_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TranningVideos", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.TranningVideos", "FileLibraryId", "dbo.FileLibraries");
            DropForeignKey("dbo.Templates", "FileLibraryId", "dbo.FileLibraries");
            DropForeignKey("dbo.SymbolLibraries", "IndustryId", "dbo.Industries");
            DropForeignKey("dbo.SubNavigations", "NavigationId", "dbo.Navigations");
            DropForeignKey("dbo.PermissionMatrices", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.PermissionMatrices", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.PackageDetails", "Serviceid", "dbo.PolicyServices");
            DropForeignKey("dbo.PackageDetails", "Packageid", "dbo.Packages");
            DropForeignKey("dbo.Notifications", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.MYOBSettings", "FileLibraryId", "dbo.FileLibraries");
            DropForeignKey("dbo.MessageSettings", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.AppUsers", "UserDesignationId", "dbo.UserDesignations");
            DropForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AppUsers", "ImageId", "dbo.FileLibraries");
            DropForeignKey("dbo.AppUsers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.LogoSettings", "FileLibrary_Id", "dbo.FileLibraries");
            DropForeignKey("dbo.LicenseServices", "LicenseId", "dbo.Licenses");
            DropForeignKey("dbo.LicenseRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.LicenseRoles", "LicenseId", "dbo.Licenses");
            DropForeignKey("dbo.CustomerPolicyDatails", "Package_id", "dbo.Packages");
            DropForeignKey("dbo.CustomerPolicyDatails", "Customerid", "dbo.Customers");
            DropForeignKey("dbo.CustomerLSQAnswers", "Customerid", "dbo.Customers");
            DropForeignKey("dbo.Customers", "CustomerRegid", "dbo.CustomerRegistrations");
            DropForeignKey("dbo.CustomerFHistrories", "Customerid", "dbo.CustomerRegistrations");
            DropForeignKey("dbo.CompanyPackageDetails", "PaymentModeId", "dbo.PaymentModes");
            DropForeignKey("dbo.CompanyPackageDetails", "LicenseSubscriptionId", "dbo.LicenseSubscriptions");
            DropForeignKey("dbo.LicenseSubscriptions", "LicenseId", "dbo.Licenses");
            DropForeignKey("dbo.CompanyPackageDetails", "LicenseId", "dbo.Licenses");
            DropForeignKey("dbo.CompanyPackageDetails", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyGeneralDetails", "StateId", "dbo.States");
            DropForeignKey("dbo.CompanyGeneralDetails", "IndustryId", "dbo.Industries");
            DropForeignKey("dbo.CompanyGeneralDetails", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.CompanyGeneralDetails", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Companies", "StateId", "dbo.States");
            DropForeignKey("dbo.Companies", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Companies", "CityId", "dbo.Cities");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.Agents", "AgentResid", "dbo.AgentRegistrations");
            DropForeignKey("dbo.AgentEmpDetails", "AgentResid", "dbo.AgentRegistrations");
            DropForeignKey("dbo.AgentAnswers", "Questionid", "dbo.AgentScoreQuestions");
            DropForeignKey("dbo.AgentAnswers", "Agentid", "dbo.AgentRegistrations");
            DropForeignKey("dbo.ActSettings", "FileLibraryId", "dbo.FileLibraries");
            DropForeignKey("dbo.GeneralQueries", "UserDesignationId", "dbo.UserDesignations");
            DropForeignKey("dbo.GeneralQueries", "QueryTypeId", "dbo.QueryTypes");
            DropForeignKey("dbo.GeneralQueryUpdates", "GeneralQueryId", "dbo.GeneralQueries");
            DropForeignKey("dbo.GeneralQueryUpdateFileLibraries", "FileLibrary_Id", "dbo.FileLibraries");
            DropForeignKey("dbo.GeneralQueryUpdateFileLibraries", "GeneralQueryUpdate_Id", "dbo.GeneralQueryUpdates");
            DropForeignKey("dbo.GeneralQueryFileLibraries", "FileLibrary_Id", "dbo.FileLibraries");
            DropForeignKey("dbo.GeneralQueryFileLibraries", "GeneralQuery_Id", "dbo.GeneralQueries");
            DropIndex("dbo.GeneralQueryUpdateFileLibraries", new[] { "FileLibrary_Id" });
            DropIndex("dbo.GeneralQueryUpdateFileLibraries", new[] { "GeneralQueryUpdate_Id" });
            DropIndex("dbo.GeneralQueryFileLibraries", new[] { "FileLibrary_Id" });
            DropIndex("dbo.GeneralQueryFileLibraries", new[] { "GeneralQuery_Id" });
            DropIndex("dbo.TranningVideos", new[] { "FileLibraryId" });
            DropIndex("dbo.TranningVideos", new[] { "RoleId" });
            DropIndex("dbo.Templates", new[] { "FileLibraryId" });
            DropIndex("dbo.SymbolLibraries", new[] { "IndustryId" });
            DropIndex("dbo.SubNavigations", new[] { "NavigationId" });
            DropIndex("dbo.PermissionMatrices", new[] { "RoleId" });
            DropIndex("dbo.PermissionMatrices", new[] { "PermissionId" });
            DropIndex("dbo.PackageDetails", new[] { "Serviceid" });
            DropIndex("dbo.PackageDetails", new[] { "Packageid" });
            DropIndex("dbo.Notifications", new[] { "RoleId" });
            DropIndex("dbo.MYOBSettings", new[] { "FileLibraryId" });
            DropIndex("dbo.AppUsers", new[] { "RoleId" });
            DropIndex("dbo.AppUsers", new[] { "CityId" });
            DropIndex("dbo.AppUsers", new[] { "UserDesignationId" });
            DropIndex("dbo.AppUsers", new[] { "ImageId" });
            DropIndex("dbo.MessageSettings", new[] { "UserId" });
            DropIndex("dbo.LogoSettings", new[] { "FileLibrary_Id" });
            DropIndex("dbo.LicenseServices", new[] { "LicenseId" });
            DropIndex("dbo.LicenseRoles", new[] { "LicenseId" });
            DropIndex("dbo.LicenseRoles", new[] { "RoleId" });
            DropIndex("dbo.CustomerPolicyDatails", new[] { "Package_id" });
            DropIndex("dbo.CustomerPolicyDatails", new[] { "Customerid" });
            DropIndex("dbo.Customers", new[] { "CustomerRegid" });
            DropIndex("dbo.CustomerLSQAnswers", new[] { "Customerid" });
            DropIndex("dbo.CustomerFHistrories", new[] { "Customerid" });
            DropIndex("dbo.LicenseSubscriptions", new[] { "LicenseId" });
            DropIndex("dbo.CompanyPackageDetails", new[] { "PaymentModeId" });
            DropIndex("dbo.CompanyPackageDetails", new[] { "LicenseSubscriptionId" });
            DropIndex("dbo.CompanyPackageDetails", new[] { "LicenseId" });
            DropIndex("dbo.CompanyPackageDetails", new[] { "CompanyId" });
            DropIndex("dbo.CompanyGeneralDetails", new[] { "CityId" });
            DropIndex("dbo.CompanyGeneralDetails", new[] { "StateId" });
            DropIndex("dbo.CompanyGeneralDetails", new[] { "CountryId" });
            DropIndex("dbo.CompanyGeneralDetails", new[] { "IndustryId" });
            DropIndex("dbo.Companies", new[] { "CountryId" });
            DropIndex("dbo.Companies", new[] { "StateId" });
            DropIndex("dbo.Companies", new[] { "CityId" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.Agents", new[] { "AgentResid" });
            DropIndex("dbo.AgentEmpDetails", new[] { "AgentResid" });
            DropIndex("dbo.AgentAnswers", new[] { "Questionid" });
            DropIndex("dbo.AgentAnswers", new[] { "Agentid" });
            DropIndex("dbo.GeneralQueryUpdates", new[] { "GeneralQueryId" });
            DropIndex("dbo.GeneralQueries", new[] { "QueryTypeId" });
            DropIndex("dbo.GeneralQueries", new[] { "UserDesignationId" });
            DropIndex("dbo.ActSettings", new[] { "FileLibraryId" });
            DropTable("dbo.GeneralQueryUpdateFileLibraries");
            DropTable("dbo.GeneralQueryFileLibraries");
            DropTable("dbo.DashBoradWidgets");
            DropTable("dbo.TranningVideos");
            DropTable("dbo.TestRecordsDetails");
            DropTable("dbo.TestRecords_Detail");
            DropTable("dbo.TestRecords");
            DropTable("dbo.TestNotifications");
            DropTable("dbo.Templates");
            DropTable("dbo.SymbolLibraries");
            DropTable("dbo.SubNavigations");
            DropTable("dbo.QuestionWheels");
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionMasters");
            DropTable("dbo.Procedures");
            DropTable("dbo.Permissions");
            DropTable("dbo.PermissionMatrices");
            DropTable("dbo.PaymentCardTypes");
            DropTable("dbo.PolicyServices");
            DropTable("dbo.PackageDetails");
            DropTable("dbo.Notifications");
            DropTable("dbo.Navigations");
            DropTable("dbo.MYOBSettings");
            DropTable("dbo.AppUsers");
            DropTable("dbo.MessageSettings");
            DropTable("dbo.LogoSettings");
            DropTable("dbo.LicenseServices");
            DropTable("dbo.Roles");
            DropTable("dbo.LicenseRoles");
            DropTable("dbo.Employees");
            DropTable("dbo.Packages");
            DropTable("dbo.CustomerPolicyDatails");
            DropTable("dbo.CustomerLSQMasters");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerLSQAnswers");
            DropTable("dbo.CustomerRegistrations");
            DropTable("dbo.CustomerFHistrories");
            DropTable("dbo.Configurations");
            DropTable("dbo.PaymentModes");
            DropTable("dbo.LicenseSubscriptions");
            DropTable("dbo.Licenses");
            DropTable("dbo.CompanyPackageDetails");
            DropTable("dbo.Industries");
            DropTable("dbo.CompanyGeneralDetails");
            DropTable("dbo.Companies");
            DropTable("dbo.Countries");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.Branches");
            DropTable("dbo.Authors");
            DropTable("dbo.Agents");
            DropTable("dbo.AgentEmpDetails");
            DropTable("dbo.AgentScoreQuestions");
            DropTable("dbo.AgentRegistrations");
            DropTable("dbo.AgentAnswers");
            DropTable("dbo.UserDesignations");
            DropTable("dbo.QueryTypes");
            DropTable("dbo.GeneralQueryUpdates");
            DropTable("dbo.GeneralQueries");
            DropTable("dbo.FileLibraries");
            DropTable("dbo.ActSettings");
        }
    }
}
