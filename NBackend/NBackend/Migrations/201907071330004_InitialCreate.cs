namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TBACKEND.Students",
                c => new
                    {
                        StudentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        grade = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "TBACKEND.Teachers",
                c => new
                    {
                        TeacherId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        job_title = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.TeacherId);
            
            CreateTable(
                "TBACKEND.Users",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        user_name = c.String(nullable: false, maxLength: 20),
                        department = c.String(nullable: false, maxLength: 20),
                        password = c.String(nullable: false, maxLength: 20),
                        phone_number = c.String(nullable: false, maxLength: 11),
                        mail = c.String(nullable: false, maxLength: 20),
                        avatar = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("TBACKEND.Users");
            DropTable("TBACKEND.Teachers");
            DropTable("TBACKEND.Students");
        }
    }
}
