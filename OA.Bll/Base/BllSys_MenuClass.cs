using OA.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Bll
{
    public class BllSys_MenuClass : BllSys_Class<Mng_MenuClass>
    {
        /// <summary>
        /// 增加分类
        /// </summary>
        /// <param name="sys_ManageClass"></param>
        /// <returns></returns>
        public new static bool Add(Mng_MenuClass model)
        {
            if (string.IsNullOrEmpty(model.ClassName))
                return false;

            var result = Instance().StoredProcedure<bool>("Sys_ClassDel", new SugarParameter[] {
                new SugarParameter("@TableName",typeof(Mng_MenuClass).Name,typeof(String)),
                new SugarParameter("@ClassName",model.ClassName,typeof(String)),
                new SugarParameter("@ParId",model.ParId,typeof(Int32)),
                new SugarParameter("@Url",model.Url,typeof(String)),
                new SugarParameter("@Flag",false,true)
            });

            return result;
            //var proc = Db.Context.FromProc("Sys_MenuClassAdd")
            //     .AddInParameter("TableName", System.Data.DbType.String, tableName)
            //     .AddInParameter("ClassName", System.Data.DbType.String, model.ClassName)
            //     .AddInParameter("ParId", System.Data.DbType.Int32, model.ParId)
            //     .AddInParameter("Url", System.Data.DbType.String, model.Url)
            //     .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            //proc.ExecuteNonQuery();
            //var dic = proc.GetReturnValues();
            //if (dic.ContainsKey(flag))
            //    return bool.Parse(dic[flag].ToString());
            //return false;
        }
    }
}
