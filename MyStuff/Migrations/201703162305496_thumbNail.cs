namespace MyStuff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thumbNail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photo", "ThumbnailPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photo", "ThumbnailPath");
        }
    }
}
