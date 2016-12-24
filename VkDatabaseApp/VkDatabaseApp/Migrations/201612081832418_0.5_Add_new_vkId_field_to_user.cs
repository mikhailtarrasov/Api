namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05_Add_new_vkId_field_to_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "VkId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "VkId");
        }
    }
}
