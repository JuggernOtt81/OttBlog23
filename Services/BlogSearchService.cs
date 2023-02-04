using OttBlog23.Data;
using OttBlog23.Enums;
using OttBlog23.Models;

namespace OttBlog23.Services
{
    public class BlogSearchService
    {
        private readonly ApplicationDbContext _context;

        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Search(string SearchTerm)
        {
            var posts = _context.Posts.Where(p => p.ReadyStatus == ReadyStatus.ProductionReady).AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                SearchTerm = SearchTerm.ToLower();

                posts = posts.Where(p => p.Title.ToLower().Contains(SearchTerm) ||
                p.Abstract.ToLower().Contains(SearchTerm) ||
                p.Content.ToLower().Contains(SearchTerm) ||
                    p.Comments.Any(c => c.Body.ToLower().Contains(SearchTerm) ||
                        c.ModeratedBody.ToLower().Contains(SearchTerm) ||
                        c.BlogUser.FirstName.ToLower().Contains(SearchTerm) ||
                        c.BlogUser.LastName.ToLower().Contains(SearchTerm) ||
                        c.BlogUser.Email.ToLower().Contains(SearchTerm) ||
                        c.BlogUser.UserName.ToLower().Contains(SearchTerm)));
            }

            posts = posts.OrderByDescending(p => p.Created);
            return posts;
        }
    }
}