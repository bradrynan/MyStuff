namespace MyStuff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Photos", "ThumbPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "ThumbPath", c => c.String());
        }
    }
}
