namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Discussion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TBACKEND.Discussions",
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
                    })
                .PrimaryKey(t => t.DisscussionId)
                .ForeignKey("TBACKEND.Courses", t => t.courseId, cascadeDelete: true)
                .ForeignKey("TBACKEND.Sections", t => new { t.secId, t.courseId, t.semester, t.year })
                .ForeignKey("TBACKEND.Users", t => t.userId, cascadeDelete: true)
                .Index(t => new { t.secId, t.courseId, t.semester, t.year })
                .Index(t => t.courseId)
                .Index(t => t.userId);
            
            AddColumn("TBACKEND.Disscussions", "Discussion_DisscussionId", c => c.Decimal(precision: 10, scale: 0));
            CreateIndex("TBACKEND.Disscussions", "Discussion_DisscussionId");
            AddForeignKey("TBACKEND.Disscussions", "Discussion_DisscussionId", "TBACKEND.Discussions", "DisscussionId");
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.Discussions", "userId", "TBACKEND.Users");
            DropForeignKey("TBACKEND.Discussions", new[] { "secId", "courseId", "semester", "year" }, "TBACKEND.Sections");
            DropForeignKey("TBACKEND.Discussions", "courseId", "TBACKEND.Courses");
            DropForeignKey("TBACKEND.Disscussions", "Discussion_DisscussionId", "TBACKEND.Discussions");
            DropIndex("TBACKEND.Disscussions", new[] { "Discussion_DisscussionId" });
            DropIndex("TBACKEND.Discussions", new[] { "userId" });
            DropIndex("TBACKEND.Discussions", new[] { "courseId" });
            DropIndex("TBACKEND.Discussions", new[] { "secId", "courseId", "semester", "year" });
            DropColumn("TBACKEND.Disscussions", "Discussion_DisscussionId");
            DropTable("TBACKEND.Discussions");
        }
    }
}
