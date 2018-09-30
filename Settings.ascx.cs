using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Christoc.Modules.TestURL
{
    public partial class Settings : PortalModuleBase, IExtensionUrlProviderSettingsControl
    {
        private SettingHelper _settingsHelper;
        public ExtensionUrlProviderInfo Provider { get; set; }

        public void LoadSettings()
        {
            if (!IsPostBack)
            {
                if (Provider != null)
                {
                    _settingsHelper = new SettingHelper(Provider);
                    TabController TC = new TabController();
                    foreach (var item in TC.GetTabsByPortal(PortalId))
                    {
                        dlDetailsPge.Items.Add(new ListItem(item.Value.TabName, item.Value.TabID.ToString()));
                    }
                    dlDetailsPge.SelectedValue = _settingsHelper.GetSafeInt(SettingHelper.DetailsPageID, -1).ToString();
                    chkEnableUrlReplacements.Checked = _settingsHelper.GetSafeBool(SettingHelper.EnableUrlReplacement, false);
                }
                else
                {
                    throw new ArgumentNullException("ExtensionUrlProviderInfo is null on LoadSettings()");
                }

            }
        }
        public Dictionary<string, string> SaveSettings()
        {
            var settings = new Dictionary<string, string>();
            _settingsHelper = new SettingHelper(Provider);

            if (dlDetailsPge.SelectedValue != null)
            {
                settings.Add(SettingHelper.DetailsPageID, dlDetailsPge.SelectedValue.ToString());
            }

            settings.Add(SettingHelper.EnableUrlReplacement, chkEnableUrlReplacements.Checked.ToString().ToLower());

            return settings;
        }
    }
}