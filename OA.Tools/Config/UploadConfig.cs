using System.Configuration;

namespace Tools.Config
{
    public class UploadConfig: ConfigurationSection
    {
        [ConfigurationProperty("Settings")]
        public SettingsCollection Settings
        {
            get
            {
                return this["Settings"] as SettingsCollection;
            }
        }

        [ConfigurationProperty("SiteOptions")]
        public SiteOptionCollection SiteOptions
        {
            get
            {
                return this["SiteOptions"] as SiteOptionCollection;
            }
        }
    }
}
