using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webblog.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace webblog.Controllers
{
    [RequireHttps]
    public class blogpostsController : ApplicationBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: blogposts
        public ActionResult Index(int? page, string query)
        {
            int pageSize = 5; // # of posts to show per page
            int pageNumber = (page ?? 1);
            var qposts = db.Posts.Where(p => p.Published == true).AsQueryable();
            ViewBag.query = query;
            if(!string.IsNullOrWhiteSpace(query))
            {
                qposts = qposts.Where(p => p.Title.Contains(query) || p.Body.Contains(query) || p.Category.Contains(query) ||
                p.Comments.Any(c => c.Body.Contains(query) || c.Author.DisplayName.Contains(query)));
            }
            var posts = qposts.ToList().OrderByDescending(x => x.Created).ToPagedList(pageNumber,pageSize);

            //getting a list of category/tag names from all of the blog posts
            List<string> categoryList = new List<string>();
            foreach (var i in db.Posts)
            {
                    categoryList.Add(i.Category);
            }
            //[Distinct] removes duplicate entires in the list
            ViewBag.categorylist = categoryList.Distinct();
            return View(posts);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            var posts = db.Posts.ToList();
            return View(posts);
        }

        // GET: blogposts/Details/5
        public ActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blogpost blogpost = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        // GET: blogposts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: blogposts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaURL,Published,Category")] blogpost blogpost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                //restricting valid file formats to images only
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/img/blog/"), fileName));
                    blogpost.MediaURL = "~/img/blog/" + fileName;
                }
                var Slug = StringUtilities.URLFriendly(blogpost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title.");
                    return View(blogpost);
                }
                if (db.Posts.Any(p=>p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique.");
                    return View(blogpost);
                }
                var defaultMedia = "http://placehold.it/900x300";
                if (String.IsNullOrWhiteSpace(blogpost.MediaURL))
                {
                    blogpost.MediaURL = defaultMedia;
                }

                blogpost.Slug = Slug;
                blogpost.Created = System.DateTimeOffset.Now;
                db.Posts.Add(blogpost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogpost);
        }

        // GET: blogposts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blogpost blogpost = db.Posts.Find(id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        // POST: blogposts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,MediaURL,Published,Category")] blogpost blogpost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                blogpost.Updated = System.DateTimeOffset.Now;
                db.Posts.Attach(blogpost);
                db.Entry(blogpost).Property("Body").IsModified = true;
                db.Entry(blogpost).Property("Title").IsModified = true;
                db.Entry(blogpost).Property("MediaURL").IsModified = true;
                db.Entry(blogpost).Property("Updated").IsModified = true;
                db.Entry(blogpost).Property("Category").IsModified = true;
                db.Entry(blogpost).Property("Published").IsModified = true;

                //restricting valid file formats to images only
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/img/blog/"), fileName));
                    blogpost.MediaURL = "~/img/blog/" + fileName;
                }

                var defaultMedia = "http://placehold.it/900x300";
                if (String.IsNullOrWhiteSpace(blogpost.MediaURL))
                {
                    blogpost.MediaURL = defaultMedia;
                }

                var Slug = StringUtilities.URLFriendly(blogpost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title.");
                    return View(blogpost);
                }
                var SlugAlreadyExists = db.Posts.Where(p => p.Id == blogpost.Id && p.Slug == Slug).Select(p => p.Slug);
                if (!SlugAlreadyExists.Any())
                {
                    if (db.Posts.Any(p => p.Slug == Slug))
                    {
                        ModelState.AddModelError("Title", "The title must be unique.");
                        return View(blogpost);
                    }
                }

                blogpost.Slug = Slug;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogpost);
        }

        // GET: blogposts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blogpost blogpost = db.Posts.Find(id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        // POST: blogposts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            blogpost blogpost = db.Posts.Find(id);
            db.Posts.Remove(blogpost);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateComment(comment comment, string slug)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorId = User.Identity.GetUserId();
                comment.Created = System.DateTimeOffset.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "blogposts", new { slug });
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("DeleteComment")]
        public ActionResult ConfirmComment(int? id, string slug)
        {
            comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "blogposts", new { slug });
        }

        // GET: blogposts/Edit/5
        [Authorize]
        public ActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: blogposts/EditComment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditComment(comment comment, string slug)
        {
            if (ModelState.IsValid)
            {
                comment.Updated = System.DateTimeOffset.Now;
                db.Comments.Attach(comment);
                db.Entry(comment).Property("Body").IsModified = true;
                db.Entry(comment).Property("Updated").IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Details", "blogposts", new { slug });
            }
            return View(comment);
        }

    }
}
