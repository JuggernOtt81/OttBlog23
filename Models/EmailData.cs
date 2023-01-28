﻿using System.ComponentModel.DataAnnotations;

namespace OttBlog23.Models
{
    public class EmailData
    {
        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? GroupName { get; set; }
    }
}
