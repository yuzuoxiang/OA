using System;
using System.Configuration;
using System.IO;
using System.Web;
using Tools.StrClass;

namespace Tools.Logs
{
    /// <summary>
    /// 日志处理文件
    /// </summary>
    public class Log
    {
        //日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        private static int Log_Level = ConfigurationManager.AppSettings["LogLevel"].ToInt32();
        private static string Log_Path
        {
            get
            {
                return HttpContext.Current != null
                //WebForm时的路径
                ? HttpContext.Current.Request.PhysicalApplicationPath + "/logs"
                //WinForm时的路径
                : AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        //: ConfigurationManager.AppSettings["LogPath"];
        //private static string Log_Path =  +ConfigurationManager.AppSettings["LogPath"];

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <param name="err"></param>
        public static void Debug(string message, string filePath = "", Exception err = null)
        {
            if (Log_Level >= 3)
                WriteLog("DEBUG", filePath, message, err);
        }

        /// <summary>
        /// 正常信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <param name="err"></param>
        public static void Info(string message, string filePath = "", Exception err = null)
        {
            if (Log_Level >= 2)
                WriteLog("INFO", filePath, message, err);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <param name="err"></param>
        public static void Error(string message, string filePath = "", Exception err = null)
        {
            if (Log_Level >= 1)
                WriteLog("ERROR", filePath, message, err);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="level"></param>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <param name="err"></param>
        public static void WriteLog(string level, string filePath, string message, Exception err)
        {
            //如果日志目录不存在就创建
            if (!Directory.Exists(Log_Path))
                Directory.CreateDirectory(Log_Path);

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = Log_Path + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log";//用日期格式对日志命名

            //创建或打开日志文件，向日志文件末尾追加记录
            using (StreamWriter mySw = File.AppendText(filename))
            {
                //当前请求原始URL
                string requestUrl = HttpContext.Current != null ? HttpContext.Current.Request.RawUrl : "";

                //向日志文件写入内容
                string write_content = string.Format("时间：{0}\r\n调用文件：{1}\r\n日志级别：{2}\r\n请求原始URL：{3}\r\n自定义信息：{4}\r\n错误信息：{5}\r\n----------------------------------------------------------------------------------------\r\n\r\n"
                    , time, filePath, level, requestUrl, message, (err != null ? err.ToString() : ""));

                mySw.WriteLine(write_content);
            }
        }
    }
}
