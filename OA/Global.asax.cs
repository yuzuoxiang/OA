﻿using OA.App_Start;
using OA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Tools.Logs.Log4netExt;

namespace OA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        IWebLog WebLog = WebLogHelper.GetLogger("oa_error");

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //注册AutoMapper
            //Mapper.ProfileRegister.Register();

            //加载常用数据
            //DepartmentData.Load();
            //GroupData.Load();
            AdminData.Load();
        }

        //让WebApi支持Session
        public override void Init()
        {
            PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
        //End


        //无法找到资源事件(404)
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception objExp = HttpContext.Current.Server.GetLastError();
            WebLog.Error(Request.UserHostAddress, Request.Url.ToString(), objExp.Message.ToString(), objExp, 0, "");
            Response.Redirect("/home/error?msg=" + objExp.Message.ToString());
        }
    }
}
