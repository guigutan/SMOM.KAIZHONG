using SIE.Common.Configs;
using SIE.Domain;
using System;

namespace SIE.Core.Common.IService
{
    /// <summary>
    /// Config 配置接口定义
    /// </summary>
    public interface ISysConfigService : ISingletonDependency
    {
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        V GetConfig<V>(IConfig<V> config) where V : ConfigValue;

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        V GetConfig<V>(GlobalConfig<V> config) where V : ConfigValue;

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="config"></param>
        /// <param name="category">分类</param>
        /// <returns></returns>
        V GetConfig<V, C>(GlobalCategoryConfig<C, V> config, C category) where V : ConfigValue where C : Entity;

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        V GetConfig<V, C>(ModuleCategoryConfig<C, V> config, Type type, C category) where V : ConfigValue where C : Entity;

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        V GetConfig<V>(ModuleConfig<V> config, Type type) where V : ConfigValue;

    }
}
