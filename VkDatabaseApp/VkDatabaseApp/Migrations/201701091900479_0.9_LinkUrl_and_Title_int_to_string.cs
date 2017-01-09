namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09_LinkUrl_and_Title_int_to_string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Links", "Url", c => c.String());
            AlterColumn("dbo.Links", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Links", "Title", c => c.Int(nullable: false));
            AlterColumn("dbo.Links", "Url", c => c.Int(nullable: false));
        }
    }
}
