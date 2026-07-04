using SIE.Common.Configs;
using SIE.Domain;
using System;

namespace SIE.Core.Common.Service
{
    /// <summary>
    /// 配置项处理服务类
    /// </summary>
    public class SysConfigService : /*BaseService,*/ IService.ISysConfigService
    {
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public V GetConfig<V>(IConfig<V> config) where V : ConfigValue
        {
            return config.GetValue();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public V GetConfig<V>(GlobalConfig<V> config) where V : ConfigValue
        {
            return (config as IConfig<V>).GetValue();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="config"></param>
        /// <param name="category">分类</param>
        /// <returns></returns>
        public V GetConfig<V, C>(GlobalCategoryConfig<C, V> config, C category) where V : ConfigValue where C : Entity
        {
            config.Category = category;
            return (config as IConfig<V>).GetValue();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public V GetConfig<V, C>(ModuleCategoryConfig<C, V> config, Type type, C category) where V : ConfigValue where C : Entity
        {
            config.Category = category;
            config.EntityType = type;
            return (config as IConfig<V>).GetValue();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public V GetConfig<V>(ModuleConfig<V> config, Type type) where V : ConfigValue
        {
            config.EntityType = type;
            return (config as IConfig<V>).GetValue();
        }
    }
}
