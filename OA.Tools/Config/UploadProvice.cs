using System.Configuration;

namespace Tools.Config
{
    public class UploadProvice
    {
        private static UploadConfig uploadConfig;
        static UploadProvice()
        {
            uploadConfig = ConfigurationManager.GetSection("UploadConfig") as UploadConfig;
        }
        public static UploadConfig Instance()
        {
            return uploadConfig;
        }
    }
}
