using SIE.Common.Employees;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工单叫料查询设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单叫料查询设置")]
    public partial class CallMaterialQuerySetting : Entity<double>
    {
        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<CallMaterialQuerySetting>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<CallMaterialQuerySetting>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<CallMaterialQuerySetting>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<CallMaterialQuerySetting>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单叫料查询设置 实体配置
    /// </summary>
    internal class CallMaterialQuerySettingConfig : EntityConfig<CallMaterialQuerySetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_QUERY_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}