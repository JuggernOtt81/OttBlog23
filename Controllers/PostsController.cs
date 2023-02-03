using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OttBlog23.Data;
using OttBlog23.Enums;
using OttBlog23.Models;
using OttBlog23.Services;
using OttBlog23.Services.Interfaces;
using X.PagedList;
using OttBlog23.ViewModels;

namespace OttBlog23.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly ISlugService _slugService;
        private readonly BlogSearchService _blogSearchService;

        public PostsController(ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager, ISlugService slugService, BlogSearchService blogSearchService)
        {
            _slugService = slugService;
            _userManager = userManager;
            _imageService = imageService;
            _context = context;
            _blogSearchService = blogSearchService;
        }

        //
        public async Task<IActionResult> SearchIndex(int? page, string SearchTerm)
        {
            ViewData["SearchTerm"] = SearchTerm;

            var pageNumber = page ?? 1;
            var pageSize = 3;

            var posts = _blogSearchService.Search(SearchTerm);

            return View(await posts.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Posts
        //getting all the posts and displaying them unattached from the blogs they belong to
        [AllowAnonymous]
        public IActionResult Index(int? page)
        {
            var posts = _context.Posts;

            var pageNumber = page ?? 1;
            var pageSize = 3;

            ViewData["HeaderImage"] = "/img/post-bg.jpg";
            ViewData["MainText"] = "Lawson's Posts";
            ViewData["SubText"] = "All the posts with pagination (blog/topic agnostic)";

            return View(posts.ToPagedList(pageNumber, pageSize));

        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            ViewData["Title"] = "Post Details Page";
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.BlogUser)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.BlogUser)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Moderator)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (post == null)
            {
                return NotFound();
            }

            var dataVM = new PostDetailViewModel()
            {
                Post = post,
                Tags = _context.Tags
                .Select(t => t.Text.ToLower())
                .Distinct().ToList()
            };

            ViewData["HeaderImage"] = _imageService.DecodeImage(post.ImageData, post.ContentType);
            ViewData["MainText"] = post.Title;
            ViewData["SubText"] = post.Abstract;

            return View(dataVM);
        }

        //Blog Post Index
        //per selected blog/topic, these are all the posts under that
       [AllowAnonymous]
        public async Task<IActionResult> BlogPostIndex(int? id, int? page)
        {
            if (id is null) return NotFound();

            var pageNumber = page ?? 1;
            var pageSize = 2;

            var posts = await _context.Posts
                .Where(p => p.BlogId == id && p.ReadyStatus == ReadyStatus.ProductionReady)
                .OrderByDescending(p => p.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            var blog = await _context.Blogs
               .FindAsync(id);


            ViewData["MainText"] = blog.Name;
            ViewData["SubText"] = blog.Description;
            ViewData["HeaderImage"] = _imageService.DecodeImage(blog.ImageData, blog.ContentType);
            
            return View(posts);

        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["HeaderImage"] = "/img/post-bg.jpg";
            ViewData["MainText"] = "Create New Post";
            ViewData["SubText"] = "Make something AWESOME!";
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,ReadyStatus,Image")] Post post, List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now.ToUniversalTime();

                var authorId = _userManager.GetUserId(User);
                post.BlogUserId = authorId;
                post.ImageData = await _imageService.EncodeImageAsync(post.Image);
                post.ContentType = _imageService.ContentType(post.Image);

                #region  ***SLUGS*** (POST/CREATE) [START]

                var slug = _slugService.UrlFriendly(post.Title);

                //slug error
                var validationError = false;


                if (string.IsNullOrEmpty(slug))
                {
                    validationError = true;
                    ModelState.AddModelError("Title", "Invalid title! The title must contain valid characters and meet the minimum length requirements.");
                }

                if (!_slugService.IsUnique(slug))
                {
                    validationError = true;
                    ModelState.AddModelError("Title", "Duplicate slug found! The title's slug conversion must be unique. Please revise the title.");
                }

                if (validationError)
                {
                    ViewData["TagValues"] = string.Join(",", tagValues);
                    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
                    return View(post);
                }

                post.Slug = slug;

                #endregion ***SLUGS*** (POST/CREATE) [END]

                _context.Add(post);

                await _context.SaveChangesAsync();

                foreach (var tagText in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        BlogUserId = authorId,
                        Text = tagText
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
            ViewData["HeaderImage"] = "/img/post-bg.jpg";
            ViewData["MainText"] = "Edit Post";
            ViewData["SubText"] = "Make some changes.";
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,ReadyStatus")] Post post, IFormFile newImage, List<string> tagValues)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newPost = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == post.Id);

                    newPost.Updated = DateTime.Now.ToUniversalTime();
                    newPost.Title = post.Title;
                    newPost.Abstract = post.Abstract;
                    newPost.Content = post.Content;
                    newPost.ReadyStatus = post.ReadyStatus;

                    #region  ***SLUGS*** (POST/EDIT) [START]
                    var newSlug = _slugService.UrlFriendly(post.Title);
                    if (newSlug != newPost.Slug)
                    {
                        if (_slugService.IsUnique(newSlug))
                        {
                            newPost.Title = post.Title;
                            newPost.Slug = newSlug;
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "Duplicate slug found! The title's slug conversion must be unique. Please revise the title.");
                            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
                            ViewData["TagValues"] = string.Join(",", tagValues);
                        }
                    }
                    #endregion ***SLUGS*** (POST/EDIT) [END]

                    if (newImage != null)
                    {
                        newPost.ImageData = await _imageService.EncodeImageAsync(newImage);
                        post.ContentType = _imageService.ContentType(newImage);
                    };

                    //remove all previously attached tags
                    _context.RemoveRange(newPost.Tags);

                    await _context.SaveChangesAsync();

                    foreach (var tagText in tagValues)
                    {
                        _context.Add(new Tag()
                        {
                            PostId = newPost.Id,
                            BlogUserId = newPost.BlogUserId,
                            Text = tagText
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", post.BlogUserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["HeaderImage"] = "/img/post-bg.jpg";
            ViewData["MainText"] = "DELETE POST";
            ViewData["SubText"] = "Was this one not worthy?";
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }





    }
}
