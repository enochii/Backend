namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class warelen : DbMigration
    {
        public override void Up()
        {
            AlterColumn("TBACKEND.CourseWares", "location", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("TBACKEND.CourseWares", "location", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
