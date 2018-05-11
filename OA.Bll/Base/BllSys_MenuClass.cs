using OA.Model;
using SqlSugar;
using SqlSugarTool;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            var result = Instance().StoredProcedureToString("Sys_MenuClassAdd", new SugarParameter[] {
                new SugarParameter("@TableName",typeof(Mng_MenuClass).Name,typeof(String)),
                new SugarParameter("@ClassName",model.ClassName,typeof(String)),
                new SugarParameter("@ParId",model.ParId,typeof(Int32)),
                new SugarParameter("@Url",model.Url,typeof(String)),
                new SugarParameter("@Flag",null,true)
            });

            return result == "1";
        }
    }
}
