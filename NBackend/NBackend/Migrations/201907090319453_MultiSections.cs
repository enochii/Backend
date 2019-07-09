namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultiSections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TBACKEND.MultiSectionsTimes",
                c => new
                    {
                        SecId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(nullable: false, maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        section_timeId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        day = c.String(nullable: false, maxLength: 20),
                        single_or_double = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Section_SecId = c.Decimal(precision: 10, scale: 0),
                        Section_courseId = c.Decimal(precision: 10, scale: 0),
                        Section_semester = c.String(maxLength: 20),
                        Section_year = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.SecId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.Section_SecId, t.Section_courseId, t.Section_semester, t.Section_year })
                .ForeignKey("TBACKEND.SectionTimes", t => t.section_timeId, cascadeDelete: true)
                .Index(t => t.courseId)
                .Index(t => t.section_timeId)
                .Index(t => new { t.Section_SecId, t.Section_courseId, t.Section_semester, t.Section_year });
            
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.MultiSectionsTimes", "section_timeId", "TBACKEND.SectionTimes");
            DropForeignKey("TBACKEND.MultiSectionsTimes", new[] { "Section_SecId", "Section_courseId", "Section_semester", "Section_year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.MultiSectionsTimes", "courseId", "TBACKEND.Courses");
            DropIndex("TBACKEND.MultiSectionsTimes", new[] { "Section_SecId", "Section_courseId", "Section_semester", "Section_year" });
            DropIndex("TBACKEND.MultiSectionsTimes", new[] { "section_timeId" });
            DropIndex("TBACKEND.MultiSectionsTimes", new[] { "courseId" });
            DropTable("TBACKEND.MultiSectionsTimes");
        }
    }
}
