namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _08_Add_FromUser_Field_To_Post : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "FromUser_VkId", c => c.Int());
            CreateIndex("dbo.Posts", "FromUser_VkId");
            AddForeignKey("dbo.Posts", "FromUser_VkId", "dbo.Users", "VkId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "FromUser_VkId", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "FromUser_VkId" });
            DropColumn("dbo.Posts", "FromUser_VkId");
        }
    }
}
