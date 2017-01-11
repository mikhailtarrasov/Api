namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _011_Delete_link_between_User_and_Post : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Owner_VkId", "dbo.Users");
            DropForeignKey("dbo.Posts", "FromUser_VkId", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "Owner_VkId" });
            DropIndex("dbo.Posts", new[] { "FromUser_VkId" });
            DropColumn("dbo.Posts", "Owner_VkId");
            DropColumn("dbo.Posts", "FromUser_VkId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "FromUser_VkId", c => c.Int());
            AddColumn("dbo.Posts", "Owner_VkId", c => c.Int());
            CreateIndex("dbo.Posts", "FromUser_VkId");
            CreateIndex("dbo.Posts", "Owner_VkId");
            AddForeignKey("dbo.Posts", "FromUser_VkId", "dbo.Users", "VkId");
            AddForeignKey("dbo.Posts", "Owner_VkId", "dbo.Users", "VkId");
        }
    }
}
