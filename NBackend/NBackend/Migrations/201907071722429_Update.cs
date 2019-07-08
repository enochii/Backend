namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TBACKEND.Attentions",
                c => new
                    {
                        StudentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(nullable: false, maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        timeId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        status = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => new { t.StudentId, t.secId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year }, cascadeDelete: true)
                .ForeignKey("TBACKEND.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.Broadcasts",
                c => new
                    {
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BroadcastId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        type = c.Decimal(nullable: false, precision: 10, scale: 0),
                        content = c.String(nullable: false, maxLength: 200),
                        scope = c.Decimal(nullable: false, precision: 10, scale: 0),
                        publish_time = c.String(nullable: false, maxLength: 20),
                        start_time = c.String(nullable: false, maxLength: 20),
                        end_time = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.BroadcastId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.CourseWares",
                c => new
                    {
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CourseWareId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        name = c.String(nullable: false, maxLength: 40),
                        location = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CourseWareId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.Disscussions",
                c => new
                    {
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DisscussionId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        userId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        content = c.String(nullable: false, maxLength: 400),
                        time = c.String(nullable: false, maxLength: 20),
                        Disscussion_DisscussionId = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.DisscussionId)
                .ForeignKey("TBACKEND.Disscussions", t => t.Disscussion_DisscussionId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Users", t => t.userId, cascadeDelete: true)
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId)
                .Index(t => t.userId)
                .Index(t => t.Disscussion_DisscussionId);
            
            CreateTable(
                "TBACKEND.ExamQuestions",
                c => new
                    {
                        examId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        questionId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        index = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.examId, t.questionId })
                .ForeignKey("TBACKEND.Exams", t => t.examId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Questions", t => t.questionId, cascadeDelete: true)
                .Index(t => t.examId)
                .Index(t => t.questionId);
            
            CreateTable(
                "TBACKEND.Exams",
                c => new
                    {
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ExamId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        scope = c.String(nullable: false, maxLength: 100),
                        title = c.String(nullable: false, maxLength: 50),
                        type = c.Decimal(nullable: false, precision: 10, scale: 0),
                        start_time = c.String(nullable: false, maxLength: 20),
                        end_time = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ExamId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.Questions",
                c => new
                    {
                        QuestionId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        chapter = c.String(nullable: false),
                        content = c.String(nullable: false, maxLength: 100),
                        options = c.String(nullable: false, maxLength: 200),
                        answer = c.String(nullable: false, maxLength: 20),
                        single_score = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.Takes",
                c => new
                    {
                        StudentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(nullable: false, maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        score = c.Decimal(nullable: false, precision: 10, scale: 0),
                        validate_status = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => new { t.StudentId, t.secId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year }, cascadeDelete: true)
                .ForeignKey("TBACKEND.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.TakesExams",
                c => new
                    {
                        StudentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ExamId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.ExamId })
                .ForeignKey("TBACKEND.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.ExamId);
            
            CreateTable(
                "TBACKEND.TeacherBroadcasts",
                c => new
                    {
                        teacherId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        broadcastId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.teacherId, t.broadcastId })
                .ForeignKey("TBACKEND.Broadcasts", t => t.broadcastId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Teachers", t => t.teacherId, cascadeDelete: true)
                .Index(t => t.teacherId)
                .Index(t => t.broadcastId);
            
            CreateTable(
                "TBACKEND.Teams",
                c => new
                    {
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TeamId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        team_name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId);
            
            CreateTable(
                "TBACKEND.TeamStudents",
                c => new
                    {
                        teamId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        studentId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.teamId, t.studentId })
                .ForeignKey("TBACKEND.Students", t => t.studentId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Teams", t => t.teamId, cascadeDelete: true)
                .Index(t => t.teamId)
                .Index(t => t.studentId);
            
            CreateTable(
                "TBACKEND.Twitters",
                c => new
                    {
                        TwitterId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        userId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        content = c.String(nullable: false, maxLength: 400),
                        time = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.TwitterId)
                .ForeignKey("TBACKEND.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
            CreateTable(
                "TBACKEND.UserUsers",
                c => new
                    {
                        User_Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        User_Id1 = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.User_Id, t.User_Id1 })
                .ForeignKey("TBACKEND.Users", t => t.User_Id)
                .ForeignKey("TBACKEND.Users", t => t.User_Id1)
                .Index(t => t.User_Id)
                .Index(t => t.User_Id1);
            
            DropColumn("TBACKEND.Teaches", "grade");
        }
        
        public override void Down()
        {
            AddColumn("TBACKEND.Teaches", "grade", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropForeignKey("TBACKEND.Twitters", "userId", "TBACKEND.Users");
            DropForeignKey("TBACKEND.TeamStudents", "teamId", "TBACKEND.Teams");
            DropForeignKey("TBACKEND.TeamStudents", "studentId", "TBACKEND.Students");
            DropForeignKey("TBACKEND.Teams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Teams", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.TeacherBroadcasts", "teacherId", "TBACKEND.Teachers");
            DropForeignKey("TBACKEND.TeacherBroadcasts", "broadcastId", "TBACKEND.Broadcasts");
            DropForeignKey("TBACKEND.TakesExams", "StudentId", "TBACKEND.Students");
            DropForeignKey("TBACKEND.TakesExams", "ExamId", "TBACKEND.Exams");
            DropForeignKey("TBACKEND.Takes", "StudentId", "TBACKEND.Students");
            DropForeignKey("TBACKEND.Takes", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Takes", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.ExamQuestions", "questionId", "TBACKEND.Questions");
            DropForeignKey("TBACKEND.Questions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.ExamQuestions", "examId", "TBACKEND.Exams");
            DropForeignKey("TBACKEND.Exams", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Exams", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", "userId", "TBACKEND.Users");
            DropForeignKey("TBACKEND.UserUsers", "User_Id1", "TBACKEND.Users");
            DropForeignKey("TBACKEND.UserUsers", "User_Id", "TBACKEND.Users");
            DropForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions");
            DropForeignKey("TBACKEND.CourseWares", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.CourseWares", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Broadcasts", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Broadcasts", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Attentions", "StudentId", "TBACKEND.Students");
            DropForeignKey("TBACKEND.Attentions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Attentions", "courseId", "TBACKEND.Courses");
            DropIndex("TBACKEND.UserUsers", new[] { "User_Id1" });
            DropIndex("TBACKEND.UserUsers", new[] { "User_Id" });
            DropIndex("TBACKEND.Twitters", new[] { "userId" });
            DropIndex("TBACKEND.TeamStudents", new[] { "studentId" });
            DropIndex("TBACKEND.TeamStudents", new[] { "teamId" });
            DropIndex("TBACKEND.Teams", new[] { "courseId" });
            DropIndex("TBACKEND.Teams", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.TeacherBroadcasts", new[] { "broadcastId" });
            DropIndex("TBACKEND.TeacherBroadcasts", new[] { "teacherId" });
            DropIndex("TBACKEND.TakesExams", new[] { "ExamId" });
            DropIndex("TBACKEND.TakesExams", new[] { "StudentId" });
            DropIndex("TBACKEND.Takes", new[] { "courseId" });
            DropIndex("TBACKEND.Takes", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.Takes", new[] { "StudentId" });
            DropIndex("TBACKEND.Questions", new[] { "courseId" });
            DropIndex("TBACKEND.Exams", new[] { "courseId" });
            DropIndex("TBACKEND.Exams", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.ExamQuestions", new[] { "questionId" });
            DropIndex("TBACKEND.ExamQuestions", new[] { "examId" });
            DropIndex("TBACKEND.Disscussions", new[] { "Disscussion_DisscussionId" });
            DropIndex("TBACKEND.Disscussions", new[] { "userId" });
            DropIndex("TBACKEND.Disscussions", new[] { "courseId" });
            DropIndex("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.CourseWares", new[] { "courseId" });
            DropIndex("TBACKEND.CourseWares", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.Broadcasts", new[] { "courseId" });
            DropIndex("TBACKEND.Broadcasts", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.Attentions", new[] { "courseId" });
            DropIndex("TBACKEND.Attentions", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.Attentions", new[] { "StudentId" });
            DropTable("TBACKEND.UserUsers");
            DropTable("TBACKEND.Twitters");
            DropTable("TBACKEND.TeamStudents");
            DropTable("TBACKEND.Teams");
            DropTable("TBACKEND.TeacherBroadcasts");
            DropTable("TBACKEND.TakesExams");
            DropTable("TBACKEND.Takes");
            DropTable("TBACKEND.Questions");
            DropTable("TBACKEND.Exams");
            DropTable("TBACKEND.ExamQuestions");
            DropTable("TBACKEND.Disscussions");
            DropTable("TBACKEND.CourseWares");
            DropTable("TBACKEND.Broadcasts");
            DropTable("TBACKEND.Attentions");
        }
    }
}
