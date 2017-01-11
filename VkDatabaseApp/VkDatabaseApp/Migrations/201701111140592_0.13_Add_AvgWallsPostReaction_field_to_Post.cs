namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _013_Add_AvgWallsPostReaction_field_to_Post : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "AvgWallsPostReaction", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "AvgWallsPostReaction");
        }
    }
}
