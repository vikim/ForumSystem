namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using ForumSystem.Web.InputModels.Questions;

    public class QuestionsController : Controller
    {
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
            return Content("GET");
        }

        [HttpPost]
        public ActionResult Ask(AskInputModel input)
        {
            return Content("POST");
        }
    }
}