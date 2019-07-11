namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changelength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("TBACKEND.Users", "password", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("TBACKEND.Users", "mail", c => c.String(nullable: false, maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("TBACKEND.Users", "mail", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("TBACKEND.Users", "password", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
