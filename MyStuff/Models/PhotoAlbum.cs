using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyStuff.Models
{
    public class PhotoAlbum
    {
        [Key]
        public int AlbumId { get; set; }

        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [Display(Name = "Album Description")]
        [Required]
        public String Description { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created By")]
        public String CreatedBy { get; set; }

        public ICollection<Photo> Photos { get; set; }

    }
}