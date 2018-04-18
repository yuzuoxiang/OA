using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Tools.Tool
{
    /// <summary>
    /// QueryString 地址栏参数
    /// </summary>
    public class QueryString
    {
        #region 等于Request.QueryString;如果为null 返回 空“” ，否则返回Request.QueryString[name]
        /// <summary>
        /// 等于Request.QueryString;如果为null 返回 空“” ，否则返回Request.QueryString[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Q(string name)
        {
            return Request.QueryString[name] == null ? "" : Request.QueryString[name];
        }
        #endregion
        /// <summary>
        /// 等于  Request.Form  如果为null 返回 空“” ，否则返回 Request.Form[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string F(string name)
        {
            return Request.Form[name] == null ? "" : Request.Form[name].ToString();
        }
        public static int FId(string name)
        {
            string str = Request.Form[name] == null ? "" : Request.Form[name];
            return StrToId(str);
        }
        public static string C(string a, string b)
        {
            if (HttpContext.Current.Request.Cookies[a] == null)
            {
                return "";
            }
            else
            {
                return System.Web.HttpContext.Current.Request.Cookies[a][b];
            }
        }
        public static string CC(string a)
        {
            if (HttpContext.Current.Request.Cookies[a] == null)
            {
                return "";
            }
            else
            {
                return HttpContext.Current.Request.Cookies[a].Value;
            }
        }
        public static string QF(string name)
        {
            return Request[name] == null ? "" : Request[name].ToString();
        }

        public static string QF(string name, string defaultval)
        {
            string val = QF(name);
            if (string.IsNullOrEmpty(val))
            {
                return defaultval;
            }
            return val;
        }

        public static string P(string name)
        {
            return Request.Params[name] == null ? "" : Request.Params[name].ToString();
        }
        #region 获取url中的id
        /// <summary>
        /// 获取url中的id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int QId(string name)
        {
            return StrToId(Q(name));
        }
        public static int QId(string name, int defaultval)
        {
            string val = Q(name);
            int iRet;
            if (int.TryParse(val, out iRet) == false)
            {
                iRet = defaultval;
            }
            return iRet;
        }
        public static int QFId(string name)
        {
            return StrToId(QF(name));
        }
        public static int QFId(string name, int defaultval)
        {
            string val = QF(name);
            int iRet;
            if (int.TryParse(val, out iRet) == false)
            {
                iRet = defaultval;
            }
            return iRet;
        }
        #endregion
        #region 获取正确的Id，如果不是正整数，返回0
        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        #endregion

        #region 获取过滤后的字符串
        public static string StrToStr(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = Regex.Replace(value, @";", string.Empty);
            value = Regex.Replace(value, @"'", string.Empty);
            value = Regex.Replace(value, @"&", string.Empty);
            value = Regex.Replace(value, @"%20", string.Empty);
            value = Regex.Replace(value, @"--", string.Empty);
            value = Regex.Replace(value, @"==", string.Empty);
            value = Regex.Replace(value, @"<", string.Empty);
            value = Regex.Replace(value, @">", string.Empty);
            //value = Regex.Replace(value, @"table", string.Empty);
            //value = Regex.Replace(value, @"drop", string.Empty);
            //value = Regex.Replace(value, @"insert", string.Empty);
            return value;
        }
        #endregion

        #region FormatNumSerial 格式化数字，以逗号分开
        public static string FormatNumSerial(string value)
        {
            if (string.IsNullOrEmpty(value)) return "0";
            int i;
            value = value.Replace(" ", "");
            if (value.Length < 1) return "0";
            string[] a = value.Split(new char[] { ',' });
            string ss = "";
            for (i = 0; i < a.Length; i++)
            {
                if (StrToId(a[i]) != 0) ss += a[i] + ",";
            }
            if (ss.Length > 0)
            {
                ss = ss.Substring(0, ss.Length - 1);
            }
            return ss;
        }

        #endregion

        #region CheckBoxScript
        public static string CheckBoxScript(string FormElement, string ElementValue)
        {
            string s = "";
            s += "<s" + "cript language=\"JavaScript\">" + "\n";
            s += "var ss ='" + ElementValue + "';" + "\n";
            s += "for(i=0;i<" + FormElement + ".length;i++){" + "\n";
            s += "if(ss.indexOf(" + FormElement + "[i].value)>=0){ " + "\n";
            s += FormElement + "[i].checked = true" + "\n";
            s += "}	}" + "\n";
            s += "</" + "script>" + "\n";
            return s;
        }

        //新增
        public static string Str4Js(string sInfo)
        {
            if (string.IsNullOrEmpty(sInfo)) return "";
            sInfo = sInfo.Replace(@"\", @"\\");
            sInfo = sInfo.Replace(@"'", @"\'");
            return sInfo;
        }

        public static string BoolToInt(object s)
        {
            bool ss = (bool)s;
            if (ss)
            {
                return "1";
            }
            return "0";
        }
        public static bool IntToBool(int s)
        {
            if (s == 1) return true;
            return false;
        }
        #endregion

        #region Str4Sql
        public static string Str4Sql(string sText)
        {
            string ostr = sText;
            if (ostr != null)
            {
                //ostr = Regex.Replace(ostr, @"'", "''");               
            }
            else
            {
                ostr = "";
            }
            return ostr;
        }
        #endregion
        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        #endregion
        #region 快速验证一个字符串是否符合指定的正则表达式。
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
        #endregion

        #region 类内部调用
        /// <summary>
        /// HttpContext Current
        /// </summary>
        public static HttpContext Current
        {
            get { return HttpContext.Current; }
        }
        /// <summary>
        /// HttpContext Current  HttpRequest Request   get { return Current.Request;
        /// </summary>
        public static HttpRequest Request
        {
            get { return Current.Request; }
        }
        /// <summary>
        ///  HttpContext Current  HttpRequest Request   get { return Current.Request; HttpResponse Response  return Current.Response;
        /// </summary>
        public static HttpResponse Response
        {
            get { return Current.Response; }
        }
        #endregion
    }
}
