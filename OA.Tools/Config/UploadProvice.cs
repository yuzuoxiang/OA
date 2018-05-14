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
                uploadConfig = ConfigurationManager.GetSection("UploadConfig") as UploadConfig;
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
