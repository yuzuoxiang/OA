using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Tools.StrClass
{
    /// <summary>
    /// 安全的在页面中输出各种内容
    /// 包括过滤xss脚本
    /// </summary>
    public class SafeEcho
    {
        /// <summary>
        /// 过滤内容中的其他网站链接
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveOtherSiteLink(string content)
        {
            //<a[^>]*https?://(?!(\w+\.)*dzsc.com)[^>]*?>(.*?)</a>
            //<a[^>]*href\s*?=\s*?["'](?![\w+\.:/]+?\.dzsc\.com)[^>]*?>(?<get>.*?)</a>
            return Regex.Replace(content, @"<a[^>]*href\s*?=\s*?[""'](?![\w+\.:/\-_]+?\.dzsc\.com)[^>]*?>(?<get>.*?)</a>", "$1", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 输出动态html代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Html(string str)
        {
            return FilterXss2(str);
        }

        #region sql防注入检测
        /// <summary>
        /// 检测关键词，防止SQL注入
        /// </summary>
        /// <param name="sqlValue">需要检测的关键词</param>
        /// <returns></returns>
        public static string CheckSqlValue(string sqlValue)
        {
            string reStr = sqlValue;
            if (reStr == null)
                reStr = "";
            reStr = reStr.Replace("'", "''");
            return reStr;
        }

        /// <summary>
        /// 检测Like字符串,防止注入和关键词匹配(替换 '_ % [ ] )
        /// </summary>
        /// <param name="keyword">需要检测的关键词</param>
        /// <returns></returns>
        public static string CheckSqlKeyword(string keyword)
        {
            string reStr = keyword;
            if (reStr == null)
                reStr = "";
            reStr = reStr.Replace("'", "''");
            reStr = reStr.Replace("[", "");
            reStr = reStr.Replace("]", "");
            reStr = reStr.Replace("%", "[%]");
            reStr = reStr.Replace("_", "[_]");
            return reStr;
        }

        /// <summary>
        /// 检测数据库字段名或表名
        /// </summary>
        /// <param name="fieldName">要检测的字段名或表名</param>
        /// <returns></returns>
        public static bool CheckSqlField(string fieldName)
        {
            return string.IsNullOrEmpty(fieldName) ? false : Regex.IsMatch(fieldName, @"^[a-zA-Z0-9_\.\,]+$");
        }

        /// <summary>
        /// 检测全文索引关键词(* " ')且长度不少于3
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string CheckSqlFullText(string keyword)
        {
            keyword = new Regex(@"\W+", RegexOptions.IgnoreCase).Replace(keyword.Trim(), "|");
            keyword = new Regex(@"\*|""|'", RegexOptions.IgnoreCase).Replace(keyword, "");
            return keyword;
        }
        #endregion

        #region xss
        private static string FilterXss(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            //把可能的16进制转回来
            Regex r16 = new Regex("&#[x|X]0{0,}(\\d*);?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string ret = r16.Replace(html, new MatchEvaluator(R16CapText));

            //去掉标签中的换行
            //Regex reg = new Regex("=[\\s]*?('|\")(.*?)(\\1)", RegexOptions.IgnoreCase|RegexOptions.Singleline);
            Regex reg = new Regex("<[^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(ret);
            ret = reg.Replace(ret, new MatchEvaluator(CapText));

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.    
            // 目的防止这样形式的入侵 ＜java\0script＞    
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到    
            ret = Regex.Replace(ret, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            //替换属性中的

            //过滤\t, \n, \r构建的恶意代码  
            string[] keywords = {"javascript", "vbscript", "expression",
                "applet", "meta", "xml", "blink", "link", "style",
                "script", "embed", "object", "iframe", "frame",
                "frameset", "ilayer", "layer", "bgsound", "title",
                "base" ,"onabort", "onactivate", "onafterprint",
                "onafterupdate", "onbeforeactivate", "onbeforecopy",
                "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus",
                "onbeforepaste", "onbeforeprint", "onbeforeunload",
                "onbeforeupdate", "onblur", "onbounce", "oncellchange",
                "onchange", "onclick", "oncontextmenu", "oncontrolselect",
                "oncopy", "oncut", "ondataavailable", "ondatasetchanged",
                "ondatasetcomplete", "ondblclick", "ondeactivate",
                "ondrag", "ondragend", "ondragenter", "ondragleave",
                "ondragover", "ondragstart", "ondrop", "onerror",
                "onerrorupdate", "onfilterchange", "onfinish",
                "onfocus", "onfocusin", "onfocusout", "onhelp",
                "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete",
                "onload", "onlosecapture", "onmousedown", "onmouseenter",
                "onmouseleave", "onmousemove", "onmouseout", "onmouseover",
                "onmouseup", "onmousewheel", "onmove", "onmoveend",
                "onmovestart", "onpaste", "onpropertychange",
                "onreadystatechange", "onreset", "onresize",
                "onresizeend", "onresizestart", "onrowenter",
                "onrowexit", "onrowsdelete", "onrowsinserted",
                "onscroll", "onselect", "onselectionchange",
                "onselectstart", "onstart", "onstop", "onsubmit",
                "onunload"};

            bool found = true;
            while (found)
            {
                var retBefore = ret;
                for (int i = 0; i < keywords.Length; i++)
                {
                    string pattern = "/";
                    for (int j = 0; j < keywords[i].Length; j++)
                    {
                        if (j > 0)
                            pattern = string.Concat(pattern,
                                '(', "(&#[x|X]0{0,8}([9][a][b]);?)?",
                                "|(&#0{0,8}([9][10][13]);?)?",
                                ")?");
                        pattern = string.Concat(pattern, keywords[i][j]);
                    }
                    string replacement =
                        string.Concat(keywords[i].Substring(0, 2),
                            "xx", keywords[i].Substring(2));
                    ret =
                        System.Text.RegularExpressions.Regex.Replace(ret,
                            pattern, replacement,
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (ret == retBefore)
                        found = false;
                }
            }
            return ret;
        }

        private static string FilterXss2(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            //把可能的16进制转回来
            Regex r16 = new Regex("&#[x|X]0{0,}(\\d*);?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string ret = r16.Replace(html, new MatchEvaluator(R16CapText));

            //去掉标签中的换行
            //Regex reg = new Regex("=[\\s]*?('|\")(.*?)(\\1)", RegexOptions.IgnoreCase|RegexOptions.Singleline);
            Regex reg = new Regex("<[^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(ret);
            ret = reg.Replace(ret, new MatchEvaluator(CapText));

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.    
            // 目的防止这样形式的入侵 ＜java\0script＞    
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到    
            ret = Regex.Replace(ret, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            string[] tags = { "script", "link", "style", "meta", "frameset", "iframe", "frame", "embed", "object", "xml", "html", "body" };
            string[] props = { "javascript", "vbscript", "expression",
                "applet", "blink",
                "script", "ilayer", "layer", "bgsound", "title",
                "base" ,"onabort", "onactivate", "onafterprint",
                "onafterupdate", "onbeforeactivate", "onbeforecopy",
                "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus",
                "onbeforepaste", "onbeforeprint", "onbeforeunload",
                "onbeforeupdate", "onblur", "onbounce", "oncellchange",
                "onchange", "onclick", "oncontextmenu", "oncontrolselect",
                "oncopy", "oncut", "ondataavailable", "ondatasetchanged",
                "ondatasetcomplete", "ondblclick", "ondeactivate",
                "ondrag", "ondragend", "ondragenter", "ondragleave",
                "ondragover", "ondragstart", "ondrop", "onerror",
                "onerrorupdate", "onfilterchange", "onfinish",
                "onfocus", "onfocusin", "onfocusout", "onhelp",
                "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete",
                "onload", "onlosecapture", "onmousedown", "onmouseenter",
                "onmouseleave", "onmousemove", "onmouseout", "onmouseover",
                "onmouseup", "onmousewheel", "onmove", "onmoveend",
                "onmovestart", "onpaste", "onpropertychange",
                "onreadystatechange", "onreset", "onresize",
                "onresizeend", "onresizestart", "onrowenter",
                "onrowexit", "onrowsdelete", "onrowsinserted",
                "onscroll", "onselect", "onselectionchange",
                "onselectstart", "onstart", "onstop", "onsubmit",
                "onunload"};

            //remove validation tags
            foreach (string tag in tags)
            {
                Regex regTag = new Regex("<(" + tag + ")[^>]*?>[\\s\\S]*?</\\1>", RegexOptions.IgnoreCase);
                ret = regTag.Replace(ret, "");
            }

            foreach (string prop in props)
            {
                ret = ret.Replace(prop, prop + "_");
            }
            return ret;
        }

        private static string CapText(Match m)
        {
            return new Regex("[\r\n\t]", RegexOptions.IgnoreCase | RegexOptions.Singleline).Replace(m.Groups[0].Value, "");
        }

        private static string R16CapText(Match m)
        {
            return ((char)Convert.ToInt32(m.Groups[1].Value)).ToString();
        }
        #endregion
    }
}