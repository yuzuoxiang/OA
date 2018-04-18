using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.StrClass
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public class StringProcess
    {
        /// <summary>
        /// 剪切字符串(中文算2个字符)
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <param name="addEllipsis">末尾添加省略号</param>
        /// <returns></returns>
        public static string CutString(string strInput, int intLen, bool addEllipsis = true)
        {
            if (String.IsNullOrEmpty(strInput))
                return strInput;
            strInput = strInput.Trim();
            byte[] buffer1 = Encoding.Default.GetBytes(strInput);
            if (buffer1.Length > intLen)
            {
                string text1 = "";
                for (int num1 = 0; num1 < strInput.Length; num1++)
                {
                    byte[] buffer2 = Encoding.Default.GetBytes(text1 + strInput.Substring(num1, 1));
                    if (buffer2.Length > intLen)
                        break;
                    text1 = string.Format("{0}{1}", text1, strInput.Substring(num1, 1));
                }
                return addEllipsis ? (text1 + "...") : text1;
            }
            return strInput;
        }

        /// <summary>
        /// 返回int型列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns>用逗号分割的数字字符串/空字符串</returns>
        public static List<int> GetListByComma(string str)
        {
            List<int> list = new List<int>();
            string[] strArr = str.Split(',');
            foreach (var i in strArr)
            {
                if (i.ToInt32() > 0)
                    list.Add(i.ToInt32());
            }

            return list;
        }

        /// <summary>
        /// 用逗号分割的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>用逗号分割的数字字符串/空字符串</returns>
        public static string GetCutStringByComma(string str)
        {
            List<string> list = new List<string>();
            string[] strArr = str.Split(',');
            foreach (var i in strArr)
            {
                if (i.ToInt32() > 0)
                    list.Add(i);
            }

            return string.Join(",", list.ToArray());
        }

        /// <summary>
        /// 截取字符串,空格截取为一句
        /// by:willian date:2015-11-30
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string GetSubSentence(string strInput, int intLen)
        {
            string subStr = CutString(strInput, intLen, false);
            return subStr.Substring(0, subStr.LastIndexOf(' ') > 0 ? subStr.LastIndexOf(' ') : subStr.Length) + "…";
        }

        /// <summary>
        /// 获取：张***三格式的字段
        /// </summary>
        /// <param name="strInput">文件</param>
        /// <returns></returns>
        public static string GetSubContent(string strInput)
        {
            if (String.IsNullOrEmpty(strInput))
                return strInput;
            else
            {
                string text1 = "";
                text1 += strInput.Substring(0, 1);
                for (int num1 = 1; num1 < strInput.Length - 1; num1++)
                {
                    text1 += "*";
                }
                text1 += strInput.Substring(strInput.Length - 1);
                return text1;
            }
        }

        /// <summary>
        /// 获取邮箱后缀
        /// by:willian date:2016-6-7
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetEmailSubffix(string email)
        {
            return email.Substring(email.IndexOf('@') + 1, email.Length - email.IndexOf('@') - 1);
        }

        /// <summary>
        /// 转换大写字母 避免特殊字符转换错误
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperNew(string str)
        {
            string ret = string.Empty;
            str = str.Trim();
            foreach (char zf in str)
            {
                if ('a' <= zf && zf <= 'z')
                {
                    ret += char.ToUpper(zf);
                }
                else
                {
                    ret += zf;
                }
            }

            return ret;
        }

        public static string GetContent(string strOriginal, string strFirst, string strLast, bool trim = true)
        {
            if (string.IsNullOrEmpty(strOriginal) == true)
                return "";

            string s = "";
            int t1, t2, t3;
            if (trim)
            {
                string strOriginal1 = strOriginal, strFirst1 = strFirst, strLast1 = strLast;
                t1 = strOriginal1.IndexOf(strFirst1);
                if (t1 >= 0)
                {
                    t2 = strOriginal1.Length;
                    t3 = t1 + strFirst1.Length;
                    strOriginal1 = strOriginal1.Substring(t3);

                    t1 = strOriginal1.IndexOf(strLast1);
                    t3 = t1;
                    if (t3 > 0)
                        s = strOriginal1.Substring(0, t3);
                }
            }
            else
            {
                s = GetContent(strOriginal, strFirst, strLast);
                s = strFirst + s + strLast;
            }
            return s;
        }
    }
}
