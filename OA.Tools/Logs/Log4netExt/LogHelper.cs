using System;

namespace Tools.Logs.Log4netExt
{
    public class LogHelper
    {
        /// <summary>
        /// 记录完整错误信息
        /// by:willian date:2016-9-28
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        /// <param name="loginId"></param>
        /// <param name="loginUserName"></param>
        public static void WriteLog(Type t, Exception ex, int loginId = 0, string loginUserName = "")
        {
            string Ip = Tools.Utility.ClientTool.GetIP();
            string RequestUrl = Tools.Utility.ClientTool.GetNowPage();
            IWebLog WebLog = WebLogHelper.GetLogger(t);
            WebLog.Error(Ip, RequestUrl, ex.Message.ToString(), ex, loginId, loginUserName);
        }

        /// <summary>
        /// 记录警告信息
        /// by:willian date:2016-9-28
        /// </summary>
        /// <param name="info"></param>
        public static void WriteLog(Exception ex, int loginId = 0, string loginUserName = "")
        {
            string Ip = Tools.Utility.ClientTool.GetIP();
            string RequestUrl = Tools.Utility.ClientTool.GetNowPage();
            IWebLog WebLog = WebLogHelper.GetLogger("loginfo");
            WebLog.Warn(Ip, RequestUrl, ex.Message, ex, loginId, loginUserName);
        }

        /// <summary>
        /// 记录警告信息
        /// by:willian daet:2016-9-28
        /// </summary>
        /// <param name="info"></param>
        public static void WarnInfo(string info, int loginId = 0, string loginUserName = "")
        {
            string Ip = Tools.Utility.ClientTool.GetIP();
            string RequestUrl = Tools.Utility.ClientTool.GetNowPage();
            IWebLog WebLog = WebLogHelper.GetLogger("loginfo");
            WebLog.Warn(Ip, RequestUrl, info, loginId, loginUserName);
        }
    }
}
