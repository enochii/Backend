namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments : DbMigration
    {
        public override void Up()
        {
            AddColumn("TBACKEND.Discussions", "Discussion_DisscussionId", c => c.Decimal(precision: 10, scale: 0));
            CreateIndex("TBACKEND.Discussions", "Discussion_DisscussionId");
            AddForeignKey("TBACKEND.Discussions", "Discussion_DisscussionId", "TBACKEND.Discussions", "DisscussionId");
        }
        
        public override void Down()
        {
            DropForeignKey("TBACKEND.Discussions", "Discussion_DisscussionId", "TBACKEND.Discussions");
            DropIndex("TBACKEND.Discussions", new[] { "Discussion_DisscussionId" });
            DropColumn("TBACKEND.Discussions", "Discussion_DisscussionId");
        }
    }
}
