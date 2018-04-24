using OA.Model;
using SqlSugar;
using SqlSugarTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Bll
{
    public class BllMng_User : BllCommonBase<Mng_User>
    {
        private static readonly string DbName = "OA";

        public BllMng_User() : base(DbName)
        {
        }

        public static BllMng_User Instance()
        {
            return new BllMng_User();
        }

        /// <summary>
        /// 获取当前用户所有权限Id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public List<int> GetPermissionIds(Mng_User model)
        {
            if (model != null && model.Id > 0)
            {
                string sql = "select Distinct PermissionId from ( ";
                sql += "select PermissionId from [dbo].[Mng_PermissionPersonSet] where AdminId = @adminId ";
                if (model.GroupId != null && model.GroupId > 0)
                {
                    sql += " union select PermissionId from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id = @groupId and belock = cast(0 as bit))";
                }
                sql += " ) as tb";

                var db = SugarDao.GetInstance(DbName);
                List<int> ids = db.Ado.SqlQuery<int>(sql, new { adminId = model.Id, groupId = model.GroupId }).ToList();

                return ids;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前用户所有权限Url
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<string> GetPermissionUrls(Mng_User model)
        {
            if (model == null || model.Id < 1)
                return null;

            if (model != null && model.Id > 0)
            {
                string sql = "select Distinct Url from ( ";
                sql += "select PermissionId from [dbo].[Mng_PermissionPersonSet] where AdminId = @adminId ";
                if (model.GroupId != null && model.GroupId > 0)
                {
                    sql += " union select PermissionId from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id = @groupId and belock = cast(0 as bit))";
                }
                sql += " ) a join [dbo].[Mng_MenuClass] b on a.PermissionId = b.Id where Url<>''";

                var db = SugarDao.GetInstance(DbName);
                List<string> urls = db.Ado.SqlQuery<string>(sql, new { adminId = model.Id, groupId = model.GroupId }).ToList();
                return urls;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="loginIp">登录IP</param>
        /// <returns>成功返回用户ID，失败返回0</returns>
        public Mng_User CheckLogin(string userName, string password, string loginIp)
        {
            try
            {
                Mng_User modMng_User = null;
                int result = CheckLogin(userName, Tools.Encryption.EncryptionHelper.Md5(password, 32));
                if (result > 0)
                {
                    //登录成功，保存登录信息
                    modMng_User = First(o => o.Id == result);
                    if (modMng_User != null)
                    {
                        modMng_User.LoginIp = loginIp;
                        modMng_User.LoginTime = DateTime.Now;
                        modMng_User.LoginTimes = modMng_User.LoginTimes + 1;
                        Update(modMng_User, o => o.Id == result);
                    }
                }
                return modMng_User;
            }
            catch (Exception ex)
            {
                Tools.Logs.Log4netExt.LogHelper.WriteLog(ex, 0, "");
                return null;
            }
        }

        public int CheckLogin(string userName, string password)
        {
            var result = FirstSelect(o => o.UserName == userName && o.Password == password && o.InJob == true, "Id")?.Id ?? 0;
            return result;
        }

        /// <summary>
        /// 增加职员
        /// </summary>
        /// <param name="Mng_User"></param>
        /// <returns>0: 出错，-1 存在用户名 ,1 成功</returns>
        public int AddNoReturn(Mng_User model)
        {

            if (ExistsUserName(model.UserName))
            {
                return -1;
            }
            else
            {
                if (Insert(model))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateInfo(Mng_User model, int id)
        {
            var result = FirstSelect(o => o.UserName == model.UserName && o.Id != id, "Id")?.Id ?? 0;
            if (result > 0)
            {
                return -1;
            }
            else
            {
                if (Update(model, o => o.Id == id))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// 判断是否存在此用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public bool ExistsUserName(string userName)
        {
            var result = FirstSelect(o => o.UserName == userName, "Id")?.Id ?? 0;
            return result > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deleteIds">删除的ID( ,分割)</param>
        /// <returns>删除成功条数</returns>
        public int DeleteBatch(string deleteIds)
        {
            int delNum = 0;
            if (deleteIds.Trim() == "") { return 0; }
            string[] ids = deleteIds.Split(',');

            foreach (string idStr in ids)
            {
                if (Delete(int.Parse(idStr)))
                {
                    delNum++;
                }
            }
            return delNum;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="searchType">搜索字段</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="records">返回总记录数</param>
        /// <returns>返回记录集</returns>
        public DataTable PageListManage(string searchType, string keyword, int page, int pageSize, ref int records)
        {
            StringBuilder sql = new StringBuilder("1=1");
            if (!string.IsNullOrEmpty(searchType) && !string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and " + CheckSqlValue(searchType) + " like '" + CheckSqlKeyword(keyword) + "%'");
            }
            //return GetPageList2("dbo.Mng_User", "id,UserName,RealName,Sex,logintime,logintimes,injob,departid", sql.ToString(), "order by UserName", pageSize, page, ref records).Tables[0];

            var db = SugarDao.GetInstance(DbName);
            StoredProcedure<DataTable>("Sys_Page2", new SugarParameter[]{
                new SugarParameter("@tb","dbo.Mng_User",typeof(String)),
                new SugarParameter("@collist","id,UserName,RealName,Sex,logintime,logintimes,injob,departid",typeof(String)),
                new SugarParameter("@where",sql.ToString(),typeof(String)),
                new SugarParameter("@orderby","order by UserName",typeof(String)),
                new SugarParameter("@pagesize",pageSize,typeof(String)),
                new SugarParameter("@page",page,typeof(String)),
                new SugarParameter("@records",records,true)
            });

            return new DataTable();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public IEnumerable<Mng_User> PageList(string userName, string realName, int departId, int groupId, int page, int pageSize, ref int records)
        {
            var db = SugarDao.GetInstance(DbName);

            return db.Queryable<Mng_User, Mng_DepartmentClass, Mng_PermissionGroup>((u, d, p) => new object[] {
                JoinType.Left,u.DepartId==d.Id,
                JoinType.Left,u.GroupId==p.Id
            })
            .WhereIF(!string.IsNullOrEmpty(userName), u => SqlFunc.StartsWith(u.UserName, userName))
            .WhereIF(!string.IsNullOrEmpty(realName), u => SqlFunc.StartsWith(u.RealName, realName))
            .WhereIF(departId > 0, u => u.DepartId == departId)
            .WhereIF(groupId > 0, u => u.GroupId == groupId)
            .Select((u, d, p) => new Mng_User
            {
                Id = u.Id,
                UserName = u.UserName,
                RealName = u.RealName,
                Sex = u.Sex,
                LoginTime = u.LoginTime,
                LoginTimes = u.LoginTimes,
                InJob = u.InJob,
                DepartId = u.DepartId,
                GroupId = u.GroupId,
                ParUserId = u.ParUserId,
                ClassName = d.ClassName,
                GroupName = p.GroupName
            })
            .ToPageList(page, pageSize, ref records);
        }

        /// <summary>
        /// 设置是否在职
        /// </summary>
        /// <param name="id">设置ID</param>
        /// <returns>是否成功</returns>
        public bool SetInJob(int id)
        {
            Mng_User modMng_User = First(o => o.Id == id);
            if (modMng_User != null)
            {
                Update(new Mng_User { Id = id, InJob = !modMng_User.InJob }, o => o.Id == id);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取姓名
        /// </summary>
        /// <param name="id"></param>
        /// <returns>出错返回error</returns>
        public string GetNameById(int id)
        {
            if (id == 0) return "";
            Mng_User model = First(o => o.Id == id);
            if (model == null)
            {
                return "";
            }
            else
            {
                return model.RealName;
            }

        }

        /// <summary>
        /// 获取所有人员
        /// </summary>
        /// <param name="onlyInjob"></param>
        /// <returns></returns>
        public IEnumerable<Mng_User> GetAllList(bool onlyInjob, int departId = 0)
        {
            var db = SugarDao.GetInstance(DbName);

            return db.Queryable<Mng_User>()
                .WhereIF(onlyInjob, o => o.InJob == true)
                .WhereIF(departId > 0, o => o.DepartId == departId)
                .OrderBy(o => o.Id)
                .ToList();
        }

        /// <summary>
        /// 获取子级人员
        /// </summary>
        /// <param name="parUserId"></param>
        /// <returns></returns>
        public List<Mng_User> GetChildList(int parUserId)
        {
            if (parUserId == 0)
                return null;

            var db = SugarDao.GetInstance(DbName);

            return db.Queryable<Mng_User>()
                .Where(o => o.ParUserId == parUserId && o.Id != parUserId && o.InJob == true)
                .OrderBy(o => o.Id)
                .ToList();
        }

        /// <summary>
        /// 指定部门下员工列表
        /// </summary>
        /// <param name="departId"></param>
        /// <param name="thisId"></param>
        /// <param name="onlyInjob"></param>
        /// <returns></returns>
        public List<Mng_User> GetListByDepart(int departId, int thisId, bool onlyInjob)
        {
            var db = SugarDao.GetInstance(DbName);

            return db.Queryable<Mng_User>()
                .WhereIF(onlyInjob, o => o.InJob == true)
                .WhereIF(thisId > 0, o => o.Id != thisId)
                .WhereIF(departId > 0, o => o.DepartId == departId)
                .OrderBy(o => o.Id)
                .ToList();
        }

        /// <summary>
        /// 获取子级人员人数
        /// </summary>
        /// <param name="parUserId"></param>
        /// <returns></returns>
        public int GetChildCount(int parUserId)
        {
            if (parUserId == 0)
                return 0;

            var db = SugarDao.GetInstance(DbName);

            return db.Queryable<Mng_User>()
                .Where(o => o.ParUserId == parUserId && o.Id != parUserId)
                .Count();
        }
    }
}
