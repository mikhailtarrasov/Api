namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07_Add_PostAttachments_Link_And_Photo : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Posts");
            CreateTable(
                "dbo.PostAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Link_Id = c.Int(),
                        Photo_Id = c.Int(),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Links", t => t.Link_Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Link_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.Int(nullable: false),
                        Title = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PhotoUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Posts", "Owner_VkId", c => c.Int());
            AlterColumn("dbo.Posts", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Posts", "Id");
            CreateIndex("dbo.Posts", "Owner_VkId");
            AddForeignKey("dbo.Posts", "Owner_VkId", "dbo.Users", "VkId");
            DropColumn("dbo.Posts", "OwnerId");
            DropColumn("dbo.Posts", "FromId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "FromId", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "OwnerId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Posts", "Owner_VkId", "dbo.Users");
            DropForeignKey("dbo.PostAttachments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.PostAttachments", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.PostAttachments", "Link_Id", "dbo.Links");
            DropIndex("dbo.PostAttachments", new[] { "Post_Id" });
            DropIndex("dbo.PostAttachments", new[] { "Photo_Id" });
            DropIndex("dbo.PostAttachments", new[] { "Link_Id" });
            DropIndex("dbo.Posts", new[] { "Owner_VkId" });
            DropPrimaryKey("dbo.Posts");
            AlterColumn("dbo.Posts", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Posts", "Owner_VkId");
            DropTable("dbo.Photos");
            DropTable("dbo.Links");
            DropTable("dbo.PostAttachments");
            AddPrimaryKey("dbo.Posts", "Id");
        }
    }
}
