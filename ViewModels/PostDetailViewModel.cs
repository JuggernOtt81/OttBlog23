using OttBlog23.Models;

namespace OttBlog23.ViewModels;

public class PostDetailViewModel
{
    public Post? Post { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}

