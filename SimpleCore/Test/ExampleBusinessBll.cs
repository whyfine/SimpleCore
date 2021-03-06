﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCore.DataAccess;
using PetaPoco;

namespace SimpleCore.Test
{
    public abstract class ExampleBusinessBll<TEntity> where TEntity : class,new()
    {
        protected virtual event ExceptionEventHandler OnException;
        protected void OnExceptionNotice(Exception ex)
        {
            if (this.OnException != null) this.OnException.BeginInvoke(this, new DbContextExceptionEventArgs() { DbContextEx = ex, Sql = DbContext.GetInstance().LastSQL, SqlArgs = DbContext.GetInstance().LastArgs, SqlCommand = DbContext.GetInstance().LastCommand }, null, null);
        }

        #region Query
        /// <summary>
        /// Query
        /// </summary>
        /// <param name="where">id=@0 and filed=@1</param>
        /// <param name="args">new object[]{ 1,"filed" }</param>
        /// <param name="columns">new string[]{"filed1","filed2"}</param>
        /// <param name="orderbyColumns">new string[]{"id asc","filed desc"}</param>
        /// <returns>异常返回null</returns>
        public virtual List<TEntity> Query(string where = null, object[] args = null, string[] columns = null, string[] orderbyColumns = null)
        {
            return this.Query<TEntity>(where, args, columns, orderbyColumns);

        }
        /// <summary>
        /// QueryCount
        /// </summary>
        /// <param name="where">id=@0 and filed=@1</param>
        /// <param name="args">new object[]{ 1,"filed" }</param>
        /// <returns>异常返回null</returns>
        public virtual long? QueryCount(string where = null, object[] args = null)
        {
            return this.QueryCount<TEntity>(where, args);
        }
        protected virtual List<TOther> Query<TOther>(string where = null, object[] args = null, string[] columns = null, string[] orderbyColumns = null)
            where TOther : class,new()
        {
            try
            {
                //var pd = PocoData.ForType(typeof(TOther), DbContext.GetInstance().DefaultMapper);
                var sql = Sql.Builder;
                if (columns != null && columns.Length > 0)
                    sql.Select(columns);
                if (!string.IsNullOrEmpty(where))
                    sql.Where(where, args);
                if (orderbyColumns != null && orderbyColumns.Length > 0)
                    sql.OrderBy(orderbyColumns);
                return this.Query<TOther>(sql);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        protected virtual long? QueryCount<TOther>(string where = null, object[] args = null)
            where TOther : class,new()
        {
            var sql = Sql.Builder.Select("count(1)");
            if (!string.IsNullOrEmpty(where))
                sql.Where(where, args);
            try
            {
                return DbContext.GetInstance().ExecuteScalar<long>(sql);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        protected List<TOther> Query<TOther>(Sql sql)
        {
            try
            {
                return DbContext.GetInstance().Query<TOther>(sql).ToList();
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="e">poco entity</param>
        /// <returns>异常返回null,成功返回自增id</returns>
        public virtual object Insert(TEntity e)
        {
            return this.Insert<TEntity>(e);
        }
        protected virtual object Insert<TOther>(TOther e)
        {
            try
            {
                return DbContext.GetInstance().Insert(e);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="e">poco entity</param>
        /// <returns>异常返回null,成功返回更新数</returns>
        public virtual int? Update(TEntity e)
        {
            return this.Update<TEntity>(e);
        }
        /// <summary>
        /// Update By Primarykey
        /// </summary>
        /// <param name="e">poco entity</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="columns">更新列名集合new object[]{"column1","column2"}</param>
        /// <returns></returns>
        public virtual int? Update(TEntity e, object primaryKeyValue = null, IEnumerable<string> columns = null)
        {
            return this.Update<TEntity>(e, primaryKeyValue, columns);
        }
        protected int? Update<TOther>(TOther e, object primaryKeyValue = null, IEnumerable<string> columns = null)
            where TOther : class,new()
        {
            try
            {
                return DbContext.GetInstance().Update(e, primaryKeyValue, columns);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        protected int? Update<TOther>(string set, string where, object[] setargs = null, object[] whereargs = null)
            where TOther : class,new()
        {
            var sql = Sql.Builder.Set(set, setargs).Where(where, whereargs);
            return this.Update<TOther>(sql);
        }
        protected int? Update<TOther>(Sql sql)
            where TOther : class,new()
        {
            try
            {
                return DbContext.GetInstance().Update<TOther>(sql);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="primarykey">primary key</param>
        /// <returns>异常返回null，成功返回删除数量</returns>
        public virtual int? Delete(object primarykey)
        {
            return this.Delete<TEntity>(primarykey);
        }
        public virtual int? Delete(string where, object[] args = null)
        {
            var sql = Sql.Builder.Where(where, args);
            return this.Delete<TEntity>(sql);
        }
        protected int? Delete<TOther>(object primarykey)
            where TOther : class,new()
        {
            try
            {
                return DbContext.GetInstance().Delete<TOther>(primarykey);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        protected int? Delete<TOther>(Sql sql)
            where TOther : class,new()
        {
            try
            {
                return DbContext.GetInstance().Delete<TOther>(sql);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        #endregion

        #region Page
        /// <summary>
        /// Page
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="where">id=@0 and filed=@1</param>
        /// <param name="args">new object[]{ id=1,filed="filed"}</param>
        /// <param name="columns">new string[]{"filed1","filed2"}</param>
        /// <param name="orderbyColumns">new string[]{"id asc","filed desc"}</param>
        /// <param name="pageIndex">default 1</param>
        /// <param name="pageSize">default 20</param>
        /// <returns></returns>
        public virtual List<TEntity> Page(out long totalCount, out long totalPage, string where = null, object[] args = null, string[] columns = null, string[] orderbyColumns = null, long pageIndex = 1, long pageSize = 20)
        {
            var sql = Sql.Builder;
            if (columns != null && columns.Length > 0)
                sql.Select(columns);
            if (!string.IsNullOrEmpty(where))
                sql.Where(where, args);
            if (orderbyColumns != null)
                sql.OrderBy(orderbyColumns);
            var p = this.Page(pageIndex, pageSize, sql);
            if (p == null)
            {
                totalCount = 0; totalPage = 0;
                return null;
            }
            totalCount = p.TotalItems;
            totalPage = p.TotalPages;
            return p.Items;
        }
        protected Page<TEntity> Page(long pageIndex, long pageSize, Sql sql)
        {
            try
            {
                return DbContext.GetInstance().Page<TEntity>(pageIndex, pageSize, sql);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        protected List<TEntity> SkipTake(long skip, long take, Sql sql)
        {
            try
            {
                return DbContext.GetInstance().SkipTake<TEntity>(skip, take, sql);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        #endregion

        #region Proc
        /// <summary>
        /// Proc
        /// </summary>
        /// <param name="proc">"proc_name"</param>
        /// <param name="args">new object[]{"arg1","arg2"}</param>
        /// <returns></returns>
        protected List<TEntity> Proc(string proc, object[] args)
        {
            try
            {
                string sql;
                if (args != null && args.Length > 0)
                    sql = string.Format("; EXEC {0} {1}", proc, "@" + string.Join(",@", Enumerable.Range(0, args.Length - 1).ToList()));
                else
                    sql = string.Format("; EXEC {0} ", proc);
                return DbContext.GetInstance().Fetch<TEntity>(sql, args);
            }
            catch (Exception ex)
            {
                this.OnExceptionNotice(ex);
            }
            return null;
        }
        #endregion

        #region Tran
        protected void BeginTran()
        {
            DbContext.GetInstance().BeginTransaction();
        }
        protected void CommiteTran()
        {
            DbContext.GetInstance().CompleteTransaction();
        }
        protected void RollbackTran()
        {
            DbContext.GetInstance().AbortTransaction();
        }
        #endregion
    }

    public class DbContextExceptionEventArgs : EventArgs
    {
        public Exception DbContextEx;
        public string Sql { get; set; }
        public object[] SqlArgs { get; set; }
        public string SqlCommand { get; set; }
    }

    public delegate void ExceptionEventHandler(object sender, DbContextExceptionEventArgs e);
}
