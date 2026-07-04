using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Roles;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 角色扩展属性
    /// </summary>
    [Label("角色扩展属性")]
    [CompiledPropertyDeclarer]
    public static class RoleExt
    {
        #region bool? IsAllWarehouse (全仓库权限)
        /// <summary>
        /// 全仓库权限 扩展属性。
        /// </summary>
        public static readonly Property<bool?> IsAllWarehouseProperty =
            P<Role>.RegisterExtension<bool?>("IsAllWarehouse", typeof(RoleExt));

        /// <summary>
        /// 获取 全仓库权限 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static bool? GetIsAllWarehouse(this Role me)
        {
            return me.GetProperty(IsAllWarehouseProperty);
        }

        /// <summary>
        /// 设置 全仓库权限 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetIsAllWarehouse(this Role me, bool? value)
        {
            me.SetProperty(IsAllWarehouseProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 角色
    /// </summary>
    [RootEntity, Serializable]   
    [Label("角色")]
    [DisplayMember(nameof(Role.Code))]
    public partial class RoleExtEntity : Role
    {
        #region 全仓库权限 IsAllWarehouse
        /// <summary>
        /// 全仓库权限
        /// </summary>
        [Label("全仓库权限")]
        public static readonly Property<bool?> IsAllWarehouseProperty = P<RoleExtEntity>.Register(e => e.IsAllWarehouse);

        /// <summary>
        /// 全仓库权限
        /// </summary>
        public bool? IsAllWarehouse
        {
            get { return this.GetProperty(IsAllWarehouseProperty); }
            set { this.SetProperty(IsAllWarehouseProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 角色 实体配置
    /// </summary>
    internal class RoleExtEntityConfig : EntityConfig<RoleExtEntity>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {            
            Meta.MapTable("SYS_ROLE").MapAllProperties();
            Meta.Property(Role.DescriptionProperty).ColumnMeta.HasLength(200);
            Meta.EnablePhantoms();
        }
    }
}
