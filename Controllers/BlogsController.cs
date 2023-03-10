using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OttBlog23.Data;
using OttBlog23.Models;
using OttBlog23.Services.Interfaces;

namespace OttBlog23.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;

        public BlogsController(ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
            _imageService = imageService;
            _context = context;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Blogs.Include(b => b.BlogUser);

            ViewData["HeaderImage"] = "/img/about-bg.jpg";
            ViewData["MainText"] = "Blogs";
            ViewData["SubText"] = $"Click through to find posts in each topic.";

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }
        [Authorize]
        // GET: Blogs/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["HeaderImage"] = "/img/about-bg.jpg";
            ViewData["MainText"] = "Create Blog";
            ViewData["SubText"] = $"Interesting Topic";
            return View();
        }

        // POST: Blogs/Create
        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Image")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.Created = DateTime.Now.ToUniversalTime();
                blog.BlogUserId = _userManager.GetUserId(User);
                _context.Add(blog);
                blog.ImageData = await _imageService.EncodeImageAsync(blog.Image);
                blog.ContentType = _imageService.ContentType(blog.Image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }
        [Authorize]
        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            ViewData["MainText"] = blog.Name;
            ViewData["SubText"] = blog.Description;
            ViewData["HeaderImage"] = _imageService.DecodeImage(blog.ImageData, blog.ContentType);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Blog blog, IFormFile newImage)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                blog.Updated = DateTime.Now.ToUniversalTime();
                try
                {
                    var newBlog = await _context.Blogs.FindAsync(blog.Id);
                    if (newBlog.Name != blog.Name)
                    {
                        newBlog.Name = blog.Name;
                    };
                    if (newBlog.Description != blog.Description)
                    {
                        newBlog.Description = blog.Description;
                    };
                    newBlog.Updated = DateTime.Now.ToUniversalTime();
                    if (newImage != null)
                    {
                        newBlog.ImageData = await _imageService.EncodeImageAsync(newImage);
                        newBlog.ContentType = _imageService.ContentType(newImage);
                    };

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", blog.BlogUserId);
            return View(blog);
        }

        [Authorize]
        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
