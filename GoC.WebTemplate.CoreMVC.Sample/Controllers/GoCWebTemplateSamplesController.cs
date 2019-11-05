﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GoC.WebTemplate.Components.Core.Services;
using GoC.WebTemplate.Components.Entities;
using GoC.WebTemplate.CoreMVC.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GoC.WebTemplate.CoreMVC.Sample.Controllers
{
    public class GoCWebTemplateSamplesController : WebTemplateBaseController
    {
        public GoCWebTemplateSamplesController(ModelAccessor modelAccessor)
            : base(modelAccessor) { }

        public IActionResult BaseSettingsSample()
        {
            //Page Title
            WebTemplateModel.HeaderTitle = "Basic Settings";

            //Metatags
            WebTemplateModel.HTMLHeaderElements.Add("<meta charset='UTF-8'>");
            WebTemplateModel.HTMLHeaderElements.Add("<meta name='singer' content='Elvis'>");
            WebTemplateModel.HTMLHeaderElements.Add("<meta http-equiv='default-style' content='sample'>");

            //Date Modifiied
            WebTemplateModel.DateModified = new DateTime(2019,10,23);

            //Version Identifier
            WebTemplateModel.VersionIdentifier = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            //Screen Identifier
            WebTemplateModel.ScreenIdentifier = "BASE-SETTING-SAMPLE";

            //Contact Links
            WebTemplateModel.ContactLinks.Add(new Components.Entities.Link { Href = "http://travel.gc.ca/" });

            return View();
        }


        public IActionResult CustomJSandCSSFilesSample()
        {
            //Add a CSS to the header
            WebTemplateModel.HTMLHeaderElements.Add("<link rel='stylesheet' type='text/css' href='/css/mystyle.css'>");

            //Add a JS to the header
            //WebTemplateMaster.HTMLHeaderElements.Add("<script src='/js/myJS.js'></script>");
            //or to add it to the body (bottom of page)
            WebTemplateModel.HTMLBodyElements.Add("<script src='/js/myJS.js'></script>");

            return View();
        }

        public IActionResult BilingualErrorSample()
        {
            return View();
        }

        public IActionResult UnilingualErrorSample()
        {
            return View();
        }

        public IActionResult BreadcrumbSample()
        {
            //Specify your breadcrumbs
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Href = "http://www.canada.ca/en/index.html", Title = "Home" });
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Href = "http://www.esdc.gc.ca/en/jobs/opportunities/index.page", Title = "Jobs" });
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Href = "http://www.esdc.gc.ca/en/jobs/opportunities/youth_students.page", Title = "Opportunities" });
            //Leaving the "href" parameter empty, will create the breadcrumb in text and not as a hyperlink. Useful for the last item of the breadcrumb list. 
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "FSWEP", Acronym = "Federal Student Work Experience Program" });

            //Note: For your solution, the values should be coming from your culture sensitive source ex: resource files, db etc...)
            return View();
        }

        public IActionResult FeedbackandShareThisPageSample()
        {
            //Display the FeedbackLink
            WebTemplateModel.Settings.FeedbackLink.Show = true; //this could be set in the appsettings.json, key = "GoC.WebTemplate.showFeedbackLink"
            WebTemplateModel.Settings.FeedbackLink.Url = "http://www.aircanada.com/en/customercare/customersolutions.html";
            WebTemplateModel.Settings.FeedbackLink.UrlFr = "http://www.aircanada.com/fr/customercare/customersolutions.html"; //will be used if the CurrentUICulture is set to 'fr' / if not set, will assume FeedbackLinkUrl is bilingual


            ////Specify the Share This Page with Media sites.
            WebTemplateModel.Settings.ShowSharePageLink = true; //this could be set in the appsettings.json, key = "GoC.WebTemplate.showSharePageLink"
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.bitly);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.blogger);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.digg);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.diigo);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.email);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.facebook);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.gmail);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.linkedin);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.myspace);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.pinterest);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.reddit);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.tumblr);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.twitter);
            WebTemplateModel.SharePageMediaSites.Add(SocialMediaSites.yahoomail);

            return View();
        }

        public IActionResult FooterLinksSample()
        {
            //Contact Links
            WebTemplateModel.ContactLinks = new List<Link> { new Link { Href = "http://travel.gc.ca/" } };

            //The code snippet below displays an example of multiple links that have text and href being updated. 
            //WebTemplateModel.Settings.Environment = "Prod_SSL";
            //WebTemplateModel.ContactLinks = new List<Link>
            //{
            //    new Link { Href = "http://travel.gc.ca/", Text = "Contact Now" },
            //    new Link { Href = "http://travel.gc.ca/", Text = "Contact Info" }
            //};

            //Footer Sections - Application, GCIntranet
            //WebTemplateModel.Settings.Environment = "Prod_SSL";
            //WebTemplateModel.FooterSections = new List<FooterSection>
            //{
            //    new FooterSection
            //    {
            //        SectionName = "Footer Section 1",
            //        CustomFooterLinks = new List<FooterLink>
            //        {
            //            new FooterLink { Href = "http://travel.gc.ca/", Text = "Link 1" },
            //            new FooterLink { Href = "http://travel.gc.ca/", Text = "Link 2" }
            //        }
            //    }
            //};
            //return View("FooterLinksAppSample");

            //Custom Footer Links - Application, GCWeb
            //WebTemplateModel.CustomFooterLinks = new List<FooterLink>
            //{
            //    new FooterLink { Href = "http://travel.gc.ca/", Text = "Link 1" },
            //    new FooterLink { Href = "http://travel.gc.ca/", Text = "Link 2" }
            //};
            //return View("FooterLinksAppSample");

            //Note: For your solution, the values should be coming from your culture sensitive source ex: resource files, db etc...)
            return View();
        }
        
        public IActionResult LeavingSecureSiteSample()
        {
            //note: other then the message the rest could be set in the web.config
            WebTemplateModel.Settings.LeavingSecureSiteWarning.Enabled = true;
            WebTemplateModel.Settings.LeavingSecureSiteWarning.RedirectUrl = "Redirect";
            WebTemplateModel.Settings.LeavingSecureSiteWarning.ExcludedDomains = "www.esdc.gc.ca, esdc.gc.ca, jobbank.gc.ca";
            WebTemplateModel.Settings.LeavingSecureSiteWarning.Message = "You are leaving a secure session sample text!";

            return View();
        }

        [HttpGet()]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public RedirectResult Redirect(string targetUrl)
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            //add any necessary clean up code (clear session, logout user, etc...)

            //redirect user to link they had clicked
            if (!string.IsNullOrEmpty(targetUrl)) return base.Redirect(targetUrl);

            // decide how you want to handle this situation
            throw new ApplicationException("targetUrl must be specified.");
        }

        public IActionResult LeftMenuSample()
        {
            //set the header for this section of the menu
            //set the links for this section of the menu
            var item = new MenuItem
            {
                Href = "http://www.tsn.ca",
                Text = "TSN",
                SubItems = new List<Link> {
                    new Link { Href="http://www.cbc.ca", Text="sub 1", NewWindow= true },
                    new Link { Href="http://www.rds.ca", Text="sub 2" }
                }
            };

            //add section to template
            WebTemplateModel.LeftMenuItems = new List<MenuSection>
            {
                new MenuSection
                {
                    Text = "Section A",
                    Href = "http://www.servicecanada.gc.ca",
                    NewWindow = true,
                    Items = new List<MenuItem> {
                        item,
                        new MenuItem { Href = "http://www.cnn.ca", Text = "CNN" }
                    }
                }
            };

            //or can be done with a 1 liner
            WebTemplateModel.LeftMenuItems.Add(new MenuSection
            {
                Text = "Section B",
                Href = "http://www.canada.ca",
                Items = new List<MenuItem> {
                    new MenuItem{ Href="http://www.rds.ca", Text="RDS" },
                    new MenuItem{ Href= "http://www.lapresse.com", Text="La Presse"}
                }
            });

            return View();
        }

        public IActionResult TransactionalSample()
        {
            //set the Terms and Condition Link
            WebTemplateModel.TermsConditionsLink = new FooterLink { Href = "http://www.tsn.ca", NewWindow = true };
            //set the Privacy link
            WebTemplateModel.PrivacyLink = new FooterLink { Href = "http://www.lapresse.ca" }; // NewWindow defaults to false

            return View();
        }

        [HttpPost]
#pragma warning disable IDE0060 // Remove unused parameter
        public IActionResult TransactionalSample(string data1, string data2, string data4)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //set the Terms and Condition Link
            WebTemplateModel.TermsConditionsLink = new FooterLink { Href = "http://www.tsn.ca", NewWindow = true };
            //set the Privacy link
            WebTemplateModel.PrivacyLink = new FooterLink { Href = "http://www.lapresse.ca" }; // NewWindow defaults to false
            //execute logic for the submit.
            return View();
        }
    }
}
