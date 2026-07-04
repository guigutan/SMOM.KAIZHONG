using SIE.Domain;
using SIE.Services;
using System;
using System.Linq.Expressions;

namespace SIE.Core.Common.Dao
{
    /// <summary>
    /// 实体DAO抽象基类
    /// </summary>
    public class BaseDao<T> : IDao where T : Entity
    {
        /// <summary>
        /// 构造方法不公开
        /// </summary>
        protected BaseDao()
        {
        }
        /// <summary>
        /// 主键获取实例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual T GetById(object id, EagerLoadOptions eagerLoad = null)
        {
            return RF.GetById<T>(id, eagerLoad);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected virtual IEntityQueryer<T> Query()
        {
            return DB.Query<T>();
        }

        /// <summary>
        /// 单表简单查询
        /// </summary>
        /// <param name="filter">过滤表达式</param>
        /// <param name="paging">分页</param>
        /// <param name="eagerLoad">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<T> FindMany(Expression<Func<T, bool>> filter, PagingInfo paging = null, EagerLoadOptions eagerLoad = null)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            return Query().Where(filter).ToList(paging, eagerLoad);
        }

        /// <summary>
        /// 获取全部实例
        /// </summary>
        /// <param name="paging">分页</param>
        /// <param name="eagerLoad">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<T> GetAll(PagingInfo paging = null, EagerLoadOptions eagerLoad = null)
        {
            return RF.GetAll<T>(paging, eagerLoad);
        }

        /// <summary>
        /// 保存某个实体列表。
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        public virtual void Save(EntityList<T> entityList)
        {
            RF.Save(entityList);
        }

        /// <summary>
        /// 保存某个实体。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(T entity)
        {
            Save(entity, EntitySaveType.Normal);
        }

        /// <summary>
        /// 保存某个实体。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveWay"></param>
        public virtual void Save(T entity, EntitySaveType saveWay)
        {
            RF.Save(entity, saveWay);
        }

        /// <summary>
        /// 更新实体部分字段或者批量更新
        /// <para>
        /// RF.Update&lt;User&gt;().Set(p=>p.Name, "NewName").Where(p=>p.Id == 1);
        /// </para>
        /// </summary>
        /// <returns></returns>
        protected virtual IEntityUpdate<T> Update()
        {
            return DB.Update<T>();
        }

        /// <summary>
        /// 批量删除
        /// <para>
        /// RF.Delete&lt;User&gt;().Where(p=>p.Name.Contains("admin"));
        /// </para>
        /// </summary>
        /// <returns></returns>
        protected virtual IEntityDelete<T> Delete()
        {
            return DB.Delete<T>();
        }

        /// <summary>
        /// 指定删除
        /// </summary>
        /// <param name="filter"></param>
        public virtual void DeleteBy(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            DB.Delete<T>().Where(filter).Execute();
        }

        /// <summary>
        /// 获取当前数据库时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetDbTime()
        {
            return RF.Find<T>().GetDbTime();
        }

        /// <summary>
        /// 批量插入逻辑
        /// batchSize 批次大小 (分批插入实体，每批最多十万条数据，默认是：9999)
        /// </summary>
        /// <param name="entityList">实体列表</param>
        public virtual void BatchInsert(EntityList entityList)
        {
            BatchInsert(entityList, 9999);
        }

        /// <summary>
        /// 批量插入逻辑
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <param name="batchSize">批次大小（分批插入实体，每批最多十万条数据）</param>
        public virtual void BatchInsert(EntityList entityList, int batchSize)
        {
            RF.BatchInsert(entityList, batchSize);
        }

    }
}
