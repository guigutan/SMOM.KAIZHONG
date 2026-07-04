using SIE.Andon.Andons.Enum;
using SIE.Common.Organizations;
using SIE.Domain;
using SIE.Domain.Caching;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯管理查询实体")]
    public class AndonManageCriterial : Criteria
    {
        #region 事件编码 AndonManageCode
        /// <summary>
        /// 事件编码
        /// </summary>
        [Label("事件编码")]
        public static readonly Property<string> AndonManageCodeProperty = P<AndonManageCriterial>.Register(e => e.AndonManageCode);

        /// <summary>
        /// 事件编码
        /// </summary>
        public string AndonManageCode
        {
            get { return this.GetProperty(AndonManageCodeProperty); }
            set { this.SetProperty(AndonManageCodeProperty, value); }
        }
        #endregion

        #region 安灯大类 AndonManageClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass?> AndonManageClassProperty = P<AndonManageCriterial>.Register(e => e.AndonManageClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass? AndonManageClass
        {
            get { return this.GetProperty(AndonManageClassProperty); }
            set { this.SetProperty(AndonManageClassProperty, value); }
        }
        #endregion

        #region 安灯类型 AndonType
        /// <summary>
        /// 安灯类型Id
        /// </summary>
        [Label("安灯类型")]
        public static readonly IRefIdProperty AndonTypeIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Normal);

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double? AndonTypeId
        {
            get { return (double?)this.GetRefNullableId(AndonTypeIdProperty); }
            set { this.SetRefNullableId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion

        #region 安灯编码 Andon
        /// <summary>
        /// 安灯编码Id
        /// </summary>
        [Label("安灯编码")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.AndonId, ReferenceType.Normal);

        /// <summary>
        /// 安灯编码Id
        /// </summary>
        public double? AndonId
        {
            get { return (double?)this.GetRefNullableId(AndonIdProperty); }
            set { this.SetRefNullableId(AndonIdProperty, value); }
        }

        /// <summary>
        /// 安灯编码
        /// </summary>
        public static readonly RefEntityProperty<Andon> AndonProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<AndonManageState?> StateProperty = P<AndonManageCriterial>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public AndonManageState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 状态 MulitState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> MulitStateProperty = P<AndonManageCriterial>.Register(e => e.MulitState);

        /// <summary>
        /// 状态
        /// </summary>
        public string MulitState
        {
            get { return this.GetProperty(MulitStateProperty); }
            set { this.SetProperty(MulitStateProperty, value); }
        }
        #endregion

        #region 负责部门 Department
        /// <summary>
        /// 负责部门Id
        /// </summary>
        [Label("负责部门")]
        public static readonly IRefIdProperty DepartmentIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 负责部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)this.GetRefNullableId(DepartmentIdProperty); }
            set { this.SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 负责部门
        /// </summary>
        public static readonly RefEntityProperty<Organization> DepartmentProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 负责部门
        /// </summary>
        public Organization Department
        {
            get { return this.GetRefEntity(DepartmentProperty); }
            set { this.SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 触发人 Trigger
        /// <summary>
        /// 触发人Id
        /// </summary>
        [Label("触发人")]
        public static readonly IRefIdProperty TriggerIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.TriggerId, ReferenceType.Normal);

        /// <summary>
        /// 触发人Id
        /// </summary>
        public double? TriggerId
        {
            get { return (double?)this.GetRefNullableId(TriggerIdProperty); }
            set { this.SetRefNullableId(TriggerIdProperty, value); }
        }

        /// <summary>
        /// 触发人
        /// </summary>
        public static readonly RefEntityProperty<Employee> TriggerProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.Trigger, TriggerIdProperty);

        /// <summary>
        /// 触发人
        /// </summary>
        public Employee Trigger
        {
            get { return this.GetRefEntity(TriggerProperty); }
            set { this.SetRefEntity(TriggerProperty, value); }
        }
        #endregion

        #region 处理人 Handler
        /// <summary>
        /// 处理人Id
        /// </summary>
        [Label("处理人")]
        public static readonly IRefIdProperty HandlerIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.HandlerId, ReferenceType.Normal);

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double? HandlerId
        {
            get { return (double?)this.GetRefNullableId(HandlerIdProperty); }
            set { this.SetRefNullableId(HandlerIdProperty, value); }
        }

        /// <summary>
        /// 处理人
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandlerProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.Handler, HandlerIdProperty);

        /// <summary>
        /// 处理人
        /// </summary>
        public Employee Handler
        {
            get { return this.GetRefEntity(HandlerProperty); }
            set { this.SetRefEntity(HandlerProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<AndonManageCriterial>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<AndonManageCriterial>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 是否停线 LineStop
        /// <summary>
        /// 是否停线
        /// </summary>
        [Label("是否停线")]
        public static readonly Property<YesNo?> LineStopProperty = P<AndonManageCriterial>.Register(e => e.LineStop);

        /// <summary>
        /// 是否停线
        /// </summary>
        public YesNo? LineStop
        {
            get { return this.GetProperty(LineStopProperty); }
            set { this.SetProperty(LineStopProperty, value); }
        }
        #endregion

        #region 是否叫料 AskMaterial
        /// <summary>
        /// 是否叫料
        /// </summary>
        [Label("是否叫料")]
        public static readonly Property<YesNo?> AskMaterialProperty = P<AndonManageCriterial>.Register(e => e.AskMaterial);

        /// <summary>
        /// 是否叫料
        /// </summary>
        public YesNo? AskMaterial
        {
            get { return this.GetProperty(AskMaterialProperty); }
            set { this.SetProperty(AskMaterialProperty, value); }
        }
        #endregion

        #region 创建时间 CreateTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateTimeProperty = P<AndonManageCriterial>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion

        #region 产线名称 WipResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> WipResourceNameProperty = P<AndonManageCriterial>.Register(e => e.WipResourceName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
            set { this.SetProperty(WipResourceNameProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonManageController>().QueryAndonManage(this);
        }
    }
}
