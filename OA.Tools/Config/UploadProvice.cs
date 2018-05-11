using System.Configuration;

namespace Tools.Config
{
    public class UploadProvice
    {
        private static UploadConfig uploadConfig;
        static UploadProvice()
        {
            try
            {
                var dd = ConfigurationManager.GetSection("UploadConfig");
                uploadConfig = new UploadConfig();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static UploadConfig Instance()
        {
            return uploadConfig;
        }
    }
}
