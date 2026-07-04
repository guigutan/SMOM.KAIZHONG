using System;

namespace SIE.Core.Caches
{
    /// <summary>
    /// 实体缓存帮助类
    /// </summary>
    public static class EntityCacheHelper
    {
        /// <summary>
        /// 获取实体缓存Key。根据实体类型创建
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEntityKey(Type type)
        {
            return "EntityCache_" + type.FullName;
        }

        /// <summary>
        /// 设置缓存实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        public static void SetCacheEntity<T>(object id, T entity)
        {
            if (!RT.Redis.IsEnableDistributedCache)
                return;
            RT.Redis.HSet<T>(GetEntityKey(typeof(T)), id.ToString(), entity);
        }

        /// <summary>
        /// 根据ID获取缓存实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetCacheEntity<T>(object id)
        {
            if (!RT.Redis.IsEnableDistributedCache)
                return default(T);
            return RT.Redis.HGet<T>(GetEntityKey(typeof(T)), id.ToString());
        }

        /// <summary>
        /// 删除实体缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DeleteCacheEntity<T>(object id)
        {
            if (!RT.Redis.IsEnableDistributedCache)
                return;
            RT.Redis.HDel(GetEntityKey(typeof(T)), id.ToString());
        }

        /// <summary>
        /// 清空实体缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ClearCacheEntity<T>()
        {
            if (!RT.Redis.IsEnableDistributedCache)
                return;
            RT.Redis.Remove(GetEntityKey(typeof(T)));
        }
    }
}
