namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class multisec : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("TBACKEND.MultiSectionsTimes", "courseId", "TBACKEND.Courses");
            //DropForeignKey("TBACKEND.MultiSectionsTimes", new[] { "Section_SecId", "Section_courseId", "Section_semester", "Section_year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.MultiSectionsTimes", "section_timeId", "TBACKEND.SectionTimes");
            DropIndex("TBACKEND.MultiSectionsTimes", new[] { "courseId" });
            DropIndex("TBACKEND.MultiSectionsTimes", new[] { "section_timeId" });
            DropIndex("TBACKEND.MultiSectionsTimes", new[] { "Section_SecId", "Section_courseId", "Section_semester", "Section_year" });
            DropTable("TBACKEND.MultiSectionsTimes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => new { t.SecId, t.courseId, t.semester, t.year });
            
            CreateIndex("TBACKEND.MultiSectionsTimes", new[] { "Section_SecId", "Section_courseId", "Section_semester", "Section_year" });
            CreateIndex("TBACKEND.MultiSectionsTimes", "section_timeId");
            CreateIndex("TBACKEND.MultiSectionsTimes", "courseId");
            AddForeignKey("TBACKEND.MultiSectionsTimes", "section_timeId", "TBACKEND.SectionTimes", "SectionTimeId", cascadeDelete: true);
            AddForeignKey("TBACKEND.MultiSectionsTimes", new[] { "Section_SecId", "Section_courseId", "Section_semester", "Section_year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.MultiSectionsTimes", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
