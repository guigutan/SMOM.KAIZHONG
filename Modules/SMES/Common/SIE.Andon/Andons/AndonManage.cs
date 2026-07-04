using SIE.Andon.Andons.Configs;
using SIE.Andon.Andons.Enum;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.Equipments.EquipAccounts;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonManageCriterial))]
    [EntityWithConfig(typeof(AndonManageCodeConfig))]
    [EntityWithConfig(typeof(AndonManageWarningConfig))]
    [Label("安灯管理")]
    public class AndonManage : DataEntity
    {
        #region 安灯事件编码 AndonManageCode
        /// <summary>
        /// 安灯事件编码
        /// </summary>
        [Required]
        [Label("安灯事件编码")]
        public static readonly Property<string> AndonManageCodeProperty = P<AndonManage>.Register(e => e.AndonManageCode);

        /// <summary>
        /// 安灯事件编码
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
        [Required]
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass> AndonManageClassProperty = P<AndonManage>.Register(e => e.AndonManageClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass AndonManageClass
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
            P<AndonManage>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Normal);

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double AndonTypeId
        {
            get { return (double)this.GetRefId(AndonTypeIdProperty); }
            set { this.SetRefId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<AndonManage>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion

        #region 安灯名称 Andon
        /// <summary>
        /// 安灯名称Id
        /// </summary>
        [Label("安灯名称")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<AndonManage>.RegisterRefId(e => e.AndonId, ReferenceType.Normal);

        /// <summary>
        /// 安灯名称Id
        /// </summary>
        public double AndonId
        {
            get { return (double)this.GetRefId(AndonIdProperty); }
            set { this.SetRefId(AndonIdProperty, value); }
        }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public static readonly RefEntityProperty<Andon> AndonProperty =
            P<AndonManage>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯名称
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        /*        #region 解决方案 Solution
                /// <summary>
                /// 解决方案
                /// </summary>
                [MaxLength(2000)]
                [Label("解决方案")]
                public static readonly Property<string> SolutionProperty = P<AndonManage>.Register(e => e.Solution);

                /// <summary>
                /// 解决方案
                /// </summary>
                public string Solution
                {
                    get { return this.GetProperty(SolutionProperty); }
                    set { this.SetProperty(SolutionProperty, value); }
                }
                #endregion*/


        #region 故障描述 Solution
        /// <summary>
        /// 故障描述
        /// </summary>
        [MaxLength(2000)]
        [Label("故障描述")]
        public static Property<string> SolutionProperty = P<AndonManage>.Register(e => e.Solution);
        //public static readonly Property<string> SolutionProperty = P<AndonManage>.Register(e => e.Solution);
        /// <summary>
        /// 故障描述
        /// </summary>
        public string Solution
        {
            get { return this.GetProperty(SolutionProperty); }
            set { this.SetProperty(SolutionProperty, value); }
        }
        #endregion


        #region 问题描述 ProblemDesc
        /// <summary>
        /// 问题描述
        /// </summary>
        [MaxLength(2000)]
        [Label("问题描述")]
        public static readonly Property<string> ProblemDescProperty = P<AndonManage>.Register(e => e.ProblemDesc);

        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDesc
        {
            get { return this.GetProperty(ProblemDescProperty); }
            set { this.SetProperty(ProblemDescProperty, value); }
        }
        #endregion

        #region 负责部门 Department
        /// <summary>
        /// 负责部门
        /// </summary>
        [Label("负责部门")]
        public static readonly Property<string> DepartmentProperty = P<AndonManage>.Register(e => e.Department);

        /// <summary>
        /// 负责部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        [Label("状态")]
        public static readonly Property<AndonManageState> StateProperty = P<AndonManage>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public AndonManageState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 缺陷代码 Defect
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        [MaxLength(240)]
        public static readonly Property<string> DefectProperty = P<AndonManage>.Register(e => e.Defect);

        /// <summary>
        /// 缺陷代码
        /// </summary>

        public string Defect
        {
            get { return this.GetProperty(DefectProperty); }
            set { this.SetProperty(DefectProperty, value); }
        }
        #endregion

        #region 缺陷代码Id列表 DefectIds
        /// <summary>
        /// 缺陷代码Id列表
        /// </summary>
        [Label("缺陷代码Id列表")]
        [MaxLength(240)]
        public static readonly Property<string> DefectIdsProperty = P<AndonManage>.Register(e => e.DefectIds);

        /// <summary>
        /// 缺陷代码Id列表
        /// </summary>
        public string DefectIds
        {
            get { return this.GetProperty(DefectIdsProperty); }
            set { this.SetProperty(DefectIdsProperty, value); }
        }
        #endregion

        #region 故障发生时间 FaultTime
        /// <summary>
        /// 故障发生时间
        /// </summary>
        [Required]
        [Label("故障发生时间")]
        public static readonly Property<DateTime> FaultTimeProperty = P<AndonManage>.Register(e => e.FaultTime);

        /// <summary>
        /// 故障发生时间
        /// </summary>
        public DateTime FaultTime
        {
            get { return this.GetProperty(FaultTimeProperty); }
            set { this.SetProperty(FaultTimeProperty, value); }
        }
        #endregion

        #region 触发人 Trigger
        /// <summary>
        /// 触发人Id
        /// </summary>
        [Label("触发人")]
        public static readonly IRefIdProperty TriggerIdProperty =
            P<AndonManage>.RegisterRefId(e => e.TriggerId, ReferenceType.Normal);

        /// <summary>
        /// 触发人Id
        /// </summary>
        public double TriggerId
        {
            get { return (double)this.GetRefId(TriggerIdProperty); }
            set { this.SetRefId(TriggerIdProperty, value); }
        }

        /// <summary>
        /// 触发人
        /// </summary>
        public static readonly RefEntityProperty<Employee> TriggerProperty =
            P<AndonManage>.RegisterRef(e => e.Trigger, TriggerIdProperty);

        /// <summary>
        /// 触发人
        /// </summary>
        public Employee Trigger
        {
            get { return this.GetRefEntity(TriggerProperty); }
            set { this.SetRefEntity(TriggerProperty, value); }
        }
        #endregion

        #region 责任人 RespPerson
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Label("责任人")]
        public static readonly IRefIdProperty RespPersonIdProperty =
            P<AndonManage>.RegisterRefId(e => e.RespPersonId, ReferenceType.Normal);

        /// <summary>
        /// 责任人Id
        /// </summary>
        public double? RespPersonId
        {
            get { return (double?)this.GetRefId(RespPersonIdProperty); }
            set { this.SetRefId(RespPersonIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> RespPersonProperty =
            P<AndonManage>.RegisterRef(e => e.RespPerson, RespPersonIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee RespPerson
        {
            get { return this.GetRefEntity(RespPersonProperty); }
            set { this.SetRefEntity(RespPersonProperty, value); }
        }
        #endregion

        #region 触发时间 TriggerTime
        /// <summary>
        /// 触发时间
        /// </summary>
        [Required]
        [Label("触发时间")]
        public static readonly Property<DateTime> TriggerTimeProperty = P<AndonManage>.Register(e => e.TriggerTime);

        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime
        {
            get { return this.GetProperty(TriggerTimeProperty); }
            set { this.SetProperty(TriggerTimeProperty, value); }
        }
        #endregion

        #region 处理人 Handler
        /// <summary>
        /// 处理人Id
        /// </summary>
        [Label("处理人")]
        public static readonly IRefIdProperty HandlerIdProperty =
            P<AndonManage>.RegisterRefId(e => e.HandlerId, ReferenceType.Normal);

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
            P<AndonManage>.RegisterRef(e => e.Handler, HandlerIdProperty);

        /// <summary>
        /// 处理人
        /// </summary>
        public Employee Handler
        {
            get { return this.GetRefEntity(HandlerProperty); }
            set { this.SetRefEntity(HandlerProperty, value); }
        }
        #endregion

        #region 关闭时间 CloseTime
        /// <summary>
        /// 关闭时间
        /// </summary>
        [Label("关闭时间")]
        public static readonly Property<DateTime?> CloseTimeProperty = P<AndonManage>.Register(e => e.CloseTime);

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime
        {
            get { return this.GetProperty(CloseTimeProperty); }
            set { this.SetProperty(CloseTimeProperty, value); }
        }
        #endregion

        #region 持续时间(小时) LastTime
        /// <summary>
        /// 持续时间(小时)
        /// </summary>
        [Label("持续时间(小时)")]
        public static readonly Property<double?> LastTimeProperty = P<AndonManage>.Register(e => e.LastTime);

        /// <summary>
        /// 持续时间(小时)
        /// </summary>
        public double? LastTime
        {
            get { return this.GetProperty(LastTimeProperty); }
            set { this.SetProperty(LastTimeProperty, value); }
        }
        #endregion

        #region 实际影响时间(分钟) ActualTime
        /// <summary>
        /// 实际影响时间(分钟)
        /// </summary>
        [Label("实际影响时间(分钟)")]
        public static readonly Property<double?> ActualTimeProperty = P<AndonManage>.Register(e => e.ActualTime);

        /// <summary>
        /// 实际影响时间(分钟)
        /// </summary>
        public double? ActualTime
        {
            get { return this.GetProperty(ActualTimeProperty); }
            set { this.SetProperty(ActualTimeProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AndonManage>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)this.GetRefId(FactoryIdProperty); }
            set { this.SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AndonManage>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
            P<AndonManage>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)this.GetRefId(WorkShopIdProperty); }
            set { this.SetRefId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AndonManage>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
            P<AndonManage>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<AndonManage>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<AndonManage>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<AndonManage>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AndonManage>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<AndonManage>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<AndonManage>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<AndonManage>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 条码号 BarCode
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> BarCodeProperty = P<AndonManage>.Register(e => e.BarCode);

        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode
        {
            get { return this.GetProperty(BarCodeProperty); }
            set { this.SetProperty(BarCodeProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> WorkGroupProperty = P<AndonManage>.Register(e => e.WorkGroup);

        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroup
        {
            get { return this.GetProperty(WorkGroupProperty); }
            set { this.SetProperty(WorkGroupProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<AndonManage>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<AndonManage>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 是否停线 LineStop
        /// <summary>
        /// 是否停线
        /// </summary>
        [Label("是否停线")]
        public static readonly Property<bool> LineStopProperty = P<AndonManage>.Register(e => e.LineStop);

        /// <summary>
        /// 是否停线
        /// </summary>
        public bool LineStop
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
        public static readonly Property<bool> AskMaterialProperty = P<AndonManage>.Register(e => e.AskMaterial);

        /// <summary>
        /// 是否叫料
        /// </summary>
        public bool AskMaterial
        {
            get { return this.GetProperty(AskMaterialProperty); }
            set { this.SetProperty(AskMaterialProperty, value); }
        }
        #endregion

        #region 图片 PhotoFile
        /// <summary>
        /// 图片
        /// </summary>
        [MaxLength(2000)]
        [Label("图片")]
        public static readonly Property<string> PhotoFileProperty = P<AndonManage>.Register(e => e.PhotoFile);

        /// <summary>
        /// 图片
        /// </summary>
        public string PhotoFile
        {
            get { return this.GetProperty(PhotoFileProperty); }
            set { this.SetProperty(PhotoFileProperty, value); }
        }
        #endregion

        #region 附件 Attachment
        /// <summary>
        /// 附件
        /// </summary>
        [MaxLength(2000)]
        [Label("附件")]
        public static readonly Property<string> AttachmentProperty = P<AndonManage>.Register(e => e.Attachment);

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment
        {
            get { return this.GetProperty(AttachmentProperty); }
            set { this.SetProperty(AttachmentProperty, value); }
        }
        #endregion

        #region 停线信息 AbnormalCause
        /// <summary>
        /// 停线信息Id
        /// </summary>
        [Label("停线信息")]
        public static readonly IRefIdProperty AbnormalCauseIdProperty =
            P<AndonManage>.RegisterRefId(e => e.AbnormalCauseId, ReferenceType.Normal);

        /// <summary>
        /// 停线信息Id
        /// </summary>
        public double? AbnormalCauseId
        {
            get { return (double?)this.GetRefNullableId(AbnormalCauseIdProperty); }
            set { this.SetRefNullableId(AbnormalCauseIdProperty, value); }
        }

        /// <summary>
        /// 停线信息
        /// </summary>
        public static readonly RefEntityProperty<AbnormalCause> AbnormalCauseProperty =
            P<AndonManage>.RegisterRef(e => e.AbnormalCause, AbnormalCauseIdProperty);

        /// <summary>
        /// 停线信息
        /// </summary>
        public AbnormalCause AbnormalCause
        {
            get { return this.GetRefEntity(AbnormalCauseProperty); }
            set { this.SetRefEntity(AbnormalCauseProperty, value); }
        }
        #endregion

        #region 事件报告
        #region 事件原因 Reason
        /// <summary>
        /// 事件原因
        /// </summary>
        [MaxLength(2000)]
        [Label("事件原因")]
        public static readonly Property<string> ReasonProperty = P<AndonManage>.Register(e => e.Reason);

        /// <summary>
        /// 事件原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 处理方式 HandleMethod
        /// <summary>
        /// 处理方式
        /// </summary>
        [MaxLength(2000)]
        [Label("处理方式")]
        public static readonly Property<string> HandleMethodProperty = P<AndonManage>.Register(e => e.HandleMethod);

        /// <summary>
        /// 处理方式
        /// </summary>
        public string HandleMethod
        {
            get { return this.GetProperty(HandleMethodProperty); }
            set { this.SetProperty(HandleMethodProperty, value); }
        }
        #endregion

        #region 预防措施 Measures
        /// <summary>
        /// 预防措施
        /// </summary>
        [MaxLength(2000)]
        [Label("预防措施")]
        public static readonly Property<string> MeasuresProperty = P<AndonManage>.Register(e => e.Measures);

        /// <summary>
        /// 预防措施
        /// </summary>
        public string Measures
        {
            get { return this.GetProperty(MeasuresProperty); }
            set { this.SetProperty(MeasuresProperty, value); }
        }
        #endregion

        #region 安灯管理经验库标识 ExperienceFlag
        /// <summary>
        /// 安灯管理经验库标识
        /// </summary>
        [Label("安灯管理经验库标识")]
        public static readonly Property<bool> ExperienceFlagProperty = P<AndonManage>.Register(e => e.ExperienceFlag);

        /// <summary>
        /// 安灯管理经验库标识
        /// </summary>
        public bool ExperienceFlag
        {
            get { return this.GetProperty(ExperienceFlagProperty); }
            set { this.SetProperty(ExperienceFlagProperty, value); }
        }
        #endregion
        #endregion

        #region 操作记录 OperateLogList
        /// <summary>
        /// 操作记录
        /// </summary>
        [Label("操作记录")]
        public static readonly ListProperty<EntityList<AndonManageOperateLog>> OperateLogListProperty = P<AndonManage>.RegisterList(e => e.OperateLogList);

        /// <summary>
        /// 操作记录
        /// </summary>
        public EntityList<AndonManageOperateLog> OperateLogList
        {
            get { return this.GetLazyList(OperateLogListProperty); }
        }
        #endregion

        #region 物料明细 ItemDetail
        /// <summary>
        /// 物料明细
        /// </summary>
        [Label("物料明细")]
        public static readonly ListProperty<EntityList<AndonManageCallMaterial>> ItemDetailProperty = P<AndonManage>.RegisterList(e => e.ItemDetail);

        /// <summary>
        /// 物料明细
        /// </summary>
        public EntityList<AndonManageCallMaterial> ItemDetail
        {
            get { return this.GetLazyList(ItemDetailProperty); }
        }
        #endregion

        #region 消息推送 MessageSendList
        /// <summary>
        /// 消息推送
        /// </summary>
        [Label("消息推送")]
        public static readonly ListProperty<EntityList<AndonManageMessageSend>> MessageSendListProperty = P<AndonManage>.RegisterList(e => e.MessageSendList);

        /// <summary>
        /// 消息推送
        /// </summary>
        public EntityList<AndonManageMessageSend> MessageSendList
        {
            get { return this.GetLazyList(MessageSendListProperty); }
        }
        #endregion

        #region 通用问题描述 GeneralProbDtl
        /// <summary>
        /// 通用问题描述Id
        /// </summary>
        [Label("通用问题描述")]
        public static readonly IRefIdProperty GeneralProbDtlIdProperty =
            P<AndonManage>.RegisterRefId(e => e.GeneralProbDtlId, ReferenceType.Normal);

        /// <summary>
        /// 通用问题描述Id
        /// </summary>
        public double? GeneralProbDtlId
        {
            get { return (double?)this.GetRefNullableId(GeneralProbDtlIdProperty); }
            set { this.SetRefNullableId(GeneralProbDtlIdProperty, value); }
        }

        /// <summary>
        /// 通用问题描述
        /// </summary>
        public static readonly RefEntityProperty<GeneralProbDtl> GeneralProbDtlProperty =
            P<AndonManage>.RegisterRef(e => e.GeneralProbDtl, GeneralProbDtlIdProperty);

        /// <summary>
        /// 通用问题描述
        /// </summary>
        public GeneralProbDtl GeneralProbDtl
        {
            get { return this.GetRefEntity(GeneralProbDtlProperty); }
            set { this.SetRefEntity(GeneralProbDtlProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<string> PriorityProperty = P<AndonManage>.RegisterView(e => e.Priority, p => p.Andon.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority
        {
            get { return this.GetProperty(PriorityProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<AndonManage>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<AndonManage>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<AndonManage>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<AndonManage>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 产线名称 WipResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> WipResourceNameProperty = P<AndonManage>.RegisterView(e => e.WipResourceName, p => p.WipResource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
        }
        #endregion

        #region 产线编码 WipResourceCode
        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> WipResourceCodeProperty = P<AndonManage>.RegisterView(e => e.WipResourceCode, p => p.WipResource.Code);

        /// <summary>
        /// 产线编码
        /// </summary>
        public string WipResourceCode
        {
            get { return this.GetProperty(WipResourceCodeProperty); }
        }
        #endregion

        #region 停线只读标识 LineStopFlag
        /// <summary>
        /// 停线只读标识
        /// </summary>
        [Label("停线只读标识")]
        public static readonly Property<AndonYesOrNo> LineStopFlagProperty = P<AndonManage>.RegisterView(e => e.LineStopFlag, p => p.Andon.LineStop);

        /// <summary>
        /// 停线只读标识
        /// </summary>
        public AndonYesOrNo LineStopFlag
        {
            get { return this.GetProperty(LineStopFlagProperty); }
            set { this.SetProperty(LineStopFlagProperty, value); }
        }
        #endregion

        #region 叫料只读标识 AskMaterialFlag
        /// <summary>
        /// 叫料只读标识
        /// </summary>
        [Label("叫料只读标识")]
        public static readonly Property<AndonYesOrNo> AskMaterialFlagProperty = P<AndonManage>.RegisterView(e => e.AskMaterialFlag, p => p.Andon.AskMaterial);

        /// <summary>
        /// 叫料只读标识
        /// </summary>
        public AndonYesOrNo AskMaterialFlag
        {
            get { return this.GetProperty(AskMaterialFlagProperty); }
            set { this.SetProperty(AskMaterialFlagProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<AndonManage>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion


        #region 序号 OrderNum
        /// <summary>
        /// 序号
        /// </summary>
        [Label("序号")]
        public static readonly Property<string> OrderNumProperty = P<AndonManage>.RegisterView(e => e.OrderNum, p => p.Andon.OrderNo);

        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNum
        {
            get { return this.GetProperty(OrderNumProperty); }
        }
        #endregion


        #region 安灯类型名称 AndonTypeName
        /// <summary>
        /// 安灯类型名称
        /// </summary>
        [Label("安灯类型名称")]
        public static readonly Property<string> AndonTypeNameProperty = P<AndonManage>.RegisterView(e => e.AndonTypeName, p => p.AndonType.AndonTypeName);

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName
        {
            get { return this.GetProperty(AndonTypeNameProperty); }
            set { this.SetProperty(AndonTypeNameProperty, value); }
        }
        #endregion

        #region 安灯名称 AndonName
        /// <summary>
        /// 安灯名称
        /// </summary>
        [Label("安灯名称")]
        public static readonly Property<string> AndonNameProperty = P<AndonManage>.RegisterView(e => e.AndonName, p => p.Andon.AndonName);

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName
        {
            get { return this.GetProperty(AndonNameProperty); }
            set { this.SetProperty(AndonNameProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<AndonManage>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }
        #endregion


        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<AndonManage>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion


        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<AndonManage>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion


        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<AndonManage>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 触发人 TriggerByName
        /// <summary>
        /// 触发人
        /// </summary>
        [Label("触发人")]
        public static readonly Property<string> TriggerByNameProperty = P<AndonManage>.RegisterView(e => e.TriggerByName, p => p.Trigger.Name);

        /// <summary>
        /// 触发人
        /// </summary>
        public string TriggerByName
        {
            get { return this.GetProperty(TriggerByNameProperty); }
        }
        #endregion

        #region 处理人 HandlerName
        /// <summary>
        /// 处理人
        /// </summary>
        [Label("处理人")]
        public static readonly Property<string> HandlerNameProperty = P<AndonManage>.RegisterView(e => e.HandlerName, p => p.Handler.Name);

        /// <summary>
        /// 处理人
        /// </summary>
        public string HandlerName
        {
            get { return this.GetProperty(HandlerNameProperty); }
        }
        #endregion


        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<AndonManage>.RegisterView(e => e.AndonCode, p => p.Andon.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
        }
        #endregion

        #region 通用问题描述 GeneralProbDtlDesc
        /// <summary>
        /// 通用问题描述
        /// </summary>
        [Label("通用问题描述")]
        public static readonly Property<string> GeneralProbDtlDescProperty = P<AndonManage>.RegisterView(e => e.GeneralProbDtlDesc, p => p.GeneralProbDtl.Desc);

        /// <summary>
        /// 通用问题描述
        /// </summary>
        public string GeneralProbDtlDesc
        {
            get { return this.GetProperty(GeneralProbDtlDescProperty); }
        }
        #endregion

        #region 安灯编码 WipResourceAndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> WipResourceAndonCodeProperty = P<AndonManage>.RegisterView(e => e.WipResourceAndonCode, p => p.WipResource.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string WipResourceAndonCode
        {
            get { return this.GetProperty(WipResourceAndonCodeProperty); }
        }
        #endregion

        #endregion

        #region 状态描述 StateDesc
        /// <summary>
        /// 状态描述
        /// </summary>
        [Label("状态描述")]
        public static readonly Property<string> StateDescProperty = P<AndonManage>.RegisterReadOnly(
            e => e.StateDesc, e => e.GetStateDesc(), StateProperty);

        /// <summary>
        /// 状态描述
        /// </summary>

        public string StateDesc
        {
            get { return this.GetProperty(StateDescProperty); }
        }

        private string GetStateDesc()
        {
            return State.ToLabel().L10N();
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<AndonManage>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 不映射数据库

        #region 安灯控件显示 AndonEventControlShow
        /// <summary>
        /// 安灯控件显示
        /// </summary>
        [Label("安灯控件显示")]
        public static readonly Property<string> AndonEventControlShowProperty = P<AndonManage>.RegisterReadOnly(
            e => e.AndonEventControlShow, e => e.GetAndonEventControlShow(), AndonManageCodeProperty);
        /// <summary>
        /// 安灯控件显示
        /// </summary>

        public string AndonEventControlShow
        {
            get { return this.GetProperty(AndonEventControlShowProperty); }
        }
        private string GetAndonEventControlShow()
        {
            return AndonName + "/" + WipResourceName;
        }
        #endregion

        #region 响应时间 ResponseTime
        /// <summary>
        /// 响应时间
        /// </summary>
        [Label("响应时间")]
        public static readonly Property<DateTime?> ResponseTimeProperty = P<AndonManage>.Register(e => e.ResponseTime);

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime? ResponseTime
        {
            get { return this.GetProperty(ResponseTimeProperty); }
            set { this.SetProperty(ResponseTimeProperty, value); }
        }
        #endregion

        #region 处理时间 HandleTime
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<DateTime?> HandleTimeProperty = P<AndonManage>.Register(e => e.HandleTime);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime
        {
            get { return this.GetProperty(HandleTimeProperty); }
            set { this.SetProperty(HandleTimeProperty, value); }
        }
        #endregion

        #region 验收时间 CheckTime
        /// <summary>
        /// 验收时间
        /// </summary>
        [Label("验收时间")]
        public static readonly Property<DateTime?> CheckTimeProperty = P<AndonManage>.Register(e => e.CheckTime);

        /// <summary>
        /// 验收时间
        /// </summary>
        public DateTime? CheckTime
        {
            get { return this.GetProperty(CheckTimeProperty); }
            set { this.SetProperty(CheckTimeProperty, value); }
        }
        #endregion

        #region 响应时长 ResponseHour
        /// <summary>
        /// 响应时长
        /// </summary>
        [Label("响应时长")]
        public static readonly Property<decimal?> ResponseHourProperty = P<AndonManage>.Register(e => e.ResponseHour);

        /// <summary>
        /// 响应时长
        /// </summary>
        public decimal? ResponseHour
        {
            get { return this.GetProperty(ResponseHourProperty); }
            set { this.SetProperty(ResponseHourProperty, value); }
        }
        #endregion

        #region 处理时长 HandleHour
        /// <summary>
        /// 处理时长
        /// </summary>
        [Label("处理时长")]
        public static readonly Property<decimal?> HandleHourProperty = P<AndonManage>.Register(e => e.HandleHour);

        /// <summary>
        /// 处理时长
        /// </summary>
        public decimal? HandleHour
        {
            get { return this.GetProperty(HandleHourProperty); }
            set { this.SetProperty(HandleHourProperty, value); }
        }
        #endregion

        #region 验收时长 CheckHour
        /// <summary>
        /// 验收时长
        /// </summary>
        [Label("验收时长")]
        public static readonly Property<decimal?> CheckHourProperty = P<AndonManage>.Register(e => e.CheckHour);

        /// <summary>
        /// 验收时长
        /// </summary>
        public decimal? CheckHour
        {
            get { return this.GetProperty(CheckHourProperty); }
            set { this.SetProperty(CheckHourProperty, value); }
        }
        #endregion

        #region 持续时长 Duration
        /// <summary>
        /// 持续时长
        /// </summary>
        [Label("持续时长")]
        public static readonly Property<decimal?> DurationProperty = P<AndonManage>.Register(e => e.Duration);

        /// <summary>
        /// 持续时长
        /// </summary>
        public decimal? Duration
        {
            get { return this.GetProperty(DurationProperty); }
            set { this.SetProperty(DurationProperty, value); }
        }
        #endregion

        #region 响应人 Responser
        /// <summary>
        /// 响应人
        /// </summary>
        [Label("响应人")]
        public static readonly Property<string> ResponserProperty = P<AndonManage>.Register(e => e.Responser);

        /// <summary>
        /// 响应人
        /// </summary>
        public string Responser
        {
            get { return this.GetProperty(ResponserProperty); }
            set { this.SetProperty(ResponserProperty, value); }
        }
        #endregion

        #region 验收人 Checker
        /// <summary>
        /// 验收人
        /// </summary>
        [Label("验收人")]
        public static readonly Property<string> CheckerProperty = P<AndonManage>.Register(e => e.Checker);

        /// <summary>
        /// 验收人
        /// </summary>
        public string Checker
        {
            get { return this.GetProperty(CheckerProperty); }
            set { this.SetProperty(CheckerProperty, value); }
        }
        #endregion

        #region 处理人  Handler2
        /// <summary>
        /// 处理人
        /// </summary>
        [Label("处理人")]
        public static readonly Property<string>  Handler2Property = P<AndonManage>.Register(e => e. Handler2);

        /// <summary>
        /// 处理人
        /// </summary>
        public string  Handler2
        {
            get { return this.GetProperty( Handler2Property); }
            set { this.SetProperty( Handler2Property, value); }
        }
        #endregion


        #endregion


        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.Property == AndonManage.EquipAccountCodeProperty)
            {
                if (EquipAccountCode == "1")
                {
                    this.ProblemDesc = "123";
                }
            }
        }

    }

    /// <summary>
    /// 安灯管理实体配置
    /// </summary>
    public class AndonManageConfig : EntityConfig<AndonManage>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(AndonManage.FaultTimeProperty, new RequiredRule());
            rules.AddRule(AndonManage.TriggerTimeProperty, new RequiredRule());
            rules.AddRule(AndonManage.LineStopProperty, new RequiredRule());
            rules.AddRule(AndonManage.AskMaterialProperty, new RequiredRule());
            base.AddValidations(rules);
        }

        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONMANAGE").MapAllProperties();
            Meta.Property(AndonManage.ProblemDescProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.SolutionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.ReasonProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.HandleMethodProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.MeasuresProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.PhotoFileProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.DefectIdsProperty).ColumnMeta.HasLength(960);
            Meta.Property(AndonManage.DefectProperty).ColumnMeta.HasLength(960);
            Meta.Property(AndonManage.AttachmentProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManage.HandleTimeProperty).DontMapColumn();
            Meta.Property(AndonManage.ResponseTimeProperty).DontMapColumn();
            Meta.Property(AndonManage.CheckTimeProperty).DontMapColumn();
            Meta.Property(AndonManage.ResponseHourProperty).DontMapColumn();
            Meta.Property(AndonManage.HandleHourProperty).DontMapColumn();
            Meta.Property(AndonManage.CheckHourProperty).DontMapColumn();
            Meta.Property(AndonManage.DurationProperty).DontMapColumn();
            Meta.Property(AndonManage.CheckerProperty).DontMapColumn();
            Meta.Property(AndonManage.ResponserProperty).DontMapColumn();
            Meta.Property(AndonManage.Handler2Property).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
