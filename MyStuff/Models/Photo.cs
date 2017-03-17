using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyStuff.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }

        [Display(Name = "Upload File Name")]
        public string UploadFileName { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Description")]
        [Required]
        public String Description { get; set; }

        [Display(Name = "Image Path")]
        public String ImagePath { get; set; }

        [Display(Name = "Thumbnail Path")]
        public String ThumbnailPath { get; set; }

        [Display(Name = "Date Photo Taken")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTaken { get; set; }

        [Display(Name = "Taken By")]
        public String TakenBy { get; set; }

        [Display(Name = "Date Photo Uploaded")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateUploaded { get; set; }

        [Display(Name = "Uploaded By")]
        public String UploadedBy { get; set; }

        public ICollection<PhotoAlbum> PhotoAlbums { get; set; }
    }

}