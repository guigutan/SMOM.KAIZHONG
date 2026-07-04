using SIE.Domain;
using SIE.Core.Common.IService;
using System;

namespace SIE.Core.Common.Service
{
    /// <summary>
    /// Repository工厂服务层封装
    /// </summary>
    public class RepositoryFactoryService : /*BaseService, */IRepositoryFactoryService
    {
        /// <summary>
        /// 用于查找指定实体的仓库。
        /// </summary>
        /// <returns></returns>
        public virtual EntityRepository Find(Type entityType)
        {
            return RepositoryFactoryHost.Factory.FindByEntity(entityType) as EntityRepository;
        }

        /// <summary>
        /// 用于查找指定实体的仓库。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual EntityRepository Find<TEntity>()
            where TEntity : Entity
        {
            return RepositoryFactoryHost.Factory.FindByEntity(typeof(TEntity)) as EntityRepository;
        }

        /// <summary>
        /// 用于查找指定类型的仓库。
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        public virtual TRepository Concrete<TRepository>()
            where TRepository : EntityRepository
        {
            return RF.Concrete<TRepository>();
        }

        #region Shortcuts
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


        /// <summary>
        /// 保存某个实体列表。
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        public virtual void Save(EntityList entityList)
        {
            RF.Save(entityList);
        }

        /// <summary>
        /// 保存某个实体。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(Entity entity)
        {
            Save(entity, EntitySaveType.Normal);
        }

        /// <summary>
        /// 保存某个实体。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveWay"></param>
        public virtual void Save(Entity entity, EntitySaveType saveWay)
        {
            RF.Save(entity, saveWay);
        }

        /// <summary>
        /// 申明一个实体上下文操作代码块。
        /// </summary>
        /// <returns></returns>
        public virtual IDisposable EnterEntityContext()
        {
            return RF.EnterEntityContext();
        }

        /// <summary>
        /// 申明一个禁用了实体上下文操作代码块。
        /// </summary>
        /// <returns></returns>
        public virtual IDisposable DisableEntityContext()
        {
            return RF.DisableEntityContext();
        }

        #endregion

        /// <summary>
        /// 主键获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual T GetById<T>(object id, EagerLoadOptions eagerLoad) where T : Entity
        {
            return Find<T>().GetById(id, eagerLoad) as T;
        }

        /// <summary>
        /// ID获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual T GetById<T>(object id) where T : Entity
        {
            return RF.GetById<T>(id);
        }

        /// <summary>
        /// 获取全部实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paging"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual EntityList<T> GetAll<T>(PagingInfo paging, EagerLoadOptions eagerLoad) where T : Entity
        {
            return Find<T>().GetAll(paging, eagerLoad) as EntityList<T>;
        }

        /// <summary>
        /// Get全部实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual EntityList<T> GetAll<T>() where T : Entity
        {
            return RF.GetAll<T>();
        }

        /// <summary>
        /// 此方法不能在客户端执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual IEntityQueryer<T> Query<T>() where T : Entity
        {
            return DB.Query<T>();
        }

        /// <summary>
        /// 获取下一个Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual long GetNextId<T>() where T : Entity
        {
            return RF.Find<T>().GetNextId();
        }

        /// <summary>
        /// 更新实体部分字段或者批量更新
        /// <para>
        /// RF.Update&lt;User&gt;().Set(p=>p.Name, "NewName").Where(p=>p.Id == 1);
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual IEntityUpdate<T> Update<T>() where T : Entity
        {
            return DB.Update<T>();
        }

        /// <summary>
        /// 批量删除
        /// <para>
        /// RF.Delete&lt;User&gt;().Where(p=>p.Name.Contains("admin"));
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEntityDelete<T> Delete<T>() where T : Entity
        {
            return DB.Delete<T>();
        }

        /// <summary>
        /// 获取当前用户的Id
        /// </summary>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual double GetEmployeeId()
        {
            return RT.IdentityId;
        }

        /// <summary>
        /// 获取当前数据库时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetDateTime()
        {
            return RF.Find<WorkOrders.WorkOrder>().GetDbTime();
        }
    }
}
