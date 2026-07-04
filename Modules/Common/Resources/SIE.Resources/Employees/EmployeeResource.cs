using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工与资源
    /// </summary>
    [ChildEntity, Serializable]
    [Label("员工与资源")]
    public partial class EmployeeResource : DataEntity
    {
        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<EmployeeResource>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<EmployeeResource>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary> 
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ResourceCodeProperty = P<EmployeeResource>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion 

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ResourceNameProperty = P<EmployeeResource>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 资源类型 ResourceSourceType
        /// <summary>
        /// 资源类型
        /// </summary>
        [Label("资源类型")]
        public static readonly Property<SyncSourceType> ResourceSourceTypeProperty = P<EmployeeResource>.RegisterView(e => e.ResourceSourceType, p => p.Resource.SourceType);

        /// <summary>
        /// 资源类型
        /// </summary>
        public SyncSourceType ResourceSourceType
        {
            get { return this.GetProperty(ResourceSourceTypeProperty); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmployeeResource>.RegisterRefId(e => e.EmployeeId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmployeeResource>.RegisterRef(e => e.Employee, EmployeeIdProperty);

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
    /// 员工与资源 实体配置
    /// </summary>
    internal class EmployeeResourceConfig : EntityConfig<EmployeeResource>
    {
        /// <summary>
        /// 实体数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP_RESOURCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}