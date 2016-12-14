namespace MyStuff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUploadFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "UploadFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "UploadFileName");
        }
    }
}
