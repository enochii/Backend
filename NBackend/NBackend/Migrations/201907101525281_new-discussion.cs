namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newdiscussion : DbMigration
    {
        public override void Up()
        {
            DropTable("TBACKEND.Discussions");
            //DropTable("TBACKEND.Discussions");
        }
        
        public override void Down()
        {
        }
    }
}
