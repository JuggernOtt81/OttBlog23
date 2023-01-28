using OttBlog23.ViewModels;

namespace OttBlog23.Services.Interfaces
{
    public interface ISlugService
    {
        string UrlFriendly(string title);
        bool IsUnique(string slug);
    }
}