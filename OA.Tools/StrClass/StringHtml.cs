using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools.StrClass
{
    public class StringHtml
    {
        /// <summary>
        /// 是否输出nofollow
        /// </summary>
        /// <param name="showFlag"></param>
        /// <returns></returns>
        public static string ResponseNoFollow(bool showFlag = true)
        {
            return showFlag ? " rel=\"nofollow\"" : "";
        }

        private static string replaceA(Match mth)
        {
            string v = mth.Value;
            string rev = string.Empty;
            if (!string.IsNullOrEmpty(v))
            {
                try
                {
                    rev = v.Insert(v.IndexOf(' ') + 1, @"rel=""nofollow"" ");
                }
                catch
                {
                    rev = v;
                }
            }
            return rev;
        }

        /// <summary>
        /// 获取html中所有图片的路径
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetImageUrlListFromHtml(string htmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        /// <summary>
        /// 获取html中所有a标签的链接地址
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetLinkUrlListFromHtml(string htmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<a\b[^<>]*?\bhref[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<hyperLink>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["hyperLink"].Value;
            return sUrlList;
        }

        /// <summary>
        /// 替换文本中不带Nofollow标签的链接加上nofollow
        /// </summary>
        /// <param name="str">需要替换的HTML</param>
        /// <returns></returns>
        public static string ReplaceLinkAddNofollow(string str)
        {
            string exp = @"<[A|a](?![^<>]*rel=[^<>]*)[^<>]*>";
            MatchEvaluator me = new MatchEvaluator(replaceA);
            return System.Text.RegularExpressions.Regex.Replace(str, exp, me);
        }

        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            string text1 = "<.*?>";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            str = str.Replace("&nbsp;", " ");
            return str;
        }

        /// <summary>
        /// 去除关键词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string OutHtmlKeyword(string str)
        {
            string text1 = "<!--.*?-->";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            return str;
        }

        /// <summary>
        /// 去除div和script
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string OutHtmlByDivAndScript(string str)
        {
            string text1 = "<div.*?>|<DIV.*?>|</div.*?>|</DIV.*?>|<script.*?>|<SCRIPT.*?>|</script.*?>|</SCRIPT.*?>";
            Regex reg = new Regex(text1);
            str = reg.Replace(str, "");
            return str;
        }

        /// <summary>
        /// 去除div、script和超链接
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string OutHtmlByDivAndScriptAndHref(string str)
        {
            string text1 = "<div.*?>|</div>|<a.*?>|</a>|(<script.*?>(.*?)</script>)";
            Regex reg = new Regex(text1, RegexOptions.IgnoreCase);
            str = reg.Replace(str, "");
            return str;
        }

        /// <summary>
        /// 去除空格换行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpace(string str)
        {
            Regex reg = new Regex("\\s*");
            return reg.Replace(str, "");
        }

        /// <summary>
        /// 去除html标签并截字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string RemoveHtmlAndLimit(string str, int num)
        {
            str = RemoveHtml(str);
            str = StringProcess.CutString(str, num);

            return str;
        }
        

        /// <summary>
        /// 输出input的value
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Input(string value)
        {
            return Attr(value);
        }

        /// <summary>
        /// 在页面输出文本内容
        /// 会将\n\r转换成br,\t转换成4个nbsp
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Text(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(str);
            //builder1.Replace("&", "&amp;");
            builder1.Replace("<", "&lt;");
            builder1.Replace(">", "&gt;");
            builder1.Replace("\"", "&quot;");
            builder1.Replace("\n", "<br/>");
            //builder1.Replace(" ", "&nbsp;");
            builder1.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            return builder1.ToString();
        }

        /// <summary>
        /// 文本框内容输出成一行(文本内容去回车空格,转换HTML输出)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLineText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = RemoveHtml(str);
            str = str.Replace("　", "");

            string text1 = "\\s+";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, " ").ToHtmlEncode();
            str = str.Replace("&#58;", ";");
            str = str.Replace("&#46;", ".");
            str = str.Replace("&#32;", " ");
            str = str.Replace("&#40;", "(");
            str = str.Replace("&#41;", ")");
            str = str.Replace("&#43;", " ");
            str = str.Replace("&#39;", "'");
            return str.Trim();
        }

        /// <summary>
        /// 文本框内容输出成html显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHtmlText(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(str);
            builder1.Replace("&", "&amp;");
            builder1.Replace("<", "&lt;");
            builder1.Replace(">", "&gt;");
            builder1.Replace("\"", "&quot;");
            builder1.Replace("\r", "<br>");
            builder1.Replace(" ", "&nbsp;");
            return builder1.ToString();
        }

        

        /// <summary>
        /// Texteara 文本显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTexteara(string str)
        {
            if (string.IsNullOrEmpty(str))
                return ("&nbsp;");

            str = str.Replace("\r\n", "<br />");
            str = str.Replace("\r", "<br />");
            str = str.Replace("\t", "<br />");
            return (str);
        }

        /// <summary>
        /// 输出img的src
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Img(string src)
        {
            return Attr(src);
        }

        /// <summary>
        /// 去除2个以上的空格
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveMoreSpace(string content)
        {
            if (string.IsNullOrEmpty(content)) return "";
            Regex r = new Regex(@"\s{2,}", RegexOptions.Multiline);
            return r.Replace(content, " ");
        }

        private static string Attr(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return value.Replace("\n", "").Replace("\r", "").ToHtmlEncode();
        }

        /// <summary>
        /// 在textarea中输出内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string TextArea(string content)
        {
            return content.ToHtmlEncode();
        }

        /// <summary>
        /// 输出Title
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Title(string str)
        {
            return Meta(str);
        }

        /// <summary>
        /// 输出Title
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Title(string str, int len)
        {
            return Meta(str, len);
        }

        /// <summary>
        /// 输出Meta
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Meta(string str)
        {
            str = str.Replace("\r", " ");
            str = str.Replace("\n", " ");
            str = str.Replace("\t", " ");
            str = str.Replace("\"", "");
            str = str.Replace("&nbsp;", " ");
            str = RemoveMoreSpace(str);
            return RemoveHtml(str);
        }

        /// <summary>
        /// 输出Meta
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Meta(string str, int len)
        {
            str = Meta(str);
            return StrClass.StringProcess.CutString(str, len);
        }

        /// <summary>
        /// 去除HTML、空格、换行，常用于标题 Keywords 输出
        /// </summary>
        /// <param name="str"></param>
        /// <param name="line">是否清除换行符</param>
        /// <returns></returns>
        public static string ToLineText(string str, bool line = true)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = RemoveHtml(str);
            str = str.Replace("　", "");
            str = line ? str.Replace("\r\n", " ") : str;
            string text1 = "\\s+";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, " ");
            str = System.Web.HttpUtility.HtmlEncode(str);
            return str.Trim();
        }

        /// <summary>
        /// 过滤内容中的其他网站链接
        /// 创建人：陈民礼  时间：2013-5-2
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveOtherSiteLink(string content)
        {
            //<a[^>]*https?://(?!(\w+\.)*dzsc.com)[^>]*?>(.*?)</a>
            //<a[^>]*href\s*?=\s*?["'](?![\w+\.:/]+?\.dzsc\.com)[^>]*?>(?<get>.*?)</a>
            return Regex.Replace(content, @"<a[^>]*href\s*?=\s*?[""'](?![\w+\.:/\-_]+?\.1goic\.com)[^>]*?>(?<get>.*?)</a>", "$1", RegexOptions.IgnoreCase);
        }
    }
}
