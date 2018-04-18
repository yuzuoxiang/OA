using SqlSugar;
using SqlSugarTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.Bll
{
    /// <summary>
    /// Bll层通用的增删改查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BllCommonBase<T> : BllServiceBase where T : class, new()
    {
        private static string DbName { get; set; }

        public BllCommonBase() { }
        /// <summary>
        /// 初始化BllBase类
        /// </summary>
        /// <param name="dbName">连接的数据库名</param>
        public BllCommonBase(string dbName)
        {
            DbName = dbName;
        }

        #region 通用查询
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            var db = SugarDao.GetInstance(DbName);
            var sq = db.Queryable<T>().ToList();

            return sq.ToList();
        }

        /// <summary>
        /// 返回第一条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> where)
        {
            var db = SugarDao.GetInstance(DbName);
            var sq = db.Queryable<T>().Where(where);

            return sq.First();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="select"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> Queryable(Expression<Func<T, bool>> where, string orderBy = "", Expression<Func<T, object>> select = null, int top = 0)
        {
            var db = SugarDao.GetInstance(DbName);
            var sq = db.Queryable<T>().Where(where);

            if (!string.IsNullOrEmpty(orderBy))
                sq.OrderBy(orderBy);

            if (select != null)
                sq.Select(select);

            if (top > 1)
                sq.Take(top);

            return sq.ToList();
        }

        /// <summary>
        /// SQL参数化查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sugarParameter"></param>
        /// <returns></returns>
        public List<T> Queryable(string sql, SugarParameter[] sugarParameter = null)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException(string.Format("{0}参数不能为空", nameof(sql)));

            var db = SugarDao.GetInstance(DbName);
            return db.Ado.SqlQuery<T>(sql, sugarParameter);
        }

        /// <summary>
        /// 通用查询，带分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="select"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> QueryableByPage(int pageIndex, int pageSize, ref int totalCount, Expression<Func<T, bool>> where, string orderBy = "",
            Expression<Func<T, object>> select = null, int top = 0)
        {
            var db = SugarDao.GetInstance(DbName);
            var sq = db.Queryable<T>().Where(where);

            if (!string.IsNullOrEmpty(orderBy))
                sq.OrderBy(orderBy);

            if (select != null)
                sq.Select(select);

            if (top > 1)
                sq.Take(top);

            return sq.ToPageList(pageIndex, pageSize, ref totalCount);
        }
        #endregion

        #region 通用插入
        /// <summary>
        /// 通用插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(T model)
        {
            var db = SugarDao.GetInstance(DbName);
            return db.Insertable(model).ExecuteCommand() > 0;
        }
        #endregion

        #region 通用删除
        /// <summary>
        /// 通用删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> where)
        {
            var db = SugarDao.GetInstance(DbName);
            return db.Deleteable(where).ExecuteCommand() > 0;
        }
        #endregion

        #region 通用修改
        /// <summary>
        /// 通用修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="updateColumns">只修改列</param>
        /// <param name="ignoreColumns">忽略列</param>
        /// <returns></returns>
        public bool Update(T model, Expression<Func<T, bool>> where = null, Expression<Func<T, object>> updateColumns = null,
            Expression<Func<T, object>> ignoreColumns = null)
        {
            var db = SugarDao.GetInstance(DbName);
            var sq = db.Updateable(model);

            if (where != null)
                sq.Where(where);

            if (updateColumns != null)
                sq.UpdateColumns(updateColumns);

            if (ignoreColumns != null)
                sq.IgnoreColumns(ignoreColumns);

            return sq.ExecuteCommand() > 0;
        }


        #endregion
    }
}
