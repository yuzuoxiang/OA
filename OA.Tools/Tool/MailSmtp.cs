using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.Tool
{ 
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class MailSmtp
    {
        #region 属性
        /// <summary>
        /// 设置邮件编码
        /// </summary>
        public void SetEncoding(Encoding contentEncoding)
        {
            _encoding = contentEncoding;
        }

        /// <summary>
        /// 设置邮件正文是否为 Html 格式 ,默认ture
        /// </summary>
        public void SetIsHtml(bool isHtml)
        {
            _isHtml = isHtml;
        }

        /// <summary>
        /// 抄送
        /// </summary>
        public void SetCC(params string[] cc)
        {
            _cc = cc;
        }

        /// <summary>
        /// 暗送
        /// </summary>
        public void BCC(params string[] bcc)
        {
            _bcc = bcc;
        }

        /// <summary>
        /// 是否SSL加密
        /// </summary>
        public void SetIsSSL(bool isSSL)
        {
            smtp.EnableSsl = isSSL;
        }

        private Encoding _encoding { get; set; }
        private bool _isHtml { get; set; }
        private string[] _cc { get; set; }
        private string[] _bcc { get; set; }
        public string Result { get; private set; }

        #endregion
        private SmtpClient smtp = new SmtpClient();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username">邮件账号</param>
        /// <param name="password">密码</param>
        public MailSmtp(string host, string username, string password)
        {
            smtp.Host = host;
            smtp.Port = 0x19;
            smtp.EnableSsl = false;
            _isHtml = true;
            _encoding = Encoding.UTF8;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                smtp.UseDefaultCredentials = false;
            }
            else
            {
                smtp.Credentials = new NetworkCredential(username, password);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="file">附件地址数组</param>
        /// <returns>bool 是否成功</returns>
        public bool Send(string from, string sender, string to, string subject, string body, params string[] file)
        {
            return Send(from, sender, new string[] { to }, subject, body, file);
        }

        /// <summary>
        /// 发送邮件，多个收件人
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="file">附件地址数组</param>
        /// <returns>bool 是否成功 </returns>
        private bool Send(string from, string sender, string[] to, string subject, string body, params string[] file)
        {
            if (!Validate(to))
                return false;

            MailMessage message = new MailMessage();

            //创建一个附件对象
            foreach (var r in file)
            {
                Attachment attachment = new Attachment(r);//发送邮件的附件
                message.Attachments.Add(attachment);
            }
            message.From = new MailAddress(from, sender);
            message.Subject = subject;
            message.SubjectEncoding = _encoding;
            message.Body = body;
            message.BodyEncoding = _encoding;
            message.IsBodyHtml = _isHtml;
            message.Priority = MailPriority.Normal;
            foreach (string str in to)
                message.To.Add(str);

            if (_bcc != null && _bcc.Length > 0)
            {
                foreach (string b in _bcc)
                    message.Bcc.Add(b);
            }

            if (_cc != null && _cc.Length > 0)
            {
                foreach (string c in _cc)
                    message.CC.Add(c);
            }

            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            return false;
        }
        
        private bool Validate(string[] to)
        {
            string mailReg = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            if (to == null)
                throw new ArgumentNullException("MailSmtp.Send.to");

            if (to.Any(o => !Regex.IsMatch(o + "", mailReg)))
            {
                Result = "收件人地址不合法";
                return false;
            }

            if (_bcc != null && _bcc.Length > 0)
            {
                if (_bcc.Any(o => !Regex.IsMatch(o + "", mailReg)))
                {
                    Result = "暗送人地址不合法";
                    return false;
                }
            }
            return true;
        }
    }
}

//声名对象
//MailSmtp ms = new MailSmtp("smtp.qq.com","xxx","xx");

//可选参数
//ms.SetCC("xxxx@qq.com");//抄送可以多个
//ms.SetBC("xxx@qq.com");//暗送可以多个
//ms.SetIsHtml(true);//默认:true
//ms.SetEncoding(System.Text.Encoding.UTF8);//设置格式 默认utf-8
//ms.SetIsSSL(true);//是否ssl加密 默认为false

//调用函数
//bool isSuccess = ms.Send("xx@qq.com", "test", "xxxxqq.com", "哈哈", "哈哈");

//获取返回值
//var result=ms.Result