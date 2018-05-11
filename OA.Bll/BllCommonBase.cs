using SqlSugar;
using SqlSugarTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace OA.Bll
{
    /// <summary>
    /// Bll层通用的增删改查,这个类里面的所有方法都要声明为静态方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BllCommonBase<T> : BllServiceBase where T : class, new()
    {
        private static string DBName { get; set; }

        /// <summary>
        /// 初始化BllBase类
        /// </summary>
        /// <param name="DBName">连接的数据库名</param>
        public BllCommonBase(string dbName)
        {
            DBName = dbName;
        }

        #region 通用查询
        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            var db = SugarDao.GetInstance(DBName);
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
            return SugarDao.GetInstance(DBName).Queryable<T>().Where(where).First();
        }

        public T FirstSelect(Expression<Func<T, bool>> where, string select = null, Expression<Func<object, object>> orderBy = null)
        {
            var db = SugarDao.GetInstance(DBName);
            var list = db.Queryable<T>().Where(where);
            if (select != null || !string.IsNullOrEmpty(select))
                list.Select(select);
            var sql = list.ToSql();
            return list.First();
        }

        /// <summary>
        /// 根据条件判断是否存在数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> where)
        {
            return SugarDao.GetInstance(DBName).Queryable<T>().Any(where);
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return SugarDao.GetInstance(DBName).Queryable<T>().Where(where).Count();
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="select"></param>
        /// <param name="top"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<T> Queryable(Expression<Func<T, bool>> where, string orderBy = "", Expression<Func<T, object>> select = null, int top = 0,
            int? pageSize = null, int? pageIndex = null)
        {
            var db = SugarDao.GetInstance(DBName);
            var sq = db.Queryable<T>().Where(where);

            if (!string.IsNullOrEmpty(orderBy))
                sq.OrderBy(orderBy);

            if (select != null)
                sq.Select(select);

            if (top > 0)
                sq.Take(top);

            return (pageSize != null && pageIndex != null)
                ? sq.ToPageList(pageIndex.GetValueOrDefault(), pageSize.GetValueOrDefault())
                : sq.ToList();
        }

        public IEnumerable<T> GetSelectList(int top, string cols, string tableName, string where, string orderBy)
        {
            string selectCols = "*";
            if (cols != "")
                selectCols = cols;

            string topStr = "";
            if (top > 0)
                topStr = "top " + top.ToString();
            string whereStr = "";
            if (where != "")
                whereStr += "where " + where;
            string orderByStr = "";
            if (orderBy != "")
                orderByStr = "order by " + orderBy;

            var sql = string.Format("select {0} {1} from {2} {3} {4}", topStr, selectCols, tableName, whereStr, orderByStr);
            var db = SugarDao.GetInstance(DBName);
            IEnumerable<T> dataList = db.Ado.SqlQuery<T>(sql);
            return dataList;
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

            var db = SugarDao.GetInstance(DBName);
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
            var db = SugarDao.GetInstance(DBName);
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
            var db = SugarDao.GetInstance(DBName);
            return db.Insertable(model).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Insert(List<T> list)
        {
            var db = SugarDao.GetInstance(DBName);
            return db.Insertable(list.ToArray()).ExecuteCommand() > 0;
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
            var db = SugarDao.GetInstance(DBName);
            return db.Deleteable(where).ExecuteCommand() > 0;
        }

        public bool Delete(int id)
        {
            var db = SugarDao.GetInstance(DBName);
            return db.Deleteable<T>().In(id).ExecuteCommand() > 0;
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
            var db = SugarDao.GetInstance(DBName);
            var sq = db.Updateable(model);

            if (where != null)
                sq.Where(where);

            if (updateColumns != null)
                sq.UpdateColumns(updateColumns);

            if (ignoreColumns != null)
                sq.IgnoreColumns(ignoreColumns);

            return sq.ExecuteCommand() > 0;
        }

        public bool UpdateBitField(string setField, bool setValue, int id, string tableName)
        {
            return SugarDao.GetInstance(DBName).Ado.ExecuteCommand($"update {tableName} set {setField}={(setValue ? 1 : 0)} where Id={id}") > 0;
        }
        #endregion

        #region 存储过程
        public string StoredProcedureToString(string tableName, SugarParameter[] sugarParameters)
        {
            var db = SugarDao.GetInstance(DBName);

            var result = db.Ado.UseStoredProcedure(() =>
            {
                return db.Ado.SqlQueryDynamic(tableName, sugarParameters);
            });

            return sugarParameters[sugarParameters.Count() - 1].Value.ToString();
        }

        public bool StoredProcedureToBool(string tableName, SugarParameter[] sugarParameters)
        {
            var result = StoredProcedureToString(tableName, sugarParameters);

            return result == "1";
        }
        #endregion
    }
}
