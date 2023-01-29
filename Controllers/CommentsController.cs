using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OttBlog23.Data;
using OttBlog23.Models;
//using OttBlog23.ViewModels;

namespace OttBlog23.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: ALL ORIGINAL Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Moderator).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize]
        //GET: MODERATED Comments
        public async Task<IActionResult> ModeratedIndex()
        {
            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Moderator).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize]
        //GET: DELETED Comments
        public async Task<IActionResult> DeletedIndex()
        {
            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Moderator).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize]
        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Moderator)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
        [Authorize]
        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("PostId", "BlogUserId", "Body")] Comment comment)
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                //var comment = new Comment
                //{
                //    PostId = commentViewModel.PostId,
                //    BlogUserId = commentViewModel.BlogUserId,
                //    Body = commentViewModel.Body
                //};
                // Add the new Comment object to the database
                _context.Add(comment);
                await _context.SaveChangesAsync();
                // Redirect to the post details page
                return RedirectToAction(nameof(Index));
            }
            else
            {
                throw new Exception("The ModelState is NOT valid. Make sure all the properties are accounted for.");
            }
            //if (ModelState.IsValid)
            //{
            //    comment.Created = DateTime.Now.ToUniversalTime();
            //    //comment.Updated = DateTime.Now.ToUniversalTime();
            //    //comment.Moderated = DateTime.Now.ToUniversalTime();
            //    //comment.Deleted = DateTime.Now.ToUniversalTime();
            //    comment.Body = comment.Body;
            //    //comment.ModeratedBody = comment.ModeratedBody;
            //    //comment.ModerationType = comment.ModerationType;
            //    //comment.BlogUserId = comment.BlogUserId;
            //    //comment.ModeratorId = comment.ModeratorId;
            //    comment.PostId = comment.PostId;
            //    _context.Add(comment);
            //    _context.SaveChanges();
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", comment.BlogUserId);
            //ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);
            //ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract", comment.PostId);
            //return View(comment);
        }
        [Authorize]
        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", comment.BlogUserId);
            ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,BlogUserId,ModeratorId,Body,Created,Updated,Moderated,Deleted,ModeratedBody,ModerationType")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                comment.Updated = DateTime.Now.ToUniversalTime();
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", comment.BlogUserId);
            ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract", comment.PostId);
            return View(comment);
        }
        [Authorize]
        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Moderator)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
