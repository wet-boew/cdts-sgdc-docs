﻿using System.Collections.Generic;
using System.Web.Mvc;
using GoC.WebTemplate;

namespace SampleCode.C3.MVC.Controllers
{
    public class GoCWebTemplateApplicationSampleController : WebTemplateBaseController
    {

        public ActionResult StandardPage()
        {
            WebTemplateCore.ApplicationTitle.Text = "Application Name Test";
            WebTemplateCore.Breadcrumbs = null;
            WebTemplateCore.LanguageLink.Href = "apptop-fr.html";
            WebTemplateCore.ShowLanguageLink = true;
            WebTemplateCore.ShowFeatures = false;
            WebTemplateCore.ShowGlobalNav = false;
            WebTemplateCore.ShowPostContent = false;

            WebTemplateCore.SignOutLinkURL = "about:blank";
            WebTemplateCore.ShowSignOutLink = true;


            return View();
        }

    }
}