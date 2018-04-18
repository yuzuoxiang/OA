using NSoup;
using NSoup.Nodes;
using System; 

namespace Tools.Tool
{   
    /// <summary>
    /// 使Html页面内容变成Document对象
    /// </summary>
    public class NsoupHelper
    {
        Http.HttpHelper httpHelper = new Http.HttpHelper("UTF-8");

        public Document GetDocument(string url)
        {
            try
            {
                string html = GetHtml(url);
                if (string.IsNullOrEmpty(html))
                    return null;
                if (html.Contains("<title>509 unused</title>") || html.Contains("<h1>unused</h1>") || html.Contains("The server encountered an internal error or misconfiguration and was unable to complete your request."))
                    return null;
                return NSoupClient.Parse(html);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetHtml(string url)
        {
            try
            {
                return httpHelper.Get(url).ResultHtml;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
