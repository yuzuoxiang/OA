using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.Areas.Admin.Controllers
{
    public class AdminInfoController : Base.BaseAdminController
    {
        public ActionResult Edit()
        {
            var user = Bll.BllMng_User.Instance().First(o => o.Id == MyInfo.Id);
            return View(user);
        }

        //[PermissionFilter("/admin/admininfo/edit")]
        public ActionResult Save()
        {
            string password = RequestString("password").Trim();
            string rPassword = RequestString("rpassword").Trim();
            string realname = RequestString("realname").Trim();
            string email = RequestString("email").Trim();
            string qq = RequestString("qq").Trim();
            string tel = RequestString("tel").Trim();
            string pic = GetPic();

            if (password != "")
            {
                if (password != rPassword)
                    return MessageBoxAndReturn("两次输入密码不同！");
                MyInfo.Password = Tools.Encryption.EncryptionHelper.Md5(password);
            }
            MyInfo.Pic = pic;
            MyInfo.RealName = realname;
            MyInfo.QQ = qq;
            MyInfo.Tel = tel;
            MyInfo.Email = email;

            if (Bll.BllMng_User.Instance().Update(MyInfo, o => o.Id == MyInfo.Id))
                return MessageBoxAndJump("保存成功！", "/admin/admininfo/edit");
            else
                return MessageBoxAndReturn("保存失败！");
        }
    }
}