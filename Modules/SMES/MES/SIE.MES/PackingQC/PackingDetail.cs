using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// 装箱明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("装箱明细")]
    public class PackingDetail:DataEntity
    {
        #region 装箱QC  PackingQc
        /// <summary>
        /// 装箱QCId
        /// </summary>
        [Label("装箱QC")]
        public static readonly IRefIdProperty PackingQcIdProperty =
            P<PackingDetail>.RegisterRefId(e => e.PackingQcId, ReferenceType.Parent);

        /// <summary>
        /// 装箱QCId
        /// </summary>
        public double PackingQcId
        {
            get { return (double)this.GetRefId(PackingQcIdProperty); }
            set { this.SetRefId(PackingQcIdProperty, value); }
        }

        /// <summary>
        /// 装箱QC
        /// </summary>
        public static readonly RefEntityProperty<PackingQc> PackingQcProperty =
            P<PackingDetail>.RegisterRef(e => e.PackingQc, PackingQcIdProperty);

        /// <summary>
        /// 装箱QC
        /// </summary>
        public PackingQc PackingQc
        {
            get { return this.GetRefEntity(PackingQcProperty); }
            set { this.SetRefEntity(PackingQcProperty, value); }
        }
        #endregion

        #region 工序标签 ProductLabel
        /// <summary>
        /// 工序标签
        /// </summary>
        [Required]
        [Label("标签号/SN")]
        public static readonly Property<string> ProductLabelProperty = P<PackingDetail>.Register(e => e.ProductLabel);

        /// <summary>
        /// 工序标签
        /// </summary>
        public string ProductLabel
        {
            get { return this.GetProperty(ProductLabelProperty); }
            set { this.SetProperty(ProductLabelProperty, value); }
        }
        #endregion

        #region 批次标签 BatchLabel
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchLabelProperty = P<PackingDetail>.Register(e => e.BatchLabel);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchLabel
        {
            get { return this.GetProperty(BatchLabelProperty); }
            set { this.SetProperty(BatchLabelProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingDetail>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region (QC)是否确认 Confirm
        /// <summary>
        /// (QC)是否确认
        /// </summary>
        [Label("(QC)是否确认")]
        public static readonly Property<ConfirmEnum> ConfirmProperty = P<PackingDetail>.Register(e => e.Confirm);

        /// <summary>
        /// (QC)是否确认
        /// </summary>
        public ConfirmEnum Confirm
        {
            get { return this.GetProperty(ConfirmProperty); }
            set { this.SetProperty(ConfirmProperty, value); }
        }
        #endregion

        #region 标签类型 LabelType
        /// <summary>
        /// 标签类型
        /// </summary>
        [Label("标签类型")]
        public static readonly Property<LabelTypeEnum> LabelTypeProperty = P<PackingDetail>.Register(e => e.LabelType);

        /// <summary>
        /// 标签类型
        /// </summary>
        public LabelTypeEnum LabelType
        {
            get { return this.GetProperty(LabelTypeProperty); }
            set { this.SetProperty(LabelTypeProperty, value); }
        }
        #endregion

        #region 报工类型 ReportsType
        /// <summary>
        /// 报工类型
        /// </summary>
        [Label("报工类型")]
        public static readonly Property<ReportsTypeEnum> ReportsTypeProperty = P<PackingDetail>.Register(e => e.ReportsType);

        /// <summary>
        /// 报工类型
        /// </summary>
        public ReportsTypeEnum ReportsType
        {
            get { return this.GetProperty(ReportsTypeProperty); }
            set { this.SetProperty(ReportsTypeProperty, value); }
        }
        #endregion

        #region 装箱数量 PackingNum
        /// <summary>
        /// 装箱数量
        /// </summary>
        [Label("装箱数量")]
        public static readonly Property<int> PackingNumProperty = P<PackingDetail>.Register(e => e.PackingNum);

        /// <summary>
        /// 装箱数量
        /// </summary>
        public int PackingNum
        {
            get { return this.GetProperty(PackingNumProperty); }
            set { this.SetProperty(PackingNumProperty, value); }
        }
        #endregion

        #region 测试值 TestValue
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly Property<string> TestValueProperty = P<PackingDetail>.Register(e => e.TestValue);

        /// <summary>
        /// 测试值
        /// </summary>
        public string TestValue
        {
            get { return this.GetProperty(TestValueProperty); }
            set { this.SetProperty(TestValueProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 装箱明细 实体配置
    /// </summary>
    public class PackingDetailConfig : EntityConfig<PackingDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACKING_DETAIL").MapAllProperties();
            Meta.Property(PackingDetail.TestValueProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
