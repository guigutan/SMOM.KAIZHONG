using System;

namespace SIE.DataAuth
{
    /// <summary>
    /// 实体数据权限特性，标记需要控制数据权限的实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class EntityDataAuthAttribute : Attribute
    {
        /// <summary>
        /// 权限关联的实体类型，需要使用<see cref="EmployeeAuthAttribute"/>标记
        /// </summary>
        public Type AuthType { get; }

        /// <summary>
        /// 控制业务权限的属性名称
        /// </summary>
        public string AuthIdProperty { get; }

        /// <summary>
        /// 控制业务权限的属性字段是否可以为空
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// 创建<see cref="EntityDataAuthAttribute"/>
        /// </summary>
        /// <param name="authType"></param>
        /// <param name="authIdProperty"></param>
        /// <param name="nullable">authIdProperty值是否可空</param>
        public EntityDataAuthAttribute(Type authType, string authIdProperty, bool nullable = false)
        {
            Check.NotNull(authType, nameof(authType));
            Check.NotNullOrEmpty(authIdProperty, nameof(authIdProperty));
            AuthType = authType;
            AuthIdProperty = authIdProperty;
            Nullable = nullable;
        }
    }
}
