using System;

namespace SIE.EMS.DataAuth
{
    /// <summary>
    /// 实体数据权限特性，标记需要控制数据权限的实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class BudgetDepartmentAuthAttribute : Attribute
    {        
        /// <summary>
        /// 控制业务权限的属性名称
        /// </summary>
        public string AuthIdProperty { get; }

        /// <summary>
        /// 控制业务权限的属性字段是否可以为空
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// 创建<see cref="BudgetDepartmentAuthAttribute"/>
        /// </summary>
        /// <param name="authIdProperty"></param>
        /// <param name="nullable">authIdProperty值是否可空</param>
        public BudgetDepartmentAuthAttribute( string authIdProperty, bool nullable = false)
        {
            Check.NotNullOrEmpty(authIdProperty, nameof(authIdProperty));
           
            AuthIdProperty = authIdProperty;
            Nullable = nullable;
        }
    }
}
