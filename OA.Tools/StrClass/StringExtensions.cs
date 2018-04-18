using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools
{
    public static class StringExtensions
    {
        /// <summary>
        /// 安全输出字符串(页面内容)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHtmlEncode(this string s)
        {
            return System.Web.HttpUtility.HtmlEncode(s);
        }

        #region 转换数字
        /// <summary>
        /// 转换INT类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public static int ToInt32(this string s, int defautValue)
        {
            int result;
            return int.TryParse(s, out result) ? result : defautValue;
        }

        /// <summary>
        /// 转换INT类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt32(this string s)
        {
            return ToInt32(s, 0);
        }

        /// <summary>
        /// 转换INT64类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public static long ToInt64(this string s, long defautValue)
        {
            long result;
            return long.TryParse(s, out result) ? result : defautValue;
        }

        /// <summary>
        /// 转换INT64类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToInt64(this string s)
        {
            return ToInt64(s, 0);
        }

        /// <summary>
        /// 判断是否INT类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int result;
            return int.TryParse(s, out result);
        }

        /// <summary>
        /// 转换成FLOAT类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string s, float defautValue)
        {
            float result;
            return float.TryParse(s, out result) ? result : defautValue;
        }

        /// <summary>
        /// 转换成FLOAT类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float ToFloat(this string s)
        {
            return ToFloat(s, 0);
        }

        /// <summary>
        /// 转换成Double类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string s, double defaultValue)
        {
            double result;
            return double.TryParse(s, out result) ? result : defaultValue;
        }

        /// <summary>
        /// 转换成Double类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ToDouble(this string s)
        {
            return ToDouble(s, 0);
        }

        /// <summary>
        /// 1.1000进来 1.1返回
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>

        public static string ToString3(this string s)
        {
            int len = s.Length;
            if (s.Substring(len - 1) == "0")
            {
                return ToString3(s.Substring(0, len - 1));
            }
            else if (s.Substring(len - 1) == ".")
            {
                return s.Substring(0, len - 1);
            }
            else
            {
                return s;
            }
        }

        public static decimal ToDecimal(this string s, decimal defaultValue)
        {
            decimal result;
            return decimal.TryParse(s, out result) ? result : defaultValue;
        }

        public static decimal ToDecimal(this string s)
        {
            return ToDecimal(s, 0);
        }

        public static byte ToByte(this string s)
        {
            byte b;
            byte.TryParse(s, out b);
            return b;
        }
        #endregion

        #region 转日期
        /// <summary>
        /// 转换日期格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string s)
        {
            DateTime time;
            return DateTime.TryParse(s, out time) ? time : Usual.Common.InitDateTime();
        }

        public static string ToForeignDateTime(this DateTime time, string format)
        {
            return time.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 转化日期格式 国外格式：December 04,2015
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToForeignDateTimeShort2(this string s)
        {
            DateTime time;
            return DateTime.TryParse(s, out time) ? time.ToForeignDateTime("MMM dd,yyyy") : Usual.Common.InitDateTime().ToString();
        }

        public static string ToForeignDateTimeShort2(this DateTime time)
        {
            return time.ToForeignDateTime("MMM dd,yyyy");
        }

        /// <summary>
        /// 转化日期格式 国外格式：04 December 2015
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToForeignDateTimeShort(this string s)
        {
            DateTime time;
            return DateTime.TryParse(s, out time) ? time.ToForeignDateTime("dd MMM yyyy") : Usual.Common.InitDateTime().ToString();
        }

        public static string ToForeignDateTimeShort(this DateTime time)
        {
            return time.ToForeignDateTime("dd MMM yyyy");
        }

        /// <summary>
        /// 转化日期格式 国外格式：04 December 2015 19:30:00
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToForeignDateTimeLong(this string s)
        {
            DateTime time;
            return DateTime.TryParse(s, out time) ? time.ToForeignDateTime("dd MMM yyyy HH:mm:ss") : Usual.Common.InitDateTime().ToString();
        }

        public static string ToForeignDateTimeLong(this DateTime time)
        {
            return time.ToForeignDateTime("dd MMM yyyy HH:mm:ss");
        }

        /// <summary>
        /// 判断是否日期格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string s)
        {
            DateTime time;
            return DateTime.TryParse(s, out time);
        }
        #endregion

        /// <summary>
        /// 部分字符串获取
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxlen"></param>
        /// <returns></returns>
        public static string SubString2(this string str, int maxlen)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            if (str.Length <= maxlen)
                return str;
            return str.Substring(0, maxlen);
        }

        public static string ToString2(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : s;
        }

        public static string ToUrlEncode2(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : System.Web.HttpContext.Current.Server.UrlEncode(s);
        }

        public static string ToUrlEncode(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : System.Web.HttpUtility.UrlEncode(s.Trim());
        }

        public static string ToUrlDecode(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : System.Web.HttpUtility.UrlDecode(s.Trim());
        }

        public static string Trim2(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : s.Trim();
        }
        public static bool ToBool(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            s = s.ToLower();

            return (s == "true" || s == "1");
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        

        /// <summary>
        /// 保留小数点后面2至4位,去除末尾多余0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToString2(this decimal d)
        {
            string s = d.ToString("F4");
            int i = s.IndexOf('.') + 3;
            return s.Substring(0, i) + s.Substring(i).TrimEnd('0');
        }

        #region 正则匹配
        public static string Match(this string s, string pattern, RegexOptions options = RegexOptions.None, string defValue = "")
        {
            if (string.IsNullOrEmpty(s))
                return defValue;
            var mat = Regex.Match(s, pattern, options);
            if (mat.Success)
            {
                if (mat.Groups.Count > 1)
                    return mat.Groups.OfType<Group>().Skip(1).Select(m => m.Value).JoinString();
                else
                    return mat.Value;
            }
            return defValue;
        }

        /// <summary>
        /// 检测字符串是否满足指定正则表达式。
        /// </summary>
        /// <param name="s">要检测的字符串。</param>
        /// <param name="pattern">用于检测的正则表达式。</param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern, RegexOptions options = RegexOptions.None)
        {
            return s != null && Regex.IsMatch(s, pattern, options);
        }

        #endregion

        #region Join String
        public static string JoinString(this string[] strs, string spliter = "")
        {
            return string.Join(spliter, strs);
        }
        public static string JoinString(this IEnumerable<string> strs, string spliter = "")
        {
            return string.Join(spliter, strs);
        }
        #endregion

        #region Int32 To Chinese

        public static Int32 ToInt32(this char c)
        {
            return c - 48;
        }

        static char[] s_cnNums = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
        static char[] s_cnUnit = new char[] { '十', '百', '千', '万', '亿' };
        /// <summary>
        /// 将Int32数字转换为中文大写字符串。
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <returns></returns>
        public static string ToChinese(this int num)
        {
            var nStr = num.ToString();
            string result = string.Empty;
            var sb = new StringBuilder((nStr.Length * 2) + (nStr.Length / 3));
            if (num < 0)
            {
                sb.Append("负");
                nStr = nStr.Remove(0, 1);
            }
            var len = nStr.Length;
            var pZero = false;
            for (int i = 0; i < len; ++i)
            {
                var c = s_cnNums[nStr[i].ToInt32()];

                var pos = len - i;
                if (!pZero && c.Equals(s_cnNums[0]))
                {
                    sb.Append(c);
                    pZero = true;
                }
                else if (!c.Equals(s_cnNums[0]))
                {
                    pos = pos - 1;
                    sb.Append(c);
                    pZero = false;
                    var idx = (pos & 3) - 1;
                    if (idx >= 0)
                        sb.Append(s_cnUnit[idx]);
                    else
                    {
                        idx = (pos & 4) - 1;
                        if (idx >= 0)
                            sb.Append(s_cnUnit[idx]);
                        else
                        {
                            idx = (pos & 8) - 4;
                            if (idx >= 0)
                                sb.Append(s_cnUnit[idx]);
                        }
                    }
                }
                else if (pZero && ((pos & 4) != 0 || (pos & 8) != 0) && (pos & 3) == 0)
                {
                    var idx = (pos & 4) - 1;
                    if (idx >= 0)
                    {
                        if (!sb[sb.Length - 2].Equals(s_cnUnit[4]))
                            sb.Insert(sb.Length - 1, s_cnUnit[idx]);
                    }
                    else
                    {
                        idx = (pos & 8) - 4;
                        if (idx >= 0)
                            sb.Append(s_cnUnit[idx]);
                    }
                }
            }
            if (sb[sb.Length - 1].Equals(s_cnNums[0]))
                sb.Remove(sb.Length - 1, 1);
            result = sb.ToString();
            sb.Remove(0, sb.Length);
            sb = null;
            return result;
        }

        #endregion
    }
}
