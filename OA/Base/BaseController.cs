﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Tools;

namespace OA.Base
{
    public class BaseController : Controller
    {
        public static string cssVersion = DateTime.Now.ToString("yyMMddhhss"); //样式版本
        public static string domain = Tools.Special.Common.GetDomain();
        public static string webUrl = Tools.Special.Common.GetWebUrl();

        public bool IsAjax
        {
            get { return Request.IsAjaxRequest(); }
        }

        #region 全局Action管道
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //executingContext.RequestContext.HttpContext.Request.ContentEncoding = System.Text.Encoding.UTF8;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //在这里可以处理最终输出到浏览器的html
            //string html = RenderViewToString(this, ((ViewResult)executedContext.Result).View);
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 将视图渲染为字符串
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        protected static string RenderViewToString(Controller controller, IView view)
        {
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        #endregion

        #region Request方法
        /// <summary>
        /// 请求一个字符串，返回null时返回空字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string RequestString(string name, string defaultValue = "")
        {
            string v = Request[name];
            return v == null ? defaultValue : v.Trim();
        }

        /// <summary>
        /// Request一个数字
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int RequestInt(string name, int defaultValue = 0)
        {
            string v = RequestString(name);
            return v == null ? 0 : v.ToInt32(defaultValue);
        }

        /// <summary>
        /// Request一个Byte
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte RequestByte(string name)
        {
            string v = RequestString(name);
            return v == null ? (byte)0 : v.ToByte();
        }

        /// <summary>
        /// Request一个Decimal
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public decimal RequestDecimal(string name)
        {
            string v = RequestString(name);
            return v == null ? 0 : v.ToDecimal();
        }

        /// <summary>
        /// Request一个布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RequestBool(string name)
        {
            string v = Request[name];
            return v.ToBool();
        }

        /// <summary>
        /// Request一个Double
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double RequestDouble(string name)
        {
            string v = RequestString(name);
            return v.ToDouble();
        }

        /// <summary>
        /// Request一个Float
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double RequestFloat(string name)
        {
            string v = RequestString(name);
            return v.ToFloat();
        }
        #endregion

        #region 常用
        /// <summary>
        /// 输出Nofollow标签
        /// </summary>
        /// <param name="nofollowFlag"></param>
        /// <returns></returns>
        public string ResponseNoFollow(bool nofollowFlag)
        {
            return Tools.StrClass.StringHtml.ResponseNoFollow(nofollowFlag);
        }

        public ActionResult ResponseWrite(string value)
        {
            return Content(value);
        }

        public ActionResult ResponseRedirect(string url, string msg)
        {
            return Redirect(url + "?msg=" + msg);
        }

        public ActionResult RedirectToLogin(string returnurl)
        {
            return RedirectToAction("login", "auth", new { returnurl = returnurl });
        }

        public ActionResult RedirectToLogin()
        {
            return RedirectToAction("login", "auth");
        }

        /// <summary>
        /// 301跳转
        /// </summary>
        /// <param name="url">跳转地址</param>
        /// <param name="cache">是否本地缓存</param>
        public ActionResult Response301(string url, bool cache)
        {
            Response.StatusCode = 301;
            Response.Status = "301 Moved Permanently";
            Response.AppendHeader("Location", url);
            Response.AppendHeader("Cache-Control", cache ? "cache" : "no-cache"); //不做本地缓存
            Response.End();
            return View();
        }

        /// <summary>
        /// 302跳转
        /// </summary>
        /// <param name="url">跳转地址</param>
        /// <param name="cache">是否本地缓存</param>
        public ActionResult Response302(string url, bool cache)
        {
            Response.StatusCode = 302;
            Response.Status = "302 Found";
            Response.AppendHeader("Location", url);
            Response.AppendHeader("Cache-Control", cache ? "cache" : "no-cache"); //不做本地缓存
            Response.End();
            return View();
        }

        /// <summary>
        /// 404跳转
        /// </summary>
        /// <returns></returns>
        public ActionResult Response404()
        {
            Response.Clear();
            Response.StatusCode = 404;
            Response.Status = "404 Not Found";
            Response.AppendHeader("Cache-Control", "no-cache");
            Response.RedirectPermanent("/error/404.html");
            Response.End();
            return View();
        }

        /// <summary>
        /// (已检测)获取客户端IP
        /// </summary>
        /// <returns></returns>
        public string GetRemoteIp()
        {
            string result = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 格式化标题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="titleStyle"></param>
        /// <returns></returns>
        public static string FormatTitle(string title, string titleStyle)
        {
            string val = title;
            string[] titleStyles = titleStyle.Split('|');
            if (titleStyles.Length == 3)
            {
                if (titleStyles[0] != "0") { val = "<font color='" + titleStyles[0] + "'>" + val + "</font>"; }
                if (titleStyles[1] != "0") { val = "<b>" + val + "</b>"; }
                if (titleStyles[2] != "0") { val = "<u>" + val + "</u>"; }
            }
            return val;
        }

        /// <summary>
        /// 返回中文性别
        /// </summary>
        /// <param name="sex">Bool类型性别</param>
        /// <returns>男(false)或女(true)</returns>
        public static string GetChineseSex(bool sex)
        {
            return sex ? "男" : "女";
        }

        /// 返回表单下拉菜单或选框的还原项
        /// </summary>
        /// <param name="selValue">设置的值</param>
        /// <param name="thisValue">表单框值</param>
        /// <param name="isSelectBox">是否是下拉菜单，否则是复选框</param>
        /// <returns>下拉菜单选中返回selected ,选框选中返回checked</returns>
        public string ReturnSel(string selValue, string thisValue, bool isSelectBox)
        {
            if (string.IsNullOrEmpty(selValue) || string.IsNullOrEmpty(thisValue))
                return "";
            if (selValue == thisValue)
                return isSelectBox ? " selected " : " checked ";
            else
                return "";
        }

        /// <summary>
        /// 返回前一页
        /// </summary>
        public void ReturnRefferPage()
        {
            Response.Redirect(RefferPageUrl());
            Response.End();
        }

        /// <summary>
        /// 获取前导页
        /// </summary>
        /// <returns></returns>
        public string RefferPageUrl()
        {
            try
            {
                return HttpUtility.UrlDecode(Request.UrlReferrer.AbsoluteUri, System.Text.Encoding.GetEncoding("GB2312"));
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region MessageBox 对话框
        public ContentResult MessageBoxAndReturn(string message)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.history.back(-1);</script>", message.Replace("'", @"\'"));
            return Content(script);
        }

        public ContentResult MessageBoxAndJump(string message, string jumpPage)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.location.href='{1}';</script>", message.Replace("'", @"\'"), jumpPage.Replace("'", @"\'"));
            return Content(script);
        }

        public ContentResult MessageBoxAndReload(string message)
        {
            string script = "<script type=\"text/javascript\">parent.location.reload();</script>";
            return Content(script);
        }

        public ContentResult MessageBoxAndJump(string message, string action, string controller, object routevalues = null)
        {
            string param = string.Empty;
            if (routevalues != null)
            {
                Type t = routevalues.GetType();
                var typeArr = t.GetProperties();
                foreach (var pi in typeArr)
                {
                    //当属性为字符串时
                    if (pi.PropertyType == typeof(string))
                    {
                        string name = pi.Name;
                        object value = pi.GetValue(routevalues, null);
                        param += name + "=" + value.ToString() + "&";
                    }
                }
            }
            if (param != "")
                action += "?" + param.TrimEnd('&');
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.location.href='/{1}/{2}';</script>", message, controller, action);
            return Content(script);
        }

        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="message">对话框内容</param>
        public ContentResult MessageBox(string message)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');</script>", message.Replace("'", @"\'"));
            return Content(script);
        }

        /// <summary>
        /// 弹出对话框并关闭页面。
        /// </summary>
        /// <param name="message">对话框内容</param>
        public ContentResult MessageBoxAndClose(string message)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.close();</script>", message.Replace("'", @"\'"));
            return Content(script);
        }

