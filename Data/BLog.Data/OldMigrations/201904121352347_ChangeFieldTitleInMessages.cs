namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldTitleInMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Subject", c => c.String());
            DropColumn("dbo.Messages", "Theme");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Theme", c => c.String());
            DropColumn("dbo.Messages", "Subject");
        }
    }
}
