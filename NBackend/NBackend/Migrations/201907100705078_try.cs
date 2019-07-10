namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _try : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("TBACKEND.Attentions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Broadcasts", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.CourseWares", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Exams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.MultiSectionTimes", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Takes", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Teaches", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Teams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropPrimaryKey("TBACKEND.Sections");
            AlterColumn("TBACKEND.Sections", "SecId", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AlterColumn("TBACKEND.Users", "department", c => c.String(maxLength: 20));
            AlterColumn("TBACKEND.Users", "mail", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("TBACKEND.Users", "avatar", c => c.String(maxLength: 100));
            AddPrimaryKey("TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Attentions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.Broadcasts", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.CourseWares", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Exams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.MultiSectionTimes", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.Takes", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.Teaches", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.Teams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.Teams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Teaches", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Takes", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.MultiSectionTimes", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Exams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.CourseWares", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Broadcasts", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Attentions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropPrimaryKey("TBACKEND.Sections");
            AlterColumn("TBACKEND.Users", "avatar", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("TBACKEND.Users", "mail", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("TBACKEND.Users", "department", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("TBACKEND.Sections", "SecId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddPrimaryKey("TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Teams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Teaches", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.Takes", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.MultiSectionTimes", new[] { "SecId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
            AddForeignKey("TBACKEND.Exams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.CourseWares", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Broadcasts", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Attentions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" }, cascadeDelete: true);
        }
    }
}
