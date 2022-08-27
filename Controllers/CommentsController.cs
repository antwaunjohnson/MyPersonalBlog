#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPersonalBlog.Data;
using MyPersonalBlog.Models;

namespace MyPersonalBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _useManager;

        public CommentsController(ApplicationDbContext context, UserManager<BlogUser> useManager)
        {
            _context = context;
            _useManager = useManager;
        }

        // GET: Comments
        public async Task<IActionResult> OriginalIndex()
        {
            var originalComments = await _context.Comments.ToListAsync();
            return View("Index", originalComments);
        }
        public async Task<IActionResult> ModeratedIndex()
        {
            var moderatedComments = await _context.Comments
                .Where(c => c.Moderated != null)
                .Include(c => c.Post)
                .Include(c => c.Author)
                .ToListAsync();

            ViewData["MainText"] = "Comments";
            ViewData["SubText"] = "All Moderated Comments";

            return View("Index", moderatedComments);
        }
        //public async Task<IActionResult> DeletedIndex()
        //{
        //    var deletedComments = await _context.Comments.Where(c => c.D != null).ToListAsync();
        //    return View("Index", deletedComments);
        //}

        // GET: Comments/Create
        //public IActionResult Create()
        //{
        //    ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["PostId"] = new SelectList(_context.Posts, "PostId", "Abstract");
        //    return View();
        //}

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorId = _useManager.GetUserId(User);
                comment.Created = DateTime.Now;

                _context.Add(comment);
                await _context.SaveChangesAsync();

                comment = await _context.Comments
                    .Include(c => c.Post)
                    .Where(c => c.CommentId == comment.CommentId)
                    .FirstOrDefaultAsync();


                return RedirectToAction("Details", "Posts", new { slug = comment.Post.Slug }, "commentSection");
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Where(c => c.CommentId == id)
                .FirstOrDefaultAsync();
            if (comment == null) return NotFound();

            ViewData["ModerationType"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int commentid, [Bind("CommentId,Body")] Comment comment)
        //{
        //    if (commentid != comment.CommentId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var newComment = await _context.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.CommentId == comment.CommentId);
        //        try
        //        {
        //            newComment.Body = comment.Body;
        //            newComment.Updated = DateTime.Now;

        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CommentExists(comment.CommentId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Details", "Posts", new { slug = newComment.Post.Slug }, "commentSection");
        //    }
            
        //    return View(comment);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Moderate(int commentid, [Bind("CommentId,Body,ModeratedBody,ModertionType")]Comment comment)
        {
            if(commentid != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newComment = await _context.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.CommentId == comment.CommentId);
                try
                {
                    newComment.ModeratedBody = comment.ModeratedBody;
                    newComment.ModerationType = comment.ModerationType;

                    newComment.Moderated = DateTime.Now;
                    newComment.ModeratorId = _useManager.GetUserId(User);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = newComment.Post.Slug }, "commentSection");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Moderator)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id, string slug)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new {slug}, "commentSection");
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
