namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaybeFinal : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("TBACKEND.Sections", "section_timeId", "TBACKEND.SectionTimes");
            DropIndex("TBACKEND.Sections", new[] { "section_timeId" });
            AddColumn("TBACKEND.Sections", "start_week", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("TBACKEND.Sections", "end_week", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropColumn("TBACKEND.Sections", "section_timeId");
            DropColumn("TBACKEND.SectionTimes", "day");
            DropColumn("TBACKEND.SectionTimes", "start_week");
            DropColumn("TBACKEND.SectionTimes", "end_week");
            DropColumn("TBACKEND.SectionTimes", "single_or_double");
        }
        
        public override void Down()
        {
            AddColumn("TBACKEND.SectionTimes", "single_or_double", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("TBACKEND.SectionTimes", "end_week", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("TBACKEND.SectionTimes", "start_week", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("TBACKEND.SectionTimes", "day", c => c.String(nullable: false, maxLength: 20));
            AddColumn("TBACKEND.Sections", "section_timeId", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropColumn("TBACKEND.Sections", "end_week");
            DropColumn("TBACKEND.Sections", "start_week");
            CreateIndex("TBACKEND.Sections", "section_timeId");
            AddForeignKey("TBACKEND.Sections", "section_timeId", "TBACKEND.SectionTimes", "SectionTimeId", cascadeDelete: true);
        }
    }
}
