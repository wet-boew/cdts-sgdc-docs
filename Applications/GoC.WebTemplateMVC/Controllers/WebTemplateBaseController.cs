﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.Reflection;

using System.Web.Caching;
using System.Web;
using GoC.WebTemplate.Proxies;
using WebTemplateCore.JSONSerializationObjects;
using WebTemplateCore.Proxies;

namespace GoC.WebTemplate
{
    public class WebTemplateBaseController : Controller
    {
        /// <summary>
        /// Method is overridden to allows us to add the web template data/info to the viewbag
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            PopulateViewBag();
            return base.View(viewName, masterName, model);
        }
        protected override ViewResult View(IView view, object model)
        {
            PopulateViewBag();
            return base.View(view, model);
        }
        /// <summary>
        /// Method that adds the info to the ViewBag. The viewbag is used by the layout files.
        /// </summary>
        protected virtual void PopulateViewBag()
        {
            ViewBag.WebTemplateCore = this.WebTemplateCore;
            ViewBag.WebTemplateVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        /// <summary>
        /// Method is executed for every action.  It is used to control the culture(language) of the site
        /// It also instantiates the Code object
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //Get culture from Querystring
            string culture = this.Request.QueryString.Get(Constants.QUERYSTRING_CULTURE_KEY);
         
            if ((string.IsNullOrEmpty(culture)))
            {
                if (this.HttpContext.Session != null)
                {
                    //culture not found in querystring, check session
                    culture = Convert.ToString(Session[Constants.SESSION_CULTURE_KEY], CultureInfo.CurrentCulture);

                    if ((string.IsNullOrEmpty(culture)))
                    {
                        //culture not found in session, use default language
                        culture = Constants.ENGLISH_CULTURE;
                        this.Session[Constants.SESSION_CULTURE_KEY] = culture;
                    }
                }
                else
                {
                    culture = Constants.ENGLISH_CULTURE;
                }
            }
            else
            {
                //culture found in querystring, use it
                culture = culture.StartsWith(Constants.ENGLISH_ACCRONYM, StringComparison.CurrentCultureIgnoreCase) ? Constants.ENGLISH_CULTURE : Constants.FRENCH_CULTURE;
                if (this.HttpContext.Session != null) this.Session[Constants.SESSION_CULTURE_KEY] = culture;
            }

            //If we have a culture, set it
            if (!string.IsNullOrEmpty(culture))
            {
                Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(culture);
            }

            //Core needs to be created here to pass in the proper culture 
            WebTemplateCore = new Core(new CurrentRequestProxy(),
                                       new CacheProxy(),
                                       new ConfigurationProxy(),
                                       new CDTSEnvironmentLoader(new CacheProxy()).LoadCDTSEnvironments("~/CDTSEnvironments.json"));

            return base.BeginExecuteCore(callback, state);
        }

#region Properties
        protected Core WebTemplateCore { get; set; }
#endregion

    }
}