using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.Workbench.AlertLights
{
    /// <summary>
    /// 员工呼叫设置
    /// </summary>
    [RootEntity, Serializable]
    ////[CriteriaQuery]
    [Label("员工呼叫设置")]
    public partial class EmpCallSetting : DataEntity
    {
        #region 异常类型 ExceptionType
        /// <summary>
        /// 异常类型Id
        /// </summary>
        [Label("异常类型")]
        public static readonly IRefIdProperty ExceptionTypeIdProperty =
            P<EmpCallSetting>.RegisterRefId(e => e.ExceptionTypeId, ReferenceType.Normal);

        /// <summary>
        /// 异常类型Id
        /// </summary>
        public double ExceptionTypeId
        {
            get { return (double)this.GetRefId(ExceptionTypeIdProperty); }
            set { this.SetRefId(ExceptionTypeIdProperty, value); }
        }

        /// <summary>
        /// 异常类型
        /// </summary>
        public static readonly RefEntityProperty<Catalog> ExceptionTypeProperty =
            P<EmpCallSetting>.RegisterRef(e => e.ExceptionType, ExceptionTypeIdProperty);

        /// <summary>
        /// 异常类型
        /// </summary>
        public Catalog ExceptionType
        {
            get { return this.GetRefEntity(ExceptionTypeProperty); }
            set { this.SetRefEntity(ExceptionTypeProperty, value); }
        }
        #endregion

        #region 通知列表 InformList
        /// <summary>
        /// 通知列表
        /// </summary>
        public static readonly ListProperty<EntityList<EmpCallInform>> InformListProperty = P<EmpCallSetting>.RegisterList(e => e.InformList);

        /// <summary>
        /// 通知列表
        /// </summary>
        public EntityList<EmpCallInform> InformList
        {
            get { return this.GetLazyList(InformListProperty); }
        }
        #endregion

        #region 呼叫类型 AlertType
        /// <summary>
        /// 呼叫类型
        /// </summary>
        [Label("呼叫类型")]
        public static readonly Property<AlertCallType> AlertTypeProperty = P<EmpCallSetting>.Register(e => e.AlertType);

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public AlertCallType AlertType
        {
            get { return GetProperty(AlertTypeProperty); }
            set { SetProperty(AlertTypeProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        public static readonly IRefIdProperty WorkGroupIdProperty = P<EmpCallSetting>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Parent);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double WorkGroupId
        {
            get { return (double)GetRefId(WorkGroupIdProperty); }
            set { SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<EmpCallSetting>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 视图注册

        #region 异常类型名称 ExceptionName
        /// <summary>
        /// 异常类型名称
        /// </summary>
        [Label("异常类型名称")]
        public static readonly Property<string> ExceptionNameProperty = P<EmpCallSetting>.RegisterView(e => e.ExceptionName, p => p.ExceptionType.Name);

        /// <summary>
        /// 异常类型名称
        /// </summary>
        public string ExceptionName
        {
            get { return this.GetProperty(ExceptionNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 员工呼叫设置 实体配置
    /// </summary>
    internal class EmpCallSettingConfig : EntityConfig<EmpCallSetting>
    {
        /// <summary>
        /// 员工呼叫设置实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP_SETTING").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}