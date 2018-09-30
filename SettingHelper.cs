using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;

namespace Christoc.Modules.TestURL
{
    public class SettingHelper
    {
        public const string DetailsPageID = "DetailsPageID";
        public const string EnableUrlReplacement = "EnableUrlReplacement";

        private readonly ExtensionUrlProviderInfo _provider;

        public SettingHelper(ExtensionUrlProviderInfo settingProvider)
        {
            _provider = settingProvider;
        }

        public TabInfo GetSafeTab(int portalId, string settingName, TabInfo defaultValue)
        {
            TabInfo result = defaultValue;
            int tabId = GetSafeInt(settingName, -1);
            if (tabId > 0)
            {
                TabController tc = new TabController();
                result = tc.GetTab(tabId, portalId, false);
            }
            return result;
        }

        public int GetSafeInt(string settingName, int defaultValue)
        {
            int result = defaultValue;
            string raw = null;
            if (_provider != null && _provider.Settings != null && _provider.Settings.ContainsKey(settingName))
            {
                raw = _provider.Settings[settingName];
            }
            if (!string.IsNullOrEmpty(raw)) int.TryParse(raw, out result);
            return result;
        }

        public bool GetSafeBool(string settingName, bool defaultValue)
        {
            bool result = defaultValue;
            string raw = null;
            if (_provider != null && _provider.Settings != null && _provider.Settings.ContainsKey(settingName))
            {
                raw = _provider.Settings[settingName];
            }
            if (!string.IsNullOrEmpty(raw)) bool.TryParse(raw, out result);
            return result;
        }
    }
}