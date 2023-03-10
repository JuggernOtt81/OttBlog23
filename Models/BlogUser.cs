using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OttBlog23.Models
{
    public class BlogUser : IdentityUser
    {
        //[Required]
        [StringLength(50, ErrorMessage = "The {0} of the blog user must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        //[Required]
        [StringLength(50, ErrorMessage = "The {0} of the blog user must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        //[Required]
        [StringLength(50, ErrorMessage = "The {0} of the blog user must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "UserName / Handle / Moniker / Alias")]
        public string? DisplayName { get; set; }

        //Profile Image
        [Display(Name = "Profile Image: ")]
        public byte[]? ImageData { get; set; }

        [Display(Name = "Image Type: ")]
        public string? ContentType { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        //Social
        [StringLength(100, ErrorMessage = "The {0} of the blog user must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        public string? FacebookUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} of the blog user must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        public string? TwitterUrl { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        //Navigation Properties
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
