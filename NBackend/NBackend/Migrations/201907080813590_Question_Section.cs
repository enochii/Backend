namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Question_Section : DbMigration
    {
        public override void Up()
        {
            AddColumn("TBACKEND.Sections", "avatar", c => c.String(nullable: false, maxLength: 100));
            AddColumn("TBACKEND.ExamQuestions", "score", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropColumn("TBACKEND.Questions", "single_score");
        }
        
        public override void Down()
        {
            AddColumn("TBACKEND.Questions", "single_score", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropColumn("TBACKEND.ExamQuestions", "score");
            DropColumn("TBACKEND.Sections", "avatar");
        }
    }
}
