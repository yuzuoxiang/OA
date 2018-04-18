using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarTool
{
    public class SugarDao
    {
        static string connName;

        /// <summary>
        /// 数据库连接属性
        /// </summary>
        private static string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            }
        }


        /// <summary>
        /// 创建数据库链接
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dbType"></param>
        /// <param name="shardSameThread"></param>
        /// <param name="autoCloseConnection"></param>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(string conn = "", DbType dbType = DbType.SqlServer, bool shardSameThread = false, bool autoCloseConnection = true)
        {
            if (string.IsNullOrEmpty(conn))
                throw new ArgumentNullException("SqlSugar的连接字符串不能为空");

            connName = conn;

            return new SqlSugarClient(new ConnectionConfig()
            {
                //连接字符串
                ConnectionString = connectionString,
                //数据库类型
                DbType = dbType,
                //是否自动释放数据库，设为true时不需要close或者Using的操作，比较推荐
                IsAutoCloseConnection = autoCloseConnection,
                //设为true相同线程是同一个SqlSugarClient
                IsShardSameThread = shardSameThread,
                //初始化主键和自增列信息的方式
                InitKeyType = InitKeyType.SystemTable
            });
        }
    }
}
