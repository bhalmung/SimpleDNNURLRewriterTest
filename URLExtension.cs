using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;
using DotNetNuke.Common.Utilities;
using System.Text.RegularExpressions;
using System.Collections;
using Christoc.Modules.TestURL.Components;

namespace Christoc.Modules.TestURL
{
    public class URLExtension : ExtensionUrlProvider
    {


        internal int DetailsPageID
        {
            get
            {
                SettingHelper settingsHelper = new SettingHelper(this.ProviderConfig);
                return settingsHelper.GetSafeInt(SettingHelper.DetailsPageID, -1);
            }
        }

        internal bool EnableUpdates
        {
            get
            {
                SettingHelper settingsHelper = new SettingHelper(this.ProviderConfig);
                return settingsHelper.GetSafeBool(SettingHelper.EnableUrlReplacement, false);
            }
        }


        public override bool AlwaysUsesDnnPagePath(int portalId)
        {

            return true;
        }

        public override string ChangeFriendlyUrl(TabInfo tab, string friendlyUrlPath, FriendlyUrlOptions options, string cultureCode, ref string endingPageName, out bool useDnnPagePath, ref List<string> messages)
        {
            string newUrlPath = friendlyUrlPath;
            //set default values for out parameters
            useDnnPagePath = AlwaysUsesDnnPagePath(tab.PortalID);
            if (messages == null) messages = new List<string>();

            if (EnableUpdates && DetailsPageID == tab.TabID)
            {
                // Match only path that starts with /Post/xxxx.  If there are parameters like "/ctl/PostEdit/"
                // before the string, then don't replace the url because it interferes with editing
                var _MatchRegx = new Regex("^/ItemID/([0-9]*)");
                var match = _MatchRegx.Match(friendlyUrlPath);
                if (match.Success && match.Groups.Count == 2 && !string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    int itemID = 0;
                    Int32.TryParse(match.Groups[1].Value, out itemID);
                    if (itemID > 0)
                    {
                        IDictionary<int, string> _List = ItemManager.Instance.GetItems();
                        string slug = _List[itemID];
                        if (!String.IsNullOrEmpty(slug))
                        {
                            newUrlPath = "/" + CleanNameForUrl(slug, options);
                            endingPageName = "";

                        }
                    }
                }

            }
            return newUrlPath;
        }

        public override bool CheckForRedirect(int tabId, int portalid, string httpAlias, Uri requestUri, NameValueCollection queryStringCol, FriendlyUrlOptions options, out string redirectLocation, ref List<string> messages)
        {
            redirectLocation = "";
            return false;
        }

        public override Dictionary<string, string> GetProviderPortalSettings()
        {
            return null;
        }
        public override bool AlwaysCallForRewrite(int portalId)
        {
            return false;
        }

        public override string TransformFriendlyUrlToQueryString(string[] urlParms, int tabId, int portalId, FriendlyUrlOptions options, string cultureCode, PortalAliasInfo portalAlias, ref List<string> messages, out int status, out string location)
        {

            int Key = 0;
            location = null;
            string result = "";
            status = 200; //OK 
            try
            {
                location = ""; //no redirect location
                if (messages == null) messages = new List<string>();
                IDictionary<int, string> _List = ItemManager.Instance.GetItems();

                if (tabId == DetailsPageID && urlParms.Length == 1)
                {

                    var _Ditem = _List.FirstOrDefault(x => CleanNameForUrl(x.Value, options).ToLower() == urlParms[0].ToLower());
                    Key = _Ditem.Key;
                }
                if (Key != 0)
                {
                    List<string> _Query = new List<string>();
                    _Query.Add($"itemid={Key}");
                    _Query.Add($"tabid={tabId}");
                    TabController T = new TabController();
                    result =string.Join("&",_Query);
                }
            }
            catch { }

            return result;
        }
    }
}