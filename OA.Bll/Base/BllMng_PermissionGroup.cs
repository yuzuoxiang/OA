using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugarTool;
using SqlSugar;

namespace OA.Bll
{
    public class BllMng_PermissionGroup : BllCommonBase<Mng_PermissionGroup>
    {
        private static readonly string DbName = "OA";
        public BllMng_PermissionGroup() : base(DbName)
        {
        }

        public static BllMng_PermissionGroup Instance()
        {
            return new BllMng_PermissionGroup();
        }

        public bool CheckPermission(int groupId, int permissionId)
        {
            try
            {
                if (groupId == 0)
                    return false;

                string sql = "select top 1 id from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id = @groupId and" +
                    " belock = cast(0 as bit)) and  PermissionId = @permissionId";
                var db = SugarDao.GetInstance(DbName);

                var result = db.Ado.SqlQuery<bool>(sql, new List<SugarParameter> {
                    new  SugarParameter("@groupId",groupId,typeof(Int32)),
                    new SugarParameter("@permissionId",permissionId,typeof(Int32))
                }).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                Tools.Logs.Log4netExt.LogHelper.WriteLog(typeof(BllMng_PermissionGroup), ex, 0, "");
                return false;
            }
        }

        /// <summary>
        /// 上移排序
        /// </summary>
        /// <param name="id"></param>
        public void Up(int id, int departId)
        {
            Mng_PermissionGroup model = First(o => o.Id == id);

            if (model == null)
                return;

            List<Mng_PermissionGroup> list = Queryable(o => o.Sequence < model.Sequence && o.DepartId == departId, " Sequence desc ", o => o.Id, 1);
            if (list.Count == 1)
            {
                Mng_PermissionGroup model1 = First(o => o.Id == list[0].Id);
                int tempSequence = (int)model.Sequence;
                model.Sequence = model1.Sequence;
                Update(model, o => o.Id == id);
                model1.Sequence = tempSequence;
                Update(model1, o => o.Id == list[0].Id);
            }
        }

        /// <summary>
        /// 下移排序
        /// </summary>
        /// <param name="id"></param>
        public void Down(int id, int departId)
        {
            Mng_PermissionGroup model = First(o => o.Id == id);

            if (model == null)
                return;

            List<Mng_PermissionGroup> list = Queryable(o => o.Sequence > model.Sequence && o.DepartId == departId, " Sequence asc ", o => o.Id, 1);
            if (list.Count == 1)
            {
                Mng_PermissionGroup model1 = First(o => o.Id == list[0].Id);
                int tempSequence = (int)model.Sequence;
                model.Sequence = model1.Sequence;
                Update(model, o => o.Id == id);
                model1.Sequence = tempSequence;
                Update(model1, o => o.Id == list[0].Id);
            }
        }

        public bool Delete(int id, int departId)
        {
            bool result = false;
            Mng_PermissionGroup model = First(o => o.Id == id);
            result = Delete(o => o.Id == id);

            if (!result)
                return result;

            var db = SugarDao.GetInstance(DbName);
            return db.Ado.ExecuteCommand("update dbo.Mng_PermissionGroup set Sequence = Sequence - 1 where Sequence > @Sequence and DepartId=@DepartId",
                new List<SugarParameter> {
                    new SugarParameter("@Sequence",model.Sequence,typeof(Int32)),
                    new SugarParameter("@DepartId",model.Sequence,typeof(Int32))
                }) > 0;
        }

        /// <summary>
        /// 设置是否锁定
        /// </summary>
        /// <param name="id">需要设置的信息ID</param>
        /// <returns>true:已设置 false:错误</returns>
        public bool SetBeLock(int id)
        {
            Mng_PermissionGroup modMng_PermissionGroup = First(o => o.Id == id);
            if (modMng_PermissionGroup == null)
                return false;

            Update(new Mng_PermissionGroup() { BeLock = !modMng_PermissionGroup.BeLock }, o => o.Id == id);
            return true;
        }

        /// <summary>
        /// 获取所有记录列表
        /// </summary>
        /// <returns>返回列表记录集</returns>
        public List<Mng_PermissionGroup> GetAllList(int departId, bool includelock = false)
        {
            var db = SugarDao.GetInstance(DbName);

            return db.Queryable<Mng_PermissionGroup>()
                .WhereIF(!includelock, o => o.BeLock == false)
                .WhereIF(departId > 0, o => o.DepartId == departId)
                .OrderBy(o => o.Sequence)
                .ToList();
        }

        /// <summary>
        /// 增加组
        /// </summary>
        /// <param name="modMng_PermissionGroup">组对象</param>
        /// <returns>-2:出错 , -1 存在相同组名的记录, 0 发布成功</returns>
        public int AddNoReturn(Mng_PermissionGroup modMng_PermissionGroup, int departId)
        {
            if (ExistsGruopName(modMng_PermissionGroup.GroupName, departId))
                return -1;

            //设置排序字段
            modMng_PermissionGroup.Sequence = GetNextSequence(departId);

            return (Insert(modMng_PermissionGroup)) ? 0 : -2;
        }

        /// <summary>
        /// 判断是否存在此组名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static bool ExistsGruopName(string groupName, int departId)
        {
            int id = Instance().FirstSelect(o => o.GroupName == groupName && o.DepartId == departId, "Id")?.Id ?? 0;
            return id > 0;
        }

        public static int GetNextSequence(int departId)
        {
            var db = SugarDao.GetInstance(DbName);
            var result = db.Ado.SqlQuery<int>("select max(sequence) from dbo.Mng_PermissionGroup where departId=@departId", new { departId = departId })
                .FirstOrDefault();

            return result < 1 ? 1 : ++result;
        }
    }
}