        #endregion

        #region Layer Msg操作

        public ContentResult LayerMsg(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.msg('" + message.Replace("'", @"\'") + "');</script>";
            return Content(script);
        }

        public ContentResult LayerMsgSuccess(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.msg('" + message.Replace("'", @"\'") + "', {icon: 1});</script>";
            return Content(script);
        }

        public ContentResult LayerMsgSuccessAndRefresh(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.msg('" + message.Replace("'", @"\'") + "', {icon: 1});parent.$table.bootstrapTable('refresh');</script>";
            return Content(script);
        }

        public ContentResult LayerMsgSuccessAndRefreshPage(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.msg('" + message.Replace("'", @"\'") + "', {icon: 1});parent.ReloadPage();</script>";
            return Content(script);
        }

        public ContentResult LayerMsgSuccessAndRedirect(string message, string url)
        {
            string script = "<script type=\"text/javascript\">parent.layer.msg('" + message.Replace("'", @"\'") + "', {icon: 1,success:function(layero, index){window.location.replace('" + url + "');}});</script>";
            return Content(script);
        }

        public ContentResult LayerMsgErrorAndReturn(string message)
        {
            string script = "<script type=\"text/javascript\">window.history.back(-1);parent.layer.msg('" + message.Replace("'", @"\'") + "', {icon: 2});</script>";
            return Content(script);
        }

        public ContentResult LayerMsgErrorAndClose(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.msg('" + message.Replace("'", @"\'") + "', {icon: 2});</script>";
            return Content(script);
        }

