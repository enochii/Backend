namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Course_Teacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("TBACKEND.Courses", "avatar", c => c.String(nullable: false, maxLength: 100));
            AddColumn("TBACKEND.Courses", "description", c => c.String(nullable: false, maxLength: 400));
            AddColumn("TBACKEND.Teachers", "is_manager", c => c.Decimal(nullable: false, precision: 1, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("TBACKEND.Teachers", "is_manager");
            DropColumn("TBACKEND.Courses", "description");
            DropColumn("TBACKEND.Courses", "avatar");
        }
    }
}
