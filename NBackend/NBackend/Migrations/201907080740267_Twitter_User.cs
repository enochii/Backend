namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Twitter_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("TBACKEND.Users", "role", c => c.String(maxLength: 20));
            AddColumn("TBACKEND.Twitters", "image", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("TBACKEND.Twitters", "image");
            DropColumn("TBACKEND.Users", "role");
        }
    }
}
