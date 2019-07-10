namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discussiontype : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("TBACKEND.Disscussions", "Discussion_DisscussionId", "TBACKEND.Discussions");
            //DropForeignKey("TBACKEND.Discussions", "courseId", "TBACKEND.Courses");
            //DropForeignKey("TBACKEND.Discussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            //DropForeignKey("TBACKEND.Discussions", "userId", "TBACKEND.Users");
            //DropIndex("TBACKEND.Discussions", new[] { "secId", "courseId", "semester", "year" });
            //DropIndex("TBACKEND.Discussions", new[] { "courseId" });
            //DropIndex("TBACKEND.Discussions", new[] { "userId" });
            //DropIndex("TBACKEND.Disscussions", new[] { "Discussion_DisscussionId" });
            //AddColumn("TBACKEND.Disscussions", "is_comment", c => c.Decimal(nullable: false, precision: 1, scale: 0));
            //DropColumn("TBACKEND.Disscussions", "Discussion_DisscussionId");
            DropTable("TBACKEND.Discussions");
        }

        public override void Down()
        {
            //CreateTable(
            //    "TBACKEND.Discussions",
            //    c => new
            //        {
            //            secId = c.Decimal(nullable: false, precision: 10, scale: 0),
            //            courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
            //            semester = c.String(maxLength: 20),
            //            year = c.Decimal(nullable: false, precision: 10, scale: 0),
            //            DisscussionId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
            //            userId = c.Decimal(nullable: false, precision: 10, scale: 0),
            //            content = c.String(nullable: false, maxLength: 400),
            //            time = c.String(nullable: false, maxLength: 20),
            //        })
            //    .PrimaryKey(t => t.DisscussionId);
            
            //AddColumn("TBACKEND.Disscussions", "Discussion_DisscussionId", c => c.Decimal(precision: 10, scale: 0));
            //DropColumn("TBACKEND.Disscussions", "is_comment");
            //CreateIndex("TBACKEND.Disscussions", "Discussion_DisscussionId");
            //CreateIndex("TBACKEND.Discussions", "userId");
            //CreateIndex("TBACKEND.Discussions", "courseId");
            //CreateIndex("TBACKEND.Discussions", new[] { "secId", "courseId", "semester", "year" });
            //AddForeignKey("TBACKEND.Discussions", "userId", "TBACKEND.Users", "Id", cascadeDelete: true);
            //AddForeignKey("TBACKEND.Discussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            //AddForeignKey("TBACKEND.Discussions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            //AddForeignKey("TBACKEND.Disscussions", "Discussion_DisscussionId", "TBACKEND.Discussions", "DisscussionId");
        }
    }
}
