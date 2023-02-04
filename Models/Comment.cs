using OttBlog23.Enums;
using System.ComponentModel.DataAnnotations;

namespace OttBlog23.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string? BlogUserId { get; set; }
        public string? ModeratorId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} of the comment must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Comment: ")]
        public string? Body { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created on: ")]
        public DateTime? Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated on: ")]
        public DateTime? Updated { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Moderated on: ")]
        public DateTime? Moderated { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Deleted on: ")]
        public DateTime? Deleted { get; set; }

        [StringLength(500, ErrorMessage = "The {0} of the comment must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        [Display(Name = "Moderated Comment")]
        public string? ModeratedBody { get; set; }

        public ModerationType? ModerationType { get; set; }

        //Navigation Properties
        public virtual Post? Post { get; set; }
        [Display(Name = "Author: ")]
        public virtual BlogUser? BlogUser { get; set; }
        public virtual BlogUser? Moderator { get; set; }
    }
}
