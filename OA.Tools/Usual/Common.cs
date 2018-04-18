using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Tools.Tool;

namespace Tools.Usual
{
    public class Common
    {
        #region 日期相关
        /// <summary>
        /// 日期值的初始值
        /// </summary>
        /// <returns></returns>
        public static DateTime InitDateTime()
        {
            return DateTime.Parse("1900-01-01");
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static long Timestamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        /// <summary>
        /// 计算时间差（分钟）
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static int DiffDateTime(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts2.Subtract(ts1).Duration();//dt2-dt1的时间差
            return (int)(ts.TotalMinutes);
        }

        /// <summary>
        /// 获取传入日期所在星期的周一和周日
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weekStart"></param>
        /// <param name="weekeEnd"></param>
        public static void GetMondayAndSunday(DateTime date, out DateTime monday, out DateTime sunday)
        {
            monday = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
            sunday = date.AddDays((int)DayOfWeek.Saturday - (int)date.DayOfWeek + 1);
        }
        #endregion

        /// <summary>
        /// 格式化文件质量,传入K
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public static string FormatFileSize(float filesize)
        {
            float filesizeFloat = filesize / 1024;
            if (filesizeFloat < 1024)
            {
                return Math.Ceiling(filesizeFloat) + "K";
            }
            else
            {
                filesizeFloat = filesizeFloat / 1024;
                return Math.Round(filesizeFloat, 2) + "M";
            }
        }

        /// <summary>
        /// 根据ID获取放置的文件夹路径(每个文件夹最多1K个文件)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDocById(int id)
        {
            string docPath = "";
            string strId = id.ToString();
            int strLen = strId.Length;
            int strDepth = (int)Math.Floor((decimal)strLen / 3);
            for (int i = 0; i < strDepth; i++)
            {
                docPath += strId.Substring(0, 3) + "/";
                strId = strId.Substring(3);
            }
            docPath += "000/";
            return docPath;
        }

        /// <summary>
        /// 获取国外常用邮箱登录地址
        /// by:willian  date:2015-12-16
        /// </summary>
        /// <param name="email"></param>
        /// <returns>返回登录地址，非常用返回空</returns>
        public static string GetMailSubfix(string email)
        {
            string subfix = "|" + email.ToLower().Substring(email.LastIndexOf('@') + 1) + "|";
            string emails = "|gmail.com|yahoo.com|yahoo.co|outlook.com|outlook.co|hotmail.com|live.com|live.co|msn.com|126.com|163.com|vip.163.com|aol.com|vip.sina.com|popmail.com|gmx.com|inbox.com|qq.com|";
            if (emails.Contains(subfix))
                return subfix.TrimEnd('|').TrimStart('|');
            else
                return "";
        }

        #region 获取随机数
        /// <summary>
        /// 获取随机数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetNumCode(int num)
        {
            string a = "0123456789";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获得一个17位时间随机数
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataRandom()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// 获得一个16位时间随机数
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDateRandom()
        {
            string strData = System.DateTime.Now.Year.ToString() +
            System.DateTime.Now.Month.ToString() +
            System.DateTime.Now.Day.ToString() +
            System.DateTime.Now.Hour.ToString() +
            System.DateTime.Now.Minute.ToString() +
            System.DateTime.Now.Second.ToString() +
            System.DateTime.Now.Millisecond.ToString();

            //Random r = new Random();
            //strData = strData + r.Next(1000);
            return strData;
        }

        /// <summary>
        /// 获得一个9位时间随机数(小时)
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataShortRandom()
        {
            return System.DateTime.Now.ToString("HHmmssfff");
        }

        /// <summary>
        /// 获取随即数.a-z,0-9
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRadmonString(int len)
        {
            Random random = new Random();
            var _chars = new char[36];
            for (int i = 65; i <= 90; i++)
            {
                _chars[i - 65] = (char)i;
            }
            for (int i = 48; i < 58; i++)
            {
                _chars[i - 22] = (char)(i);
            }
            string str = string.Empty;
            for (int i = 0; i < len; i++)
            {
                str += _chars[random.Next(0, 35)];
            }
            return str;
        }

        /// <summary>
        /// 获取随机数(解决随机数BUG,实例放到ThreadLocal中,保证每个线程一个Randonm实例)
        /// </summary>
        private static readonly ThreadLocal<Random> appRandom = new ThreadLocal<Random>(() => new Random());
        public static int GetRandomNumber()
        {
            return appRandom.Value.Next();
        }

        public static int GetRandomNumber(int num)
        {
            return appRandom.Value.Next(num);
        }

        public static int GetRandomNumber(int minNum, int maxNum)
        {
            return appRandom.Value.Next(minNum, maxNum);
        }
        #endregion

        #region  将A~Z转换为数字
        /// <summary>
        /// 将A~Z转换为数字
        /// </summary>
        /// <param name="titleIndex"></param>
        /// <returns></returns>
        public static byte TransformTitleIndex(string titleIndex)
        {
            switch (titleIndex)
            {
                case "A":
                    return 10;
                case "B":
                    return 11;
                case "C":
                    return 12;
                case "D":
                    return 13;
                case "E":
                    return 14;
                case "F":
                    return 15;
                case "G":
                    return 16;
                case "H":
                    return 17;
                case "I":
                    return 18;
                case "J":
                    return 19;
                case "K":
                    return 20;
                case "L":
                    return 21;
                case "M":
                    return 22;
                case "N":
                    return 23;
                case "O":
                    return 24;
                case "P":
                    return 25;
                case "Q":
                    return 26;
                case "R":
                    return 27;
                case "S":
                    return 28;
                case "T":
                    return 29;
                case "U":
                    return 30;
                case "V":
                    return 31;
                case "W":
                    return 32;
                case "X":
                    return 33;
                case "Y":
                    return 34;
                case "Z":
                    return 35;
                default:
                    return 0;
            };
        }
        /// <summary>
        /// 是否全角
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsFullWidth(char c)
        {
            return System.Text.Encoding.Default.GetByteCount(c.ToString()) == 2;
        }

        /// <summary>
        /// 将时间转换为INT
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetDateInt(string time)
        {
            return (!string.IsNullOrEmpty(time)) ? time.Replace("-", "").ToInt32() : 0;
        }

        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetTitleIndex(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                string firstChar = Chinese.Get(title.Replace("供应", "").Substring(0, 1)).ToUpper().Trim();
                if (!string.IsNullOrEmpty(firstChar))
                {
                    char index = Convert.ToChar(firstChar);
                    if (IsFullWidth(index))
                    {
                        char result = (char)(index - 65248);
                        return result.ToString();
                    }
                    return firstChar;
                }
            }
            return "";
        }
        /// <summary>
        /// 获取转换后的字母INT
        /// </summary>
        /// <param name="tilteIndex"></param>
        /// <returns></returns>
        public static byte GetTitleIndexInt(string title)
        {
            string titleIndex = GetTitleIndex(title);
            if (!string.IsNullOrEmpty(titleIndex))
            {
                if (Tools.StrClass.StringCheck.IsInteger(titleIndex))
                {
                    return Convert.ToByte(titleIndex);
                }
                return TransformTitleIndex(titleIndex);
            }
            return 100;
        }

        /// <summary>
        /// 获取相差时间数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type">D 天数 M 月 H 小时</param>
        /// <returns></returns>
        public static int GetDateTimeDiff(string start, string end, string type)
        {
            DateTime dtEnd = end.ToDateTime();
            DateTime dtStart = start.ToDateTime();
            if (type == "D")
                return (dtEnd - dtStart).Days + 1;
            else if (type == "M")
                return dtEnd.Year * 12 + dtEnd.Month - (dtStart.Year * 12 + dtStart.Month) + 1;
            else if (type == "H")
            {
                TimeSpan ts = dtEnd - dtStart;
                return ts.Days * 12 + ts.Hours + 1;
            }
            return 0;
        }
        #endregion
    }
}
