using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺资料-产品工艺路线明细")]
    [DisplayMember(nameof(ProcessName))]
    public class DesignTreeRoutingDetail : DataEntity
    {
        #region 产品工艺路线设置 DesignTreeRouting
        /// <summary>
        /// 产品工艺路线设置Id
        /// </summary>
        [Label("产品工艺路线设置")]
        public static readonly IRefIdProperty DesignTreeRoutingIdProperty =
            P<DesignTreeRoutingDetail>.RegisterRefId(e => e.DesignTreeRoutingId, ReferenceType.Parent);

        /// <summary>
        /// 产品工艺路线设置Id
        /// </summary>
        public double DesignTreeRoutingId
        {
            get { return (double)this.GetRefId(DesignTreeRoutingIdProperty); }
            set { this.SetRefId(DesignTreeRoutingIdProperty, value); }
        }

        /// <summary>
        /// 产品工艺路线设置
        /// </summary>
        public static readonly RefEntityProperty<DesignTreeRouting> DesignTreeRoutingProperty =
            P<DesignTreeRoutingDetail>.RegisterRef(e => e.DesignTreeRouting, DesignTreeRoutingIdProperty);

        /// <summary>
        /// 产品工艺路线设置
        /// </summary>
        public DesignTreeRouting DesignTreeRouting
        {
            get { return this.GetRefEntity(DesignTreeRoutingProperty); }
            set { this.SetRefEntity(DesignTreeRoutingProperty, value); }
        }
        #endregion

        #region 顺序 Index
        /// <summary>
        /// 顺序
        /// </summary>
        [Label("顺序")]
        public static readonly Property<int> IndexProperty = P<DesignTreeRoutingDetail>.Register(e => e.Index);

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index
        {
            get { return this.GetProperty(IndexProperty); }
            set { this.SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<DesignTreeRoutingDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<DesignTreeRoutingDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<DesignTreeRoutingDetail>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType?> ProcessTypeProperty = P<DesignTreeRoutingDetail>.RegisterView(e => e.ProcessType, p => p.Process.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType? ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
        }
        #endregion

        #region 工段名称 SegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> SegmentNameProperty = P<DesignTreeRoutingDetail>.RegisterView(e => e.SegmentName, p => p.Process.Segment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string SegmentName
        {
            get { return this.GetProperty(SegmentNameProperty); }
        }
        #endregion

        #region 是否可选 IsOptional
        /// <summary>
        /// 是否可选
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<bool> IsOptionalProperty = P<DesignTreeRoutingDetail>.Register(e => e.IsOptional);

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool IsOptional
        {
            get { return GetProperty(IsOptionalProperty); }
            set { SetProperty(IsOptionalProperty, value); }
        }
        #endregion

        #region 是否委外 Outsourcing
        /// <summary>
        /// 是否委外
        /// </summary>
        [Label("是否委外")]
        public static readonly Property<bool> OutsourcingProperty = P<DesignTreeRoutingDetail>.Register(e => e.Outsourcing);

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool Outsourcing
        {
            get { return this.GetProperty(OutsourcingProperty); }
            set { this.SetProperty(OutsourcingProperty, value); }
        }
        #endregion

        #region 是否生成工序任务 IsGenerateTask
        /// <summary>
        /// 是否生成工序任务
        /// </summary>        
        [Label("是否生成工序任务")]
        public static readonly Property<bool> IsGenerateTaskProperty = P<DesignTreeRoutingDetail>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否生成工序任务
        /// </summary>
        public bool IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
            set { this.SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion


        #region 是否需求任务清单 IsGenerateTask
        /// <summary>
        /// 是否需求任务清单
        /// </summary>        
        [Label("是否需求任务清单")]
        public static readonly Property<bool> IsRequirementTaskProperty = P<DesignTreeRoutingDetail>.Register(e => e.IsRequirementTask);

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool IsRequirementTask
        {
            get { return this.GetProperty(IsRequirementTaskProperty); }
            set { this.SetProperty(IsRequirementTaskProperty, value); }
        }
        #endregion

        #region 节拍 Beat
        /// <summary>
        /// 节拍
        /// </summary>
        [Label("节拍")]
        public static readonly Property<decimal?> BeatProperty = P<DesignTreeRoutingDetail>.Register(e => e.Beat);

        /// <summary>
        /// 节拍
        /// </summary>
        public decimal? Beat
        {
            get { return this.GetProperty(BeatProperty); }
            set { this.SetProperty(BeatProperty, value); }
        }
        #endregion

        #region 直接人工费用 DirectCost
        /// <summary>
        /// 直接人工费用
        /// </summary>
        [Label("直接人工费用")]
        public static readonly Property<decimal?> DirectCostProperty = P<DesignTreeRoutingDetail>.Register(e => e.DirectCost);

        /// <summary>
        /// 直接人工费用
        /// </summary>
        public decimal? DirectCost
        {
            get { return this.GetProperty(DirectCostProperty); }
            set { this.SetProperty(DirectCostProperty, value); }
        }
        #endregion

        #region 间接人工费用 InDirectCost
        /// <summary>
        /// 间接人工费用
        /// </summary>
        [Label("间接人工费用")]
        public static readonly Property<decimal?> InDirectCostProperty = P<DesignTreeRoutingDetail>.Register(e => e.InDirectCost);

        /// <summary>
        /// 间接人工费用
        /// </summary>
        public decimal? InDirectCost
        {
            get { return this.GetProperty(InDirectCostProperty); }
            set { this.SetProperty(InDirectCostProperty, value); }
        }
        #endregion

        #region 能源费用 EnergyCost
        /// <summary>
        /// 能源费用
        /// </summary>
        [Label("能源费用")]
        public static readonly Property<decimal?> EnergyCostProperty = P<DesignTreeRoutingDetail>.Register(e => e.EnergyCost);

        /// <summary>
        /// 能源费用
        /// </summary>
        public decimal? EnergyCost
        {
            get { return this.GetProperty(EnergyCostProperty); }
            set { this.SetProperty(EnergyCostProperty, value); }
        }
        #endregion

        #region 其他费用 OtherCost
        /// <summary>
        /// 其他费用
        /// </summary>
        [Label("其他费用")]
        public static readonly Property<decimal?> OtherCostProperty = P<DesignTreeRoutingDetail>.Register(e => e.OtherCost);

        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal? OtherCost
        {
            get { return this.GetProperty(OtherCostProperty); }
            set { this.SetProperty(OtherCostProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeRoutingDetailConfig : EntityConfig<DesignTreeRoutingDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_ROUDTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
