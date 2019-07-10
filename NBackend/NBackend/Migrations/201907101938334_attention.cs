namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attention : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("TBACKEND.Attentions");
            AddPrimaryKey("TBACKEND.Attentions", new[] { "StudentId", "secId", "courseId", "semester", "year", "timeId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("TBACKEND.Attentions");
            AddPrimaryKey("TBACKEND.Attentions", new[] { "StudentId", "secId", "courseId", "semester", "year" });
        }
    }
}
