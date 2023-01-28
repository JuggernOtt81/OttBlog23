using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OttBlog23.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string? BlogUserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} of the blog must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Blog Name: ")]
        public string? Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} of the blog must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Description: ")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created on: ")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated on: ")]
        public DateTime? Updated { get; set; }

        [Display(Name = "Blog Image: ")]
        public byte[]? ImageData { get; set; }
        [Display(Name = "Image Type: ")]
        public string? ContentType { get; set; }
        [NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile? Image { get; set; }

//When using byte[] and IFormFile for images in an ASP.NET MVC blog project, the optimal size and format for images that users would be uploading and viewing would depend on a few factors such as the purpose of the images, the target audience, and the performance of the website.
//For optimal performance, it is recommended to limit the maximum file size of images to around 2-5MB, as larger file sizes can slow down the upload and download process for users.It is also recommended to resize the images to a reasonable size for displaying on the website, as larger images can slow down the page load times.
//As for the format, JPEG is generally considered a good choice for photographic images as it offers a good balance between image quality and file size.PNG is a good option for graphics and images with transparent backgrounds, as it supports transparency and lossless compression.
//It's worth noting that in some cases, it's better to use a CDN (Content Delivery Network) to store and serve the images, it will offload the burden of serving images from the server and can improve the performance for the users.
//It's also important to consider the accessibility of the images for users with visual impairments, you can add alternative text for images to provide a description of the image for screen readers.

        //Navigation Properties
        [Display(Name = "Author: ")]
        public virtual BlogUser? BlogUser { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
}
}
