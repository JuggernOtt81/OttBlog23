using OttBlog23.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OttBlog23.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Display(Name = "Blog Name")]
        public int BlogId { get; set; }        
        public string? BlogUserId { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} of the post must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Post Title: ")]
        public string? Title { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} of the post must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Post Abstract: ")]
        public string? Abstract { get; set; }

        [Required]
        [Display(Name = "Post Content: ")]
        public string? Content { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created on: ")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated on: ")]
        public DateTime? Updated { get; set; }

        public ReadyStatus ReadyStatus { get; set; }

        public string? Slug { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ContentType { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        //Navigation Properties
        public virtual Blog? Blog { get; set; }
        [Display(Name = "Author: ")]        
        public virtual BlogUser? BlogUser { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
