namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KULE : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TBACKEND.MultiSectionTimes",
                c => new
                    {
                        SecId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(nullable: false, maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        section_timeId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        day = c.String(nullable: false, maxLength: 20),
                        single_or_double = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.SecId, t.courseId, t.semester, t.year, t.section_timeId })
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.SecId, t.courseId, t.semester, t.year }, cascadeDelete: true)
                .ForeignKey("TBACKEND.SectionTimes", t => t.section_timeId, cascadeDelete: true)
                .Index(t => new { t.SecId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId)
                .Index(t => t.section_timeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.MultiSectionTimes", "section_timeId", "TBACKEND.SectionTimes");
            DropForeignKey("TBACKEND.MultiSectionTimes", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.MultiSectionTimes", "courseId", "TBACKEND.Courses");
            DropIndex("TBACKEND.MultiSectionTimes", new[] { "section_timeId" });
            DropIndex("TBACKEND.MultiSectionTimes", new[] { "courseId" });
            DropIndex("TBACKEND.MultiSectionTimes", new[] { "SecId", "courseId", "semester", "year" });
            DropTable("TBACKEND.MultiSectionTimes");
        }
    }
}
