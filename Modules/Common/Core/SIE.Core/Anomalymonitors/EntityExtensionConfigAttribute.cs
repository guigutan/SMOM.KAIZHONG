using System;

namespace SIE.Core.Anomalymonitors
{
	/// <summary>
	/// 异常任务处理-扩展配置
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class EntityExtensionConfigAttribute : Attribute

	{
        /// <summary>
        /// 获取或设置配置类型
        /// </summary>
        public Type ConfigType { get; }

        /// <summary>
        /// 获取配置名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取配置描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="name">配置名称</param>
        /// <param name="description">配置描述</param>
        public EntityExtensionConfigAttribute(Type configType, string name, string description)
        {
            ConfigType = configType;
            Name = name;
            Description = description;
        }
    }
}

