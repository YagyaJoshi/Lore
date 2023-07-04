namespace PWA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableQuestions : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
        }
    }
}
