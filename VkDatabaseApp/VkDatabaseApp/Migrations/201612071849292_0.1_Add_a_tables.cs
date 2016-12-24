namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_Add_a_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScreenName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Photo = c.String(),
                        User_Id = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        FromId = c.Int(nullable: false),
                        Text = c.String(),
                        CommentsCount = c.Int(nullable: false),
                        LikesCount = c.Int(nullable: false),
                        RepostsCount = c.Int(nullable: false),
                        Wall_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Walls", t => t.Wall_Id)
                .Index(t => t.Wall_Id);
            
            CreateTable(
                "dbo.Walls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Wall_Id", "dbo.Walls");
            DropForeignKey("dbo.Walls", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.Walls", new[] { "Owner_Id" });
            DropIndex("dbo.Posts", new[] { "Wall_Id" });
            DropIndex("dbo.Users", new[] { "Group_Id" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropTable("dbo.Walls");
            DropTable("dbo.Posts");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
        }
    }
}
