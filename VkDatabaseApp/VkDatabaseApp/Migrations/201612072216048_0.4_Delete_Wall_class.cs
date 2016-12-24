namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_Delete_Wall_class : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Walls", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.Posts", "Wall_Id", "dbo.Walls");
            DropIndex("dbo.Posts", new[] { "Wall_Id" });
            DropIndex("dbo.Walls", new[] { "Owner_Id" });
            DropColumn("dbo.Posts", "Wall_Id");
            DropTable("dbo.Walls");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Walls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Posts", "Wall_Id", c => c.Int());
            CreateIndex("dbo.Walls", "Owner_Id");
            CreateIndex("dbo.Posts", "Wall_Id");
            AddForeignKey("dbo.Posts", "Wall_Id", "dbo.Walls", "Id");
            AddForeignKey("dbo.Walls", "Owner_Id", "dbo.Users", "Id");
        }
    }
}
