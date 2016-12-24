namespace VkDatabaseDll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06_None_Generated_UserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserUsers", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            RenameColumn(table: "dbo.UserGroups", name: "User_Id", newName: "User_VkId");
            RenameColumn(table: "dbo.UserUsers", name: "User_Id", newName: "User_VkId");
            RenameColumn(table: "dbo.UserUsers", name: "User_Id1", newName: "User_VkId1");
            RenameIndex(table: "dbo.UserUsers", name: "IX_User_Id", newName: "IX_User_VkId");
            RenameIndex(table: "dbo.UserUsers", name: "IX_User_Id1", newName: "IX_User_VkId1");
            RenameIndex(table: "dbo.UserGroups", name: "IX_User_Id", newName: "IX_User_VkId");
            DropPrimaryKey("dbo.Users");
            AddPrimaryKey("dbo.Users", "VkId");
            AddForeignKey("dbo.UserUsers", "User_VkId", "dbo.Users", "VkId");
            AddForeignKey("dbo.UserUsers", "User_VkId1", "dbo.Users", "VkId");
            AddForeignKey("dbo.UserGroups", "User_VkId", "dbo.Users", "VkId", cascadeDelete: true);
            DropColumn("dbo.Users", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.UserGroups", "User_VkId", "dbo.Users");
            DropForeignKey("dbo.UserUsers", "User_VkId1", "dbo.Users");
            DropForeignKey("dbo.UserUsers", "User_VkId", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AddPrimaryKey("dbo.Users", "Id");
            RenameIndex(table: "dbo.UserGroups", name: "IX_User_VkId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.UserUsers", name: "IX_User_VkId1", newName: "IX_User_Id1");
            RenameIndex(table: "dbo.UserUsers", name: "IX_User_VkId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.UserUsers", name: "User_VkId1", newName: "User_Id1");
            RenameColumn(table: "dbo.UserUsers", name: "User_VkId", newName: "User_Id");
            RenameColumn(table: "dbo.UserGroups", name: "User_VkId", newName: "User_Id");
            AddForeignKey("dbo.UserGroups", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserUsers", "User_Id1", "dbo.Users", "Id");
            AddForeignKey("dbo.UserUsers", "User_Id", "dbo.Users", "Id");
        }
    }
}
