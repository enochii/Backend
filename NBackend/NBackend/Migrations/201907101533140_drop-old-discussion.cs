namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropolddiscussion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions");
            DropForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Disscussions", "userId", "TBACKEND.Users");
            DropForeignKey("TBACKEND.Disscussions", "Discussion_DisscussionId", "TBACKEND.Discussions");
            DropIndex("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" });
            DropIndex("TBACKEND.Disscussions", new[] { "courseId" });
            DropIndex("TBACKEND.Disscussions", new[] { "userId" });
            DropIndex("TBACKEND.Disscussions", new[] { "Disscussion_DisscussionId" });
            DropIndex("TBACKEND.Disscussions", new[] { "Discussion_DisscussionId" });
            DropTable("TBACKEND.Disscussions");
        }
        
        public override void Down()
        {
            CreateTable(
                "TBACKEND.Disscussions",
                c => new
                    {
                        secId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        courseId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        semester = c.String(maxLength: 20),
                        year = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DisscussionId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        userId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        content = c.String(nullable: false, maxLength: 400),
                        time = c.String(nullable: false, maxLength: 20),
                        is_comment = c.Decimal(nullable: false, precision: 1, scale: 0),
                        Disscussion_DisscussionId = c.Decimal(precision: 10, scale: 0),
                        Discussion_DisscussionId = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.DisscussionId);
            
            CreateIndex("TBACKEND.Disscussions", "Discussion_DisscussionId");
            CreateIndex("TBACKEND.Disscussions", "Disscussion_DisscussionId");
            CreateIndex("TBACKEND.Disscussions", "userId");
            CreateIndex("TBACKEND.Disscussions", "courseId");
            CreateIndex("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Disscussions", "Discussion_DisscussionId", "TBACKEND.Discussions", "DisscussionId");
            AddForeignKey("TBACKEND.Disscussions", "userId", "TBACKEND.Users", "Id", cascadeDelete: true);
            AddForeignKey("TBACKEND.Disscussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections", new[] { "SecId", "courseId", "semester", "year" });
            AddForeignKey("TBACKEND.Disscussions", "courseId", "TBACKEND.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("TBACKEND.Disscussions", "Disscussion_DisscussionId", "TBACKEND.Disscussions", "DisscussionId");
        }
    }
}
