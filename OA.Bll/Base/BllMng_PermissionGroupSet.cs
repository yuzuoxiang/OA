using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugarTool;

namespace OA.Bll
{
    public class BllMng_PermissionGroupSet : BllCommonBase<Mng_PermissionGroupSet>
    {
        private static readonly string DbName = "OA";
        public BllMng_PermissionGroupSet() : base(DbName)
        {
        }

        public static BllMng_PermissionGroupSet Instance()
        {
            return new BllMng_PermissionGroupSet();
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public static bool CheckPermission(int groupId, int permissionId)
        {
            int id =Instance().FirstSelect(o => o.GroupId == groupId && o.PermissionId == permissionId, "Id")?.Id ?? 0;
            return id > 0;
        }

        /// <summary>
        /// 保存某用户OA权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        public static bool SavePermission(int groupId, string permissionIds)
        {
            if (string.IsNullOrEmpty(permissionIds))
                return Clear(groupId); //如果没有权限，则清空。

            //判断权限是否符合标准
            if (!CheckSqlValueByArrayInt(permissionIds))
                return false;

            //先删除不属于所选范围的ID
            string where = string.Format("GroupId = {0} and PermissionId not in({1})", groupId.ToString(), permissionIds);
            Clear(where);

            //再增加没有的ID
            string[] permissionId = permissionIds.Split(',');

            List<Mng_PermissionGroupSet> list = new List<Mng_PermissionGroupSet>();
            for (int i = 0; i < permissionId.Length; i++)
            {
                //加入权限表
                list.Add(new Mng_PermissionGroupSet()
                {
                    GroupId = groupId,
                    PermissionId = int.Parse(permissionId[i].ToString())
                });
            }
            return Instance().Insert(list);
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="string">where</param>
        /// <returns>True or False</returns>
        public static bool Clear(string where)
        {
            if (where == "")
                return false;

            var db = SugarDao.GetInstance(DbName);

            return db.Ado.ExecuteCommand(string.Format("delete from dbo.Mng_PermissionGroupSet where {0}", CheckSqlValue(where))) > 0;
        }

        /// <summary>
        /// 清空某用户指定类型的权限(主要用于清除某用户的OA权限)
        /// </summary>
        /// <param name="groupId">用户ID</param> 
        /// <returns>成功或失败</returns>
        private static bool Clear(int groupId)
        {
            string where = string.Format("GroupId = {0}", groupId.ToString());
            return Clear(where);
        }

        /// <summary>
        /// 获取某用户OA权限集
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static List<Mng_PermissionGroupSet> GetPermissionList(int groupId)
        {
            return Instance().GetSelectList(0, "Id,PermissionId", typeof(Mng_PermissionGroupSet).Name, string.Format("GroupId = {0}", groupId.ToString()), "")
                as List<Mng_PermissionGroupSet>;
        }
    }
}
