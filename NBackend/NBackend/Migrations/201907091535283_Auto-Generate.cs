namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoGenerate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("TBACKEND.Attentions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Sections", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Broadcasts", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.CourseWares", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Exams", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Questions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.MultiSectionTimes", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Takes", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Teaches", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Teams", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions");
            DropForeignKey("TBACKEND.ExamQuestions", "examId", "TBACKEND.Exams");
            DropForeignKey("TBACKEND.TakesExams", "ExamId", "TBACKEND.Exams");
            DropForeignKey("TBACKEND.MultiSectionTimes", "section_timeId", "TBACKEND.SectionTimes");
            DropForeignKey("TBACKEND.TeamStudents", "teamId", "TBACKEND.Teams");
            DropPrimaryKey("TBACKEND.Courses");
            DropPrimaryKey("TBACKEND.Disscussions");
            DropPrimaryKey("TBACKEND.Exams");
            DropPrimaryKey("TBACKEND.SectionTimes");
            DropPrimaryKey("TBACKEND.Teams");
            AlterColumn("TBACKEND.Attentions", "status", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AlterColumn("TBACKEND.Courses", "CourseId", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AlterColumn("TBACKEND.Disscussions", "DisscussionId", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AlterColumn("TBACKEND.Exams", "ExamId", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AlterColumn("TBACKEND.SectionTimes", "SectionTimeId", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AlterColumn("TBACKEND.Teams", "TeamId", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AddPrimaryKey("TBACKEND.Courses", "CourseId");
            AddPrimaryKey("TBACKEND.Disscussions", "DisscussionId");
            AddPrimaryKey("TBACKEND.Exams", "ExamId");
            AddPrimaryKey("TBACKEND.SectionTimes", "SectionTimeId");
            AddPrimaryKey("TBACKEND.Teams", "TeamId");
            AddForeignKey("TBACKEND.Attentions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Sections", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Broadcasts", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.CourseWares", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Exams", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Questions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.MultiSectionTimes", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Takes", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Teaches", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Teams", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions", "DisscussionId");
            AddForeignKey("TBACKEND.ExamQuestions", "examId", "TBACKEND.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("TBACKEND.TakesExams", "ExamId", "TBACKEND.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("TBACKEND.MultiSectionTimes", "section_timeId", "TBACKEND.SectionTimes", "SectionTimeId", cascadeDelete: true);
            AddForeignKey("TBACKEND.TeamStudents", "teamId", "TBACKEND.Teams", "TeamId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.TeamStudents", "teamId", "TBACKEND.Teams");
            DropForeignKey("TBACKEND.MultiSectionTimes", "section_timeId", "TBACKEND.SectionTimes");
            DropForeignKey("TBACKEND.TakesExams", "ExamId", "TBACKEND.Exams");
            DropForeignKey("TBACKEND.ExamQuestions", "examId", "TBACKEND.Exams");
            DropForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions");
            DropForeignKey("TBACKEND.Teams", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Teaches", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Takes", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.MultiSectionTimes", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Questions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Exams", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.CourseWares", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Broadcasts", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Sections", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Attentions", "courseId", "TBACKEND.Courses");
            DropPrimaryKey("TBACKEND.Teams");
            DropPrimaryKey("TBACKEND.SectionTimes");
            DropPrimaryKey("TBACKEND.Exams");
            DropPrimaryKey("TBACKEND.Disscussions");
            DropPrimaryKey("TBACKEND.Courses");
            AlterColumn("TBACKEND.Teams", "TeamId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AlterColumn("TBACKEND.SectionTimes", "SectionTimeId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AlterColumn("TBACKEND.Exams", "ExamId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AlterColumn("TBACKEND.Disscussions", "DisscussionId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AlterColumn("TBACKEND.Courses", "CourseId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AlterColumn("TBACKEND.Attentions", "status", c => c.Decimal(nullable: false, precision: 1, scale: 0));
            AddPrimaryKey("TBACKEND.Teams", "TeamId");
            AddPrimaryKey("TBACKEND.SectionTimes", "SectionTimeId");
            AddPrimaryKey("TBACKEND.Exams", "ExamId");
            AddPrimaryKey("TBACKEND.Disscussions", "DisscussionId");
            AddPrimaryKey("TBACKEND.Courses", "CourseId");
            AddForeignKey("TBACKEND.TeamStudents", "teamId", "TBACKEND.Teams", "TeamId", cascadeDelete: true);
            AddForeignKey("TBACKEND.MultiSectionTimes", "section_timeId", "TBACKEND.SectionTimes", "SectionTimeId", cascadeDelete: true);
            AddForeignKey("TBACKEND.TakesExams", "ExamId", "TBACKEND.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("TBACKEND.ExamQuestions", "examId", "TBACKEND.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions", "DisscussionId");
            AddForeignKey("TBACKEND.Teams", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Teaches", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Takes", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.MultiSectionTimes", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Questions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Exams", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.CourseWares", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Broadcasts", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Sections", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Attentions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
