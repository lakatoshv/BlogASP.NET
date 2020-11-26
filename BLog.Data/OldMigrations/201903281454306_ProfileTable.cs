namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUser = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ProfileImg = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Profiles");
        }
    }
}
