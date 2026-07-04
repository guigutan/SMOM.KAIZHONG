using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.VictoryStandards;
using System;

namespace SIE.Wpf.MES.WorkOrders.ViewModels
{
    /// <summary>
    /// 工单工艺路线工序属性ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单状态改变")]
    public class WorkOrderRoutingProcessProperty : ViewModel
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 创建Sku CreateSku
        /// <summary>
        /// 创建Sku
        /// </summary>
        [Label("创建Sku")]
        public static readonly Property<bool> CreateSkuProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.CreateSku);

        /// <summary>
        /// 创建Sku
        /// </summary>
        public bool CreateSku
        {
            get { return this.GetProperty(CreateSkuProperty); }
            set { this.SetProperty(CreateSkuProperty, value); }
        }
        #endregion

        #region 是否可选 IsOptional
        /// <summary>
        /// 是否可选
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<bool> IsOptionalProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsOptional);

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool IsOptional
        {
            get { return GetProperty(IsOptionalProperty); }
            set { SetProperty(IsOptionalProperty, value); }
        }
        #endregion

        #region 重复过站 IsRepeat
        /// <summary>
        /// 重复过站
        /// </summary>
        [Label("重复过站")]
        public static readonly Property<bool> IsRepeatProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsRepeat);

        /// <summary>
        /// 重复过站
        /// </summary>
        public bool IsRepeat
        {
            get { return GetProperty(IsRepeatProperty); }
            set { SetProperty(IsRepeatProperty, value); }
        }
        #endregion 

        #region 是否生成任务单 IsGenerateTask
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        [Label("是否生成任务单")]
        public static readonly Property<bool> IsGenerateTaskProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool IsGenerateTask
        {
            get { return GetProperty(IsGenerateTaskProperty); }
            set { SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion

        #region 是否扣料 IsBuckleMaterial
        /// <summary>
        /// 是否扣料
        /// </summary>
        [Label("是否扣料")]
        public static readonly Property<bool> IsBuckleMaterialProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsBuckleMaterial);

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool IsBuckleMaterial
        {
            get { return this.GetProperty(IsBuckleMaterialProperty); }
            set { this.SetProperty(IsBuckleMaterialProperty, value); }
        }
        #endregion

        #region 直通率取值 IsPassRate
        /// <summary>
        /// 直通率取值
        /// </summary>
        [Label("直通率取值")]
        public static readonly Property<bool> IsPassRateProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsPassRate);

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool IsPassRate
        {
            get { return this.GetProperty(IsPassRateProperty); }
            set { this.SetProperty(IsPassRateProperty, value); }
        }
        #endregion

        #region 绑定 IsBinding
        /// <summary>
        /// 绑定
        /// </summary>
        public static readonly Property<bool> IsBindingProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsBinding);

        /// <summary>
        /// 绑定
        /// </summary>
        public bool IsBinding
        {
            get { return GetProperty(IsBindingProperty); }
            set { SetProperty(IsBindingProperty, value); }
        }
        #endregion

        #region 解绑 IsPassRate
        /// <summary>
        /// 解绑
        /// </summary>
        public static readonly Property<bool> IsUnBindingProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsUnBinding);

        /// <summary>
        /// 解绑
        /// </summary>
        public bool IsUnBinding
        {
            get { return GetProperty(IsUnBindingProperty); }
            set { SetProperty(IsUnBindingProperty, value); }
        }
        #endregion

        #region 起始工序 StartProcess
        /// <summary>
        /// 起始工序
        /// </summary>
        [Label("起始工序")]
        public static readonly Property<double?> StartProcessProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.StartProcess);

        /// <summary>
        /// 起始工序
        /// </summary>
        public double? StartProcess
        {
            get { return this.GetProperty(StartProcessProperty); }
            set { this.SetProperty(StartProcessProperty, value); }
        }
        #endregion

        #region 正常胜制 NormalVictory
        /// <summary>
        /// 正常胜制Id
        /// </summary>
        [Label("正常胜制")]
        public static readonly IRefIdProperty NormalVictoryIdProperty =
            P<WorkOrderRoutingProcessProperty>.RegisterRefId(e => e.NormalVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 正常胜制Id
        /// </summary>
        public double? NormalVictoryId
        {
            get { return (double?)this.GetRefNullableId(NormalVictoryIdProperty); }
            set { this.SetRefNullableId(NormalVictoryIdProperty, value); }
        }

        /// <summary>
        /// 正常胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> NormalVictoryProperty =
            P<WorkOrderRoutingProcessProperty>.RegisterRef(e => e.NormalVictory, NormalVictoryIdProperty);

        /// <summary>
        /// 正常胜制
        /// </summary>
        public VictoryStandard NormalVictory
        {
            get { return this.GetRefEntity(NormalVictoryProperty); }
            set { this.SetRefEntity(NormalVictoryProperty, value); }
        }
        #endregion 

        #region 维修胜制 RepairVictory
        /// <summary>
        /// 维修胜制Id
        /// </summary>
        [Label("维修胜制")]
        public static readonly IRefIdProperty RepairVictoryIdProperty =
            P<WorkOrderRoutingProcessProperty>.RegisterRefId(e => e.RepairVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 维修胜制Id
        /// </summary>
        public double? RepairVictoryId
        {
            get { return (double?)this.GetRefNullableId(RepairVictoryIdProperty); }
            set { this.SetRefNullableId(RepairVictoryIdProperty, value); }
        }

        /// <summary>
        /// 维修胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> RepairVictoryProperty =
            P<WorkOrderRoutingProcessProperty>.RegisterRef(e => e.RepairVictory, RepairVictoryIdProperty);

        /// <summary>
        /// 维修胜制
        /// </summary>
        public VictoryStandard RepairVictory
        {
            get { return this.GetRefEntity(RepairVictoryProperty); }
            set { this.SetRefEntity(RepairVictoryProperty, value); }
        }
        #endregion

        #region 是否加严 IsStricter
        /// <summary>
        /// 是否加严
        /// </summary>
        [Label("加严")]
        public static readonly Property<bool> IsStricterProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.IsStricter);

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool IsStricter
        {
            get { return this.GetProperty(IsStricterProperty); }
            set { this.SetProperty(IsStricterProperty, value); }
        }
        #endregion 

        #region 超时时间（分钟） Overtime
        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        [Label("超时时间（分钟）")]
        public static readonly Property<int?> OvertimeProperty = P<WorkOrderRoutingProcessProperty>.Register(e => e.Overtime);

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime
        {
            get { return this.GetProperty(OvertimeProperty); }
            set { this.SetProperty(OvertimeProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 工单工艺路线工序属性ViewModel视图配置
    /// </summary>
    public class WorkOrderRoutingProcessViewConfig : WPFViewConfig<WorkOrderRoutingProcessProperty>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(1);
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.CreateSku).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.IsOptional).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.IsRepeat).Show(ShowInWhere.Detail).Readonly();
                View.Property(p => p.IsGenerateTask).Show(ShowInWhere.Detail).Readonly();
            }
        }
    }
}