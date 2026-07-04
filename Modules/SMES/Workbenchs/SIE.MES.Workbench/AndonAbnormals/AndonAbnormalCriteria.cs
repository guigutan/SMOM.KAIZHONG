using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.MES.Workbench.AlertLights;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯异常查询界面")]
    public partial class AndonAbnormalCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AndonAbnormalCriteria()
        {
            LoginEmpId = RT.IdentityId;
        }

        #region 产线 ProductLine
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ProductLineIdProperty = P<AndonAbnormalCriteria>.RegisterRefId(e => e.ProductLineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ProductLineId
        {
            get { return (double)GetRefNullableId(ProductLineIdProperty); }
            set { SetRefNullableId(ProductLineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ProductLineProperty = P<AndonAbnormalCriteria>.RegisterRef(e => e.ProductLine, ProductLineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource ProductLine
        {
            get { return GetRefEntity(ProductLineProperty); }
            set { SetRefEntity(ProductLineProperty, value); }
        }
        #endregion

        #region 呼叫类型 AlertType
        /// <summary>
        /// 呼叫类型
        /// </summary>
        [Label("呼叫类型")]
        public static readonly Property<AlertCallType?> AlertTypeProperty = P<AndonAbnormalCriteria>.Register(e => e.AlertType);

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public AlertCallType? AlertType
        {
            get { return GetProperty(AlertTypeProperty); }
            set { SetProperty(AlertTypeProperty, value); }
        }
        #endregion

        #region 异常类型 ExceptionType
        /// <summary>
        /// 异常类型Id
        /// </summary>
        [Label("异常类型")]
        public static readonly IRefIdProperty ExceptionTypeIdProperty =
            P<AndonAbnormalCriteria>.RegisterRefId(e => e.ExceptionTypeId, ReferenceType.Normal);

        /// <summary>
        /// 异常类型Id
        /// </summary>
        public double? ExceptionTypeId
        {
            get { return (double?)this.GetRefNullableId(ExceptionTypeIdProperty); }
            set { this.SetRefNullableId(ExceptionTypeIdProperty, value); }
        }

        /// <summary>
        /// 异常类型
        /// </summary>
        public static readonly RefEntityProperty<Catalog> ExceptionTypeProperty =
            P<AndonAbnormalCriteria>.RegisterRef(e => e.ExceptionType, ExceptionTypeIdProperty);

        /// <summary>
        /// 异常类型
        /// </summary>
        public Catalog ExceptionType
        {
            get { return this.GetRefEntity(ExceptionTypeProperty); }
            set { this.SetRefEntity(ExceptionTypeProperty, value); }
        }
        #endregion

        #region 处理状态 ProcessStatus
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<ProcessStatusType?> ProcessStatusProperty = P<AndonAbnormalCriteria>.Register(e => e.ProcessStatus);

        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessStatusType? ProcessStatus
        {
            get { return GetProperty(ProcessStatusProperty); }
            set { SetProperty(ProcessStatusProperty, value); }
        }
        #endregion

        #region 登录员工 LoginEmp
        /// <summary>
        /// 实际处理人员Id
        /// </summary>
        [Label("实际处理人员")]
        public static readonly IRefIdProperty LoginEmpIdProperty = P<AndonAbnormalCriteria>.RegisterRefId(e => e.LoginEmpId, ReferenceType.Normal);

        /// <summary>
        /// 实际处理人员Id
        /// </summary>
        public double? LoginEmpId
        {
            get { return (double?)GetRefNullableId(LoginEmpIdProperty); }
            set { SetRefNullableId(LoginEmpIdProperty, value); }
        }

        /// <summary>
        /// 实际处理人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> LoginEmpProperty = P<AndonAbnormalCriteria>.RegisterRef(e => e.LoginEmp, LoginEmpIdProperty);

        /// <summary>
        /// 实际处理人员
        /// </summary>
        public Employee LoginEmp
        {
            get { return GetRefEntity(LoginEmpProperty); }
            set { SetRefEntity(LoginEmpProperty, value); }
        }
        #endregion

        /// <summary>
        /// 实现查询的方法
        /// </summary>
        /// <returns>安灯异常列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AlertLightsController>().GetAndonAbnormals(this);
        }
    }
}
