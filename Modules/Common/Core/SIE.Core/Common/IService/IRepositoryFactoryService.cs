using SIE.Domain;
using System;

namespace SIE.Core.Common.IService
{
    /// <summary>
    /// Repository工厂接口
    /// </summary>
    public interface IRepositoryFactoryService: ISingletonDependency
    {
        /// <summary>
        /// 用于查找指定实体的仓库。
        /// </summary>
        /// <returns></returns>
        EntityRepository Find(Type entityType);

        /// <summary>
        /// 用于查找指定实体的仓库。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        EntityRepository Find<TEntity>()
            where TEntity : Entity;

        /// <summary>
        /// 用于查找指定类型的仓库。
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        TRepository Concrete<TRepository>()
            where TRepository : EntityRepository;

        #region Shortcuts
        /// <summary>
        /// 批量插入逻辑
        /// batchSize 批次大小 (分批插入实体，每批最多十万条数据，默认是：9999)
        /// </summary>
        /// <param name="entityList">实体列表</param>
        void BatchInsert(EntityList entityList);

        /// <summary>
        /// 批量插入逻辑
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <param name="batchSize">批次大小（分批插入实体，每批最多十万条数据）</param>
        void BatchInsert(EntityList entityList, int batchSize);



        /// <summary>
        /// 保存某个实体列表。
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        void Save(EntityList entityList);

        /// <summary>
        /// 保存某个实体。
        /// </summary>
        /// <param name="entity"></param>
        void Save(Entity entity);

        /// <summary>
        /// 保存某个实体。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveWay"></param>
        void Save(Entity entity, EntitySaveType saveWay);

        /// <summary>
        /// 申明一个实体上下文操作代码块。
        /// </summary>
        /// <returns></returns>
        IDisposable EnterEntityContext();

        /// <summary>
        /// 申明一个禁用了实体上下文操作代码块。
        /// </summary>
        /// <returns></returns>
        IDisposable DisableEntityContext();

        #endregion


        /// <summary>
        /// 主键获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        T GetById<T>(object id, EagerLoadOptions eagerLoad ) where T : Entity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        [IgnoreProxy]
        T GetById<T>(object id) where T : Entity;

        /// <summary>
        /// 获取全部实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paging"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        EntityList<T> GetAll<T>(PagingInfo paging, EagerLoadOptions eagerLoad) where T : Entity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        EntityList<T> GetAll<T>() where T : Entity;

        /// <summary>
        /// 此方法不能在客户端执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        IEntityQueryer<T> Query<T>() where T : Entity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [IgnoreProxy]
        long GetNextId<T>() where T : Entity;

        /// <summary>
        /// 更新实体部分字段或者批量更新
        /// <para>
        /// RF.Update&lt;User&gt;().Set(p=>p.Name, "NewName").Where(p=>p.Id == 1);
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEntityUpdate<T> Update<T>() where T : Entity;

        /// <summary>
        /// 批量删除
        /// <para>
        /// RF.Delete&lt;User&gt;().Where(p=>p.Name.Contains("admin"));
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEntityDelete<T> Delete<T>() where T : Entity;

        /// <summary>
        /// 获取当前用户的Id
        /// </summary>
        /// <returns></returns>
        double GetEmployeeId();

        /// <summary>
        /// 获取当前数据库时间
        /// </summary>
        /// <returns></returns>
        DateTime GetDateTime();
    }
}
