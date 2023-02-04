using System.ComponentModel.DataAnnotations;

namespace OttBlog23.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string? BlogUserId { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} of the tag must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Tag Text: ")]
        public string? Text { get; set; }

        //Navigation Properties
        public virtual Post? Post { get; set; }
        [Display(Name = "Author: ")]
        public virtual BlogUser? BlogUser { get; set; }
    }
}
