namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_Fix_groups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Users", new[] { "Group_Id" });
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
            DropColumn("dbo.Users", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Group_Id", c => c.Int());
            DropForeignKey("dbo.UserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropTable("dbo.UserGroups");
            CreateIndex("dbo.Users", "Group_Id");
            AddForeignKey("dbo.Users", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
