using MyStuff.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyStuff.DAL
{
    public class GalleryContext : DbContext
    {
        public GalleryContext()
            : base("ManageMyStuffContext")
        {
        }

        //public GalleryContext()
        //  : base("MyStuff")
        //{
        //    Database.SetInitializer<GalleryContext>
        //        (new DropCreateDatabaseIfModelChanges<GalleryContext>());
        //}

        public DbSet<Photo> Photos { get; set; }

        public DbSet<PhotoAlbum> PhotoAlbums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PhotoAlbum>().ToTable("Album");
            modelBuilder.Entity<Photo>().ToTable("Photo");

            modelBuilder.Entity<PhotoAlbum>()
                .HasMany(p => p.Photos)
                .WithMany(a => a.PhotoAlbums)
                .Map(m =>
                {
                    m.ToTable("PhotoAlbums");
                    m.MapLeftKey("AlbumId");
                    m.MapRightKey("PhotoId");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}