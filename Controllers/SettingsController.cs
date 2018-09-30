/*
' Copyright (c) 2018 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Collections;
using System.Web.Mvc;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Tabs;
using System.Collections.Generic;
using DotNetNuke.Entities.Urls;
using System;

namespace Christoc.Modules.TestURL.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController, IExtensionUrlProviderSettingsControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        public ExtensionUrlProviderInfo Provider { get; set; }

        [HttpGet]
        public ActionResult Settings()
        {
            TabController tc = new TabController();
            var List = new List<SelectListItem>();
            var settings = new Models.Settings();
            settings.EnableUrlReplacement = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("EnableUrlReplacement", false);
            settings.DetailsPageID = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("DetailsPageID", -1);
            var Tabs = tc.GetTabsByPortal(PortalSettings.PortalId);
            foreach (var tab in Tabs)
            {
                List.Add(new SelectListItem()
                {
                    Selected = settings.DetailsPageID == tab.Value.TabID,
                    Text = tab.Value.TabName,
                    Value = tab.Value.TabID.ToString(),
                });
            }
            ViewData["DetailPageTab"] = List;
            return View(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supportsTokens"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(Models.Settings settings)
        {
            _settings = settings;
            ModuleContext.Configuration.ModuleSettings["EnableUrlReplacement"] = settings.EnableUrlReplacement.ToString();
            ModuleContext.Configuration.ModuleSettings["DetailsPageID"] = settings.DetailsPageID.ToString();
            SaveSettings();
            return RedirectToDefaultRoute();
        }

        Models.Settings _settings { get; set; }
        public Dictionary<string, string> SaveSettings()
        {
            return new Dictionary<string, string>()
            {
                ["EnableUrlReplacement"] = _settings.EnableUrlReplacement.ToString(),
                ["DetailsPageID"] = _settings.DetailsPageID.ToString()
            };
        }

        public void LoadSettings()
        {

        }
    }
}