using System.ComponentModel.DataAnnotations;

namespace OttBlog23.ViewModels
{
    public class ContactMe
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} of the sender must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress (ErrorMessage = "The {0} address must be a VALID email address.")]
        public string? Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? Phone { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        public string? Subject { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters in length.", MinimumLength = 2)]
        public string? Message { get; set; }
    }
}
