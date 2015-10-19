﻿namespace ForumSystem.Web.InputModels.Questions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class AskInputModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [AllowHtml]
        [Display(Name = "Content")]
        //[DataType("tinymce_full")]
        [UIHint("tinymce_full")]
        public string Content { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }
    }
}