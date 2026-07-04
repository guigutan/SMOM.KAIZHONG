using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序与员工
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序")]
    [DisplayMember(nameof(ProductFamilyCategory))]
    public partial class ProcessEmployee : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<ProcessEmployee>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<ProcessEmployee>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessEmployee>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessEmployee>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品族小类

        /// <summary>
        /// 产品族小类
        /// </summary>
        [Label("产品族小类")]
        public static readonly Property<string> ProductFamilyCategoryProperty = P<ProcessEmployee>.RegisterReadOnly(
            e => e.ProductFamilyCategory, e => e.GetProductFamilyCategory());

        /// <summary>
        /// 产品族小类
        /// </summary>
        public string ProductFamilyCategory
        {
            get { return this.GetProperty(ProductFamilyCategoryProperty); }
        }

        /// <summary>
        /// 获取产品族小类
        /// </summary>
        /// <returns>返回产品族小类</returns>
        private string GetProductFamilyCategory()
        {
            var process = Process;
            if (process != null)
                return process.ProductFamily?.Name;
            return string.Empty;
        }
        #endregion

        #region 产品族小类 ProcessProductFamilyCode
        /// <summary>
        /// 产品族小类
        /// </summary>
        [Label("产品族小类")]
        public static readonly Property<string> ProcessProductFamilyCodeProperty = P<ProcessEmployee>.RegisterView(e => e.ProcessProductFamilyCode, p => p.Process.ProductFamily.Code);

        /// <summary>
        /// 产品族小类
        /// </summary>
        public string ProcessProductFamilyCode
        {
            get { return this.GetProperty(ProcessProductFamilyCodeProperty); }
        }
        #endregion

        #region 产品族 ProcessProductFamilyName
        /// <summary>
        /// 产品族
        /// </summary>
        [Label("产品族")]
        public static readonly Property<string> ProcessProductFamilyNameProperty = P<ProcessEmployee>.RegisterView(e => e.ProcessProductFamilyName, p => p.Process.ProductFamily.Name);

        /// <summary>
        /// 产品族
        /// </summary>
        public string ProcessProductFamilyName
        {
            get { return this.GetProperty(ProcessProductFamilyNameProperty); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType?> ProcessTypeProperty = P<ProcessEmployee>.RegisterView(e => e.ProcessType, p => p.Process.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType? ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
        }
        #endregion

        #region 工段名称 ProcessSegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> ProcessSegmentNameProperty = P<ProcessEmployee>.RegisterView(e => e.ProcessSegmentName, p => p.Process.Segment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string ProcessSegmentName
        {
            get { return this.GetProperty(ProcessSegmentNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 工序与员工 实体配置
    /// </summary>
    internal class ProcessEmployeeConfig : EntityConfig<ProcessEmployee>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROC_EMP").MapAllProperties();
            Meta.Property(ProcessEmployee.EmployeeIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}