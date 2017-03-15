namespace MyStuff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.AlbumId);
            
            CreateTable(
                "dbo.Photo",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        UploadFileName = c.String(),
                        FileName = c.String(),
                        Description = c.String(nullable: false),
                        ImagePath = c.String(),
                        DateTaken = c.DateTime(nullable: false),
                        TakenBy = c.String(),
                        DateUploaded = c.DateTime(nullable: false),
                        UploadedBy = c.String(),
                    })
                .PrimaryKey(t => t.PhotoId);
            
            CreateTable(
                "dbo.PhotoAlbums",
                c => new
                    {
                        AlbumId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AlbumId, t.PhotoId })
                .ForeignKey("dbo.Album", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Photo", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.PhotoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhotoAlbums", "PhotoId", "dbo.Photo");
            DropForeignKey("dbo.PhotoAlbums", "AlbumId", "dbo.Album");
            DropIndex("dbo.PhotoAlbums", new[] { "PhotoId" });
            DropIndex("dbo.PhotoAlbums", new[] { "AlbumId" });
            DropTable("dbo.PhotoAlbums");
            DropTable("dbo.Photo");
            DropTable("dbo.Album");
        }
    }
}
