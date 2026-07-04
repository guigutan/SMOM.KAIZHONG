using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using System;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 配置项帮助类
    /// </summary>
    public class ConfigExtController : DomainController
    {
        /// <summary>
        /// 初始化模块配置项（配置项在数据库已经存在则跳过，不会更新）
        /// </summary>
        /// <typeparam name="E">实体的类型</typeparam>
        /// <typeparam name="M">模块配置项的类型</typeparam>
        /// <typeparam name="V">模块配置项值的类型</typeparam>
        /// <param name="configValue">模块配置项的值</param>
        public virtual void InitModuleConfig<E, M, V>(V configValue)
            where E : Entity
            where M : ModuleConfig<V>
            where V : ConfigValue
        {
            var ctl = RT.Service.Resolve<SIE.Common.Configs.ConfigController>();

            var configTypeName = typeof(M).GetQualifiedName();
            var entityTypeName = typeof(E).GetQualifiedName();

            var cfg = ctl.Get(configTypeName, entityTypeName);
            if (cfg != null)
            {
                return;
            }

            cfg = new Config();
            cfg.GenerateId();
            cfg.PersistenceStatus = PersistenceStatus.New;
            cfg.EntityType = entityTypeName;
            cfg.TypeName = configTypeName;

            var detail = cfg.ConfigDetailList.FirstOrDefault();
            if (detail == null)
            {
                detail = new ConfigDetail();
                cfg.ConfigDetailList.Add(detail);
            }

            detail.Category = DefaultCategory.Default.Id;
            detail.Value = DomainJsonConvert.SerializeObject(configValue,
                ConfigValueSerializerSettings.Settings);
            RF.Save(cfg);
        }

        /// <summary>
        /// 初始化模块配置项（配置项在数据库已经存在则跳过，不会更新）
        /// </summary>
        /// <param name="entityTypeName">实体的类型的程序集限定名</param>
        /// <typeparam name="M">模块配置项的类型</typeparam>
        /// <typeparam name="V">模块配置项值的类型</typeparam>        
        /// <param name="configValue">模块配置项的值</param>
        public virtual void InitModuleConfig<M, V>(string entityTypeName, V configValue)            
            where M : ModuleConfig<V>
            where V : ConfigValue
        {
            var ctl = RT.Service.Resolve<SIE.Common.Configs.ConfigController>();

            var configTypeName = typeof(M).GetQualifiedName();

            var cfg = ctl.Get(configTypeName, entityTypeName);
            if (cfg != null)
            {
                return;
            }

            cfg = new Config();
            cfg.GenerateId();
            cfg.PersistenceStatus = PersistenceStatus.New;
            cfg.EntityType = entityTypeName;
            cfg.TypeName = configTypeName;

            var detail = cfg.ConfigDetailList.FirstOrDefault();
            if (detail == null)
            {
                detail = new ConfigDetail();
                cfg.ConfigDetailList.Add(detail);
            }

            detail.Category = DefaultCategory.Default.Id;
            detail.Value = DomainJsonConvert.SerializeObject(configValue,
                ConfigValueSerializerSettings.Settings);
            RF.Save(cfg);
        }

        /// <summary>
        /// 初始化模块配置项（配置项在数据库已经存在则跳过，不会更新）
        /// </summary>
        /// <typeparam name="E">实体的类型</typeparam>
        /// <typeparam name="M">模块配置项的类型</typeparam>
        /// <typeparam name="C">模块配置项类型的类型</typeparam>
        /// <typeparam name="V">模块配置项值的类型</typeparam>
        /// <param name="categoryId">模块配置项的类型Id</param>
        /// <param name="configValue">模块配置项的值</param>
        public virtual void InitModuleCategoryConfig<E, M, C, V>(string categoryId, V configValue)
            where E : Entity
            where M : ModuleCategoryConfig<C, V>
            where V : ConfigValue
        {
            var ctl = RT.Service.Resolve<SIE.Common.Configs.ConfigController>();

            var configTypeName = typeof(M).GetQualifiedName();
            var entityTypeName = typeof(E).GetQualifiedName();

            var cfg = ctl.Get(configTypeName, entityTypeName);
            if (cfg != null)
            {
                return;
            }

            cfg = new Config();
            cfg.GenerateId();
            cfg.PersistenceStatus = PersistenceStatus.New;
            cfg.EntityType = entityTypeName;
            cfg.TypeName = configTypeName;

            var detail = cfg.ConfigDetailList.FirstOrDefault();
            if (detail == null)
            {
                detail = new ConfigDetail();
                cfg.ConfigDetailList.Add(detail);
            }

            detail.Category = categoryId.IsNullOrEmpty() ? "0" : categoryId;
            detail.Value = DomainJsonConvert.SerializeObject(configValue,
                ConfigValueSerializerSettings.Settings);
            RF.Save(cfg);
        }

        /// <summary>
        /// 初始化全局配置项（配置项在数据库存在，则跳过）
        /// </summary>
        /// <typeparam name="G">全局配置项</typeparam>
        /// <typeparam name="V">全局配置项值类型</typeparam>
        /// <param name="configValue">全局配置项的配置值</param>
        public virtual void InitGlobalConfig<G, V>(V configValue)
            where G : GlobalConfig<V>
            where V : ConfigValue
        {
            var ctl = RT.Service.Resolve<SIE.Common.Configs.ConfigController>();
            var configTypeName = typeof(G).GetQualifiedName();
            var cfg = ctl.Get(configTypeName);

            if (cfg != null)
            {
                return;
            }

            cfg = new Config();
            cfg.GenerateId();
            cfg.PersistenceStatus = PersistenceStatus.New;
            cfg.TypeName = configTypeName;
            cfg.EntityType = Config.GlobalType;

            var detail = cfg.ConfigDetailList.FirstOrDefault();
            if (detail == null)
            {
                detail = new ConfigDetail();
                cfg.ConfigDetailList.Add(detail);
            }

            detail.Value = DomainJsonConvert.SerializeObject(configValue,
                ConfigValueSerializerSettings.Settings);

            RF.Save(cfg);
        }
    }
}
