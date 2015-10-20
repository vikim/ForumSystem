﻿namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using ForumSystem.Web.InputModels.Questions;
    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;

    public class QuestionsController : Controller
    {
        private readonly IDeletableEntityRepository<Post> posts;

        public QuestionsController(IDeletableEntityRepository<Post> posts)
        {
            this.posts = posts;
        }

        // GET: Questions by question id
        // questions/3/proba-url
        public ActionResult Display(int id, string url, int page = 1)
        {
            return Content(id + " " + url);
        }

        // questions/tagged/tag-string
        public ActionResult GetByTag(string tag)
        {
            return Content(tag);
        }

        // GET-POST-REDIRECT
        // questions/ask
        [HttpGet]
        public ActionResult Ask()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ask(AskInputModel input)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = input.Title,
                    Content = input.Content
                };

                this.posts.Add(post);
                this.posts.SaveChanges();

                return this.RedirectToAction("Display", new { id = post.Id, url = "new" });
            }

            return View(input);
        }
    }
}