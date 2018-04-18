using System;
using System.Text.RegularExpressions;

namespace Tools.StrClass
{
    /// <summary>
    /// 字符串验证
    /// </summary>
    public class StringCheck
    {
        #region 检验常用输入格式
        /// <summary>
        /// 验证Emal地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            Regex reg = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(email);
        }

        /// <summary>
        /// 验证是否是合法手机号码，只要11位就行
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            Regex reg = new Regex(@"^1+\d{10}$");
            return reg.IsMatch(mobile);
        }

        /// <summary>
        /// 验证是否是合格电话号码 (023)245554332-312 | 023-245554332-312
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhone(string phone)
        {
            Regex reg = new Regex(@"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$");
            return reg.IsMatch(phone);
        }

        /// <summary>
        /// 验证是否是邮编
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public static bool IsPostCode(string postCode)
        {
            Regex reg = new Regex(@"^\d{6}$");
            return reg.IsMatch(postCode);
        }

        /// <summary>
        /// 验证是否为密码6-20位任意字符
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool IsPwd(string pwd)
        {
            Regex reg = new Regex(@"^\S{6,20}$");
            return reg.IsMatch(pwd);
        }

        /// <summary>
        /// 验证是否是方法名称，过滤XSS使用
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool IsFunction(string func)
        {

            Regex reg = new Regex(@"^[\w\d\._]+$");
            return reg.IsMatch(func);
        }

        /// <summary>
        /// 验证是否是IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(string ip)
        {
            Regex reg = new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
            return reg.IsMatch(ip);
        }

        /// <summary>
        /// 是否是Guid
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static bool IsGuid(string strSrc)
        {
            try
            {
                Guid guid = new Guid(strSrc);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否合法商铺二级域名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsShopUrl(string str)
        {
            Regex reg = new Regex(@"^[0-9a-zA-Z_\-]{3,20}$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 验证是否是QQ号码 5位数以上
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static bool IsQQ(string qq)
        {
            Regex reg = new Regex(@"^[1-9]\d{4,9}$");
            return reg.IsMatch(qq);
        }

        /// <summary>
        /// 检测用户名格式
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsUserName(string userName)
        {
            Regex reg = new Regex("^[a-zA-Z]{1}[0-9a-zA-Z_]{5,24}$");
            return reg.IsMatch(userName);
        }

        /// <summary>
        /// 是否存在中文字符
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static bool IsHaveChinaChar(string CString)
        {
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) > Convert.ToInt32(Convert.ToChar(128)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为DZSC网站文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDzscFile(string path)
        {
            Regex reg = new Regex(@"^http(s)?://[\w-\.]*?\.dzsc\.com[\w\./]+?$");
            return reg.Match(path).Success;
        }

        /// <summary>
        /// MSN
        /// </summary>
        /// <param name="msn"></param>
        /// <returns></returns>
        public static bool IsMSN(string msn)
        {
            Regex reg = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?(\/((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?){0,2}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(msn);
        }

        /// <summary>
        /// 是否为URL地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            Regex reg = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        public static bool IsPartNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex(@"^[0-9a-zA-Z#][0-9a-zA-Z-_\+\/\.\\ \(\)#\=%\,:]{2,49}$");
            return objNotNaturalPattern.IsMatch(strNumber);
        }
        #endregion

        #region 检验数字格式
        /// <summary>
        /// 验证是否整数 可为负 (+/-)123
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^[\+\-]{0,1}\d+$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 验证是否是正整数 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIntegerByPositive(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^\d+$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 是否是浮点数 可为负 (+/-)123.31
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^[\-\+]{0,1}\d+(\.\d+)?$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 是否是正浮点数 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloatByPositive(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^\d+(\.\d+)?$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 是否是价格 小数点后保留后两位 包括正或负
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPrice(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^[-+]?(\d{1,}|\d{1,}\.\d{1,2}0{0,})$");
            return reg.Match(str).Success;
        }
        #endregion

        #region 时间验证
        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime2(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}$");
            return reg.IsMatch(date);
        }
        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm:ss (精确到秒)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime(string date)
        {
            DateTime t = DateTime.Now;
            if (DateTime.TryParse(date, out t))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime3(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(2[0123]|1[0-9]|0?[0-9]){1}:(0?[0-9]|[1-5][0-9]|60){1}$");
            return reg.IsMatch(date);
        }
        
        /// <summary>
        /// 严格判断时间格式为 yyyy-MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateYM(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])$");
            return reg.IsMatch(date);
        }
       
        /// <summary>
        /// 严格判断时间格式为 yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateYMD(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$");
            return reg.IsMatch(date);
        }
        
        #endregion
    }
}
