using OA.Base;
using OA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Controllers
{
    [CheckLogin]
    public class HomeController : BaseAdminController
    {
        // GET: Home
        public ActionResult Index()
        {
            var adminInfo = Bll.BllMng_User.Instance().First(o => o.Id == MyInfo.Id);
            return View(adminInfo);
        }

        public ActionResult Default()
        {
            var adminInfo = Bll.BllMng_User.Instance().First(o => o.Id == MyInfo.Id);
            //ViewBag.role = string.Join(",", Bll.BllMng_PermissionGroup.GetGruopNames(adminInfo.GroupManage, MyDepartId););
            return View(adminInfo);
        }

        public ActionResult Error()
        {
            ViewBag.msg = RequestString("msg");
            return View("error");
        }

        public ActionResult UnAuthorized()
        {
            return View("unauthorized");
        }
    }
}