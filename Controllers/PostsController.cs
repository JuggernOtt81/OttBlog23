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
using OttBlog23.Models;
using OttBlog23.Services;
using OttBlog23.Services.Interfaces;

namespace OttBlog23.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly ISlugService _slugService;

        public PostsController(ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager, ISlugService slugService)
        {
            _slugService = slugService;
            _userManager = userManager;
            _imageService = imageService;
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog).Include(p => p.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _context.Posts
            .Include(p => p.Blog)
            .Include(p => p.BlogUser)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(m => m.Slug == slug);
            //ViewData["PostId"] = post.Id;
            //ViewData["Title"] = post.Title;
            //ViewData["Content"] = post.Content;

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        //BlogPostIndex
        public async Task<IActionResult> BlogPostIndex(int? id)
        {
            if(id is null)
            {
                return NotFound();
            }

            var posts = _context.Posts.Where(p => p.BlogId == id).ToList();

            return View("Index", posts);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
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
                    if(newSlug != newPost.Slug)
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