        public ContentResult LayerMsgAndCallback(string message, string callback)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.msg('" + message.Replace("'", @"\'") + "', parent." + callback + ");</script>";
            return Content(script);
        }

        public ContentResult LayerClose()
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);</script>";
            return Content(script);
        }

        #endregion

        #region Layer Alert操作

        public ContentResult LayerAlert(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.alert('" + message.Replace("'", @"\'") + "');</script>";
            return Content(script);
        }

        public ContentResult LayerAlertSuccess(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 1});</script>";
            return Content(script);
        }

        public ContentResult LayerAlertSuccessAndRefresh(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 1});parent.$table.bootstrapTable('refresh');</script>";
            return Content(script);
        }

        public ContentResult LayerAlertSuccessAndRefreshPage(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 1});parent.ReloadPage();</script>";
            return Content(script);
        }

        public ContentResult LayerAlertSuccessAndRedirect(string message, string url)
        {
            string script = "<script type=\"text/javascript\">parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 1,btn:['确认'],yes:function(index){parent.layer.close(index);window.location.replace('" + url + "');}});</script>";
            return Content(script);
        }

        public ContentResult LayerAlertErrorAndReturn(string message)
        {
            string script = "<script type=\"text/javascript\">window.history.back(-1);parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 5});</script>";
            return Content(script);
        }

        public ContentResult LayerAlertErrorAndClose(string message)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 5});</script>";
            return Content(script);
        }

        public ContentResult LayerAlertAndCallback(string message, string callback)
        {
            string script = "<script type=\"text/javascript\">var w = parent.layer.getFrameIndex(window.name);parent.layer.close(w);parent.layer.alert('" + message.Replace("'", @"\'") + "',parent." + callback + ");</script>";
            return Content(script);
        }

        #endregion

        #region 页面方法转换

        /// <summary>
        /// 格式化成Url
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetFormatUrl(string title)
        {
            return Tools.Usual.Utils.ConverUrl(title).ToLower();
        }

        /// <summary>
        /// 去除Html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            return Tools.StrClass.StringHtml.RemoveHtml(str);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(string str, int length)
        {
            return Tools.StrClass.StringProcess.CutString(str, length);
        }
        /// <summary>
        /// 判断文件后缀是否是图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImg(string fileName)
        {
            return Tools.Http.HtmlCommon.IsImg(fileName);
        }

        /// <summary>
        /// 获取文件后缀
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            return Tools.Http.HtmlCommon.GetFileExt(fileName);
        }

        /// <summary>
        /// 格式化产品参数
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static string FormatOrderAttrDetail(string detail)
        {
            return detail.Replace(";", "\r\n");
        }

        /// <summary>
        /// Email格式化
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string FormatEmail(string email)
        {
            string[] t = email.Split('@');
            string result = string.Empty;
            result = t[0].Substring(0, 2) + "****@***.com";
            return result;
        }

        /// <summary>
        /// 文本Texteara显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ShowTextarea(string str)
        {
            return Tools.StrClass.StringHtml.ToTexteara(str);
        }

        /// <summary>
        /// 根据条件筛选列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected static DataTable GetSelect(DataTable dt, string where)
        {
            return Tools.Usual.Utils.GetSelect(dt, where);
        }

        /// <summary>
        /// 获取唯一数据的字段
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PrimaryKeyColumns"></param>
        /// <returns></returns>
        protected static DataTable GetDistinctPrimaryKeyColumnTable(DataTable dt, string[] PrimaryKeyColumns)
        {
            return Tools.Usual.Utils.GetDistinctPrimaryKeyColumnTable(dt, PrimaryKeyColumns);
        }

        /// <summary>
        /// 去除html内容里的图片高宽样式
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static string RemoveImageStyle(string detail)
        {
            Regex regWidth = new Regex("(<img[^>]*?)\\s+width\\s*=\\s*\\S+", RegexOptions.IgnoreCase);
            Regex regHeight = new Regex("(<img[^>]*?)\\s+height\\s*=\\s*\\S+", RegexOptions.IgnoreCase);
            Regex regStyle = new Regex("(<img[^>]*?)\\s+style\\s*=\\s*\\S+", RegexOptions.IgnoreCase);

            return regWidth.Replace(regHeight.Replace(regStyle.Replace(detail, "$1"), "$1"), "$1");
        }

        /// <summary>
        /// 获取html中所有图片的路径
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetImageUrlListFromHtml(string htmlText)
        {
            return Tools.StrClass.StringHtml.GetImageUrlListFromHtml(htmlText);
        }

        /// <summary>
        /// 获取html中所有a标签的链接地址
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetLinkUrlListFromHtml(string htmlText)
        {
            return Tools.StrClass.StringHtml.GetLinkUrlListFromHtml(htmlText);
        }
        #endregion
    }
}