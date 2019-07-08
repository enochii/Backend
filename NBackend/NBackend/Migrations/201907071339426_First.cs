namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TBACKEND.Courses",
                c => new
                    {
                        CourseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        course_name = c.String(nullable: false, maxLength: 30),
                        credits = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "TBACKEND.Sections",
                c => new
                    {
                        SecId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(nullable: false, maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        building = c.String(nullable: false, maxLength: 20),
                        room_numer = c.String(nullable: false, maxLength: 20),
                        section_timeId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.SecId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.SectionTimes", t => t.section_timeId, cascadeDelete: true)
                .Index(t => t.courseId)
                .Index(t => t.section_timeId);
            
            CreateTable(
                "TBACKEND.SectionTimes",
                c => new
                    {
                        SectionTimeId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        day = c.String(nullable: false, maxLength: 20),
                        start_section = c.Decimal(nullable: false, precision: 10, scale: 0),
                        length = c.Decimal(nullable: false, precision: 10, scale: 0),
                        start_week = c.Decimal(nullable: false, precision: 10, scale: 0),
                        end_week = c.Decimal(nullable: false, precision: 10, scale: 0),
                        single_or_double = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.SectionTimeId);
            
            CreateTable(
                "TBACKEND.Teaches",
                c => new
                    {
                        TeacherId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SecId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(nullable: false, maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        grade = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.TeacherId, t.SecId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.SecId, t.courseId, t.semester, t.year }, cascadeDelete: true)
                .ForeignKey("TBACKEND.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => new { t.SecId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.Teaches", "TeacherId", "TBACKEND.Teachers");
            DropForeignKey("TBACKEND.Teaches", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Teaches", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Sections", "section_timeId", "TBACKEND.SectionTimes");
            DropForeignKey("TBACKEND.Sections", "courseId", "TBACKEND.Courses");
            DropIndex("TBACKEND.Teaches", new[] { "courseId" });
            DropIndex("TBACKEND.Teaches", new[] { "SecId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.Teaches", new[] { "TeacherId" });
            DropIndex("TBACKEND.Sections", new[] { "section_timeId" });
            DropIndex("TBACKEND.Sections", new[] { "courseId" });
            DropTable("TBACKEND.Teaches");
            DropTable("TBACKEND.SectionTimes");
            DropTable("TBACKEND.Sections");
            DropTable("TBACKEND.Courses");
        }
    }
}
