namespace MyStuff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "FileName", c => c.String());
            AddColumn("dbo.Photos", "TakenBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "TakenBy");
            DropColumn("dbo.Photos", "FileName");
        }
    }
}
