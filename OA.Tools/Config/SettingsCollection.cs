using System.Configuration;

namespace Tools.Config
{
    public class SettingsCollection : ConfigurationElementCollection
    {
        public Setting this[int index]
        {
            get
            {
                return base.BaseGet(index) as Setting;
            }
        }
        public new Setting this[string name]
        {
            get
            {
                return base.BaseGet(name) as Setting;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Setting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Setting)element).Name;
        }
    }
}
