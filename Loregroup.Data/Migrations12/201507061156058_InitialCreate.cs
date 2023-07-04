namespace PWA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
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
            DropForeignKey("dbo.Notifications", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.MYOBSettings", "FileLibraryId", "dbo.FileLibraries");
            DropForeignKey("dbo.MessageSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserDesignationId", "dbo.UserDesignations");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "ImageId", "dbo.FileLibraries");
            DropForeignKey("dbo.Users", "CityId", "dbo.Cities");
            DropForeignKey("dbo.LogoSettings", "FileLibrary_Id", "dbo.FileLibraries");
            DropForeignKey("dbo.LicenseServices", "LicenseId", "dbo.Licenses");
            DropForeignKey("dbo.LicenseRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.LicenseRoles", "LicenseId", "dbo.Licenses");
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
            DropIndex("dbo.Notifications", new[] { "RoleId" });
            DropIndex("dbo.MYOBSettings", new[] { "FileLibraryId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", new[] { "CityId" });
            DropIndex("dbo.Users", new[] { "UserDesignationId" });
            DropIndex("dbo.Users", new[] { "ImageId" });
            DropIndex("dbo.MessageSettings", new[] { "UserId" });
            DropIndex("dbo.LogoSettings", new[] { "FileLibrary_Id" });
            DropIndex("dbo.LicenseServices", new[] { "LicenseId" });
            DropIndex("dbo.LicenseRoles", new[] { "LicenseId" });
            DropIndex("dbo.LicenseRoles", new[] { "RoleId" });
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
            DropIndex("dbo.GeneralQueryUpdates", new[] { "GeneralQueryId" });
            DropIndex("dbo.GeneralQueries", new[] { "QueryTypeId" });
            DropIndex("dbo.GeneralQueries", new[] { "UserDesignationId" });
            DropIndex("dbo.ActSettings", new[] { "FileLibraryId" });
            DropTable("dbo.GeneralQueryUpdateFileLibraries");
            DropTable("dbo.GeneralQueryFileLibraries");
            DropTable("dbo.DashBoradWidgets");
            DropTable("dbo.TranningVideos");
            DropTable("dbo.Templates");
            DropTable("dbo.SymbolLibraries");
            DropTable("dbo.SubNavigations");
            DropTable("dbo.QuestionWheels");
            DropTable("dbo.Permissions");
            DropTable("dbo.PermissionMatrices");
            DropTable("dbo.PaymentCardTypes");
            DropTable("dbo.Notifications");
            DropTable("dbo.Navigations");
            DropTable("dbo.MYOBSettings");
            DropTable("dbo.Users");
            DropTable("dbo.MessageSettings");
            DropTable("dbo.LogoSettings");
            DropTable("dbo.LicenseServices");
            DropTable("dbo.Roles");
            DropTable("dbo.LicenseRoles");
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
            DropTable("dbo.UserDesignations");
            DropTable("dbo.QueryTypes");
            DropTable("dbo.GeneralQueryUpdates");
            DropTable("dbo.GeneralQueries");
            DropTable("dbo.FileLibraries");
            DropTable("dbo.ActSettings");
        }
    }
}
