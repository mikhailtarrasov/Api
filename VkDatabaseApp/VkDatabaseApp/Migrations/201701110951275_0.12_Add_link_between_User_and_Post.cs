namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _012_Add_link_between_User_and_Post : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "User_VkId", c => c.Int());
            CreateIndex("dbo.Posts", "User_VkId");
            AddForeignKey("dbo.Posts", "User_VkId", "dbo.Users", "VkId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "User_VkId", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "User_VkId" });
            DropColumn("dbo.Posts", "User_VkId");
        }
    }
}
