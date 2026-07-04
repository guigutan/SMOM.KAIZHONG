using System;

namespace SIE.Equipments.DataAuth
{
    /// <summary>
    /// 实体数据权限特性，标记需要控制数据权限的实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class EquipAccountAuthAttribute : Attribute
    {
        /// <summary>
        /// 设备类型ID的字段名
        /// </summary>
        public string EquipModelIdProperty { get; }

        /// <summary>
        /// 使用部门ID的字段名
        /// </summary>
        public string UseDepartmentIdProperty { get; }

        /// <summary>
        /// 控制业务权限的属性字段是否可以为空
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// 创建<see cref="EquipAccountAuthAttribute"/>
        /// </summary>
        /// <param name="equipModelIdProperty">设备类型ID的字段名</param>
        /// <param name="useDepartmentIdProperty">使用部门ID的字段名</param>
        /// <param name="nullable">authIdProperty值是否可空</param>
        public EquipAccountAuthAttribute(string equipModelIdProperty,
            string useDepartmentIdProperty, bool nullable = false)
        {
            Check.NotNullOrEmpty(equipModelIdProperty, nameof(equipModelIdProperty));
            Check.NotNullOrEmpty(useDepartmentIdProperty, nameof(useDepartmentIdProperty));

            EquipModelIdProperty = equipModelIdProperty;

            UseDepartmentIdProperty = useDepartmentIdProperty;

            Nullable = nullable;
        }
    }
}
