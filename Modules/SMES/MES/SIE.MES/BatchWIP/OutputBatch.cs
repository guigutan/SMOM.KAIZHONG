using SIE.Barcodes.WipBatchs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.Items;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 转出批次视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("转出批次")]
    public partial class OutputBatch : ViewModel
    {
        #region 生产批号 BatchNo
        /// <summary>
        /// 生产批号
        /// </summary>
        [Label("生产批号")]
        public static readonly Property<string> BatchNoProperty = P<OutputBatch>.Register(e => e.BatchNo);

        /// <summary>
        /// 生产批号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 子批次号 SubBatchNo
        /// <summary>
        /// 子批次号
        /// </summary>
        [Label("子批次号")]
        public static readonly Property<string> SubBatchNoProperty = P<OutputBatch>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 子批次号
        /// </summary>
        public string SubBatchNo
        {
            get { return this.GetProperty(SubBatchNoProperty); }
            set { this.SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<OutputBatch>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<OutputBatch>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 最大数量 MaxQty
        /// <summary>
        /// 最大数量
        /// </summary>
        [Label("最大数量")]
        public static readonly Property<decimal?> MaxQtyProperty = P<OutputBatch>.Register(e => e.MaxQty);

        /// <summary>
        /// 最大数量
        /// </summary>
        public decimal? MaxQty
        {
            get { return this.GetProperty(MaxQtyProperty); }
            set { this.SetProperty(MaxQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<OutputBatch>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 出站时间 InputDate
        /// <summary>
        /// 出站时间
        /// </summary>
        [Label("出站时间")]
        public static readonly Property<DateTime> InputDateProperty = P<OutputBatch>.Register(e => e.InputDate);

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime InputDate
        {
            get { return this.GetProperty(InputDateProperty); }
            set { this.SetProperty(InputDateProperty, value); }
        }
        #endregion 

        #region 出站时间 OutputDate
        /// <summary>
        /// 出站时间
        /// </summary>
        [Label("出站时间")]
        public static readonly Property<DateTime> OutputDateProperty = P<OutputBatch>.Register(e => e.OutputDate);

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime OutputDate
        {
            get { return this.GetProperty(OutputDateProperty); }
            set { this.SetProperty(OutputDateProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<OutputBatch>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 是否不良 IsNg
        /// <summary>
        /// 是否不良
        /// </summary>
        [Label("是否不良")]
        public static readonly Property<bool> IsNgProperty = P<OutputBatch>.Register(e => e.IsNg);

        /// <summary>
        /// 是否不良
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 产品等级 Grade
        /// <summary>
        /// 产品等级Id
        /// </summary>
        [Label("产品等级")]
        public static readonly IRefIdProperty GradeIdProperty =
            P<OutputBatch>.RegisterRefId(e => e.GradeId, ReferenceType.Normal);

        /// <summary>
        /// 产品等级Id
        /// </summary>
        public double? GradeId
        {
            get { return (double?)this.GetRefNullableId(GradeIdProperty); }
            set { this.SetRefNullableId(GradeIdProperty, value); }
        }

        /// <summary>
        /// 产品等级
        /// </summary>
        public static readonly RefEntityProperty<ProductGrade> GradeProperty =
            P<OutputBatch>.RegisterRef(e => e.Grade, GradeIdProperty);

        /// <summary>
        /// 产品等级
        /// </summary>
        public ProductGrade Grade
        {
            get { return this.GetRefEntity(GradeProperty); }
            set { this.SetRefEntity(GradeProperty, value); }
        }
        #endregion

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型
        /// </summary>
        [Label("条码类型")]
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<OutputBatch>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return this.GetProperty(BarcodeTypeProperty); }
            set { this.SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion

        #region 是否生成新批次 isGenerateBatch 
        /// <summary>
        /// 是否生成新批次
        /// </summary>
        [Label("是否生成新批次")]
        public static readonly Property<bool> IsGenerateBatchProperty = P<OutputBatch>.Register(e => e.IsGenerateBatch);

        /// <summary>
        /// 是否生成新批次
        /// </summary>
        public bool IsGenerateBatch
        {
            get { return this.GetProperty(IsGenerateBatchProperty); }
            set { this.SetProperty(IsGenerateBatchProperty, value); }
        }
        #endregion

        #region 新生成子批 SubWipBatch
        /// <summary>
        /// 新生成子批
        /// </summary>
        [Label("新生成子批")]
        public static readonly Property<SubWipBatch> SubWipBatchProperty = P<OutputBatch>.Register(e => e.SubWipBatch);

        /// <summary>
        /// 新生成子批
        /// </summary>
        public SubWipBatch SubWipBatch
        {
            get { return this.GetProperty(SubWipBatchProperty); }
            set { this.SetProperty(SubWipBatchProperty, value); }
        }
        #endregion 

        #region 关联批次列表 RelationBatchList
        /// <summary>
        /// 关联批次列表
        /// </summary>
        [Label("关联批次批次")]
        public static readonly ListProperty<EntityList<RelationBatch>> RelationBatchListProperty = P<OutputBatch>.RegisterList(e => e.RelationBatchList);

        /// <summary>
        /// 关联批次列表
        /// </summary>
        public EntityList<RelationBatch> RelationBatchList
        {
            get { return this.GetLazyList(RelationBatchListProperty); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<OutputBatch>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单ID
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
            P<OutputBatch>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示内容
        /// </summary>
        /// <returns>内容</returns>
        public override string ToString()
        {
            System.Diagnostics.Debug.WriteLine("***************转出批次*************");
            string msg = "转出批次：生产批号[{0}]、子批次号[{1}]、载具号[{2}]、数量[{3}]、是否新批次[{4}]".FormatArgs(BatchNo, SubBatchNo, ContainerNo, Qty, IsGenerateBatch);
            string relation = string.Empty;
            RelationBatchList.ForEach(e =>
            {
                relation += "\r\n关联批次：\t\t生产批号[{0}]、子批次号[{1}]、载具号[{2}]、拆分数量[{3}]".FormatArgs(e.InputBatch.BatchNo, e.InputBatch.SubBatchNo, e.InputBatch.ContainerNo, e.Qty);
            });
            return msg + relation;
        }
    }

    /// <summary>
    /// 关联批次
    /// </summary>
    [ChildEntity, Serializable]
    [Label("关联批次")]
    public class RelationBatch : ViewModel
    {
        #region 出站批次 OutputBatch
        /// <summary>
        /// 出站批次Id
        /// </summary>
        [Label("出站批次")]
        public static readonly IRefIdProperty OutputBatchIdProperty =
            P<RelationBatch>.RegisterRefId(e => e.OutputBatchId, ReferenceType.Parent);

        /// <summary>
        /// 出站批次Id
        /// </summary>
        public string OutputBatchId
        {
            get { return (string)this.GetRefId(OutputBatchIdProperty); }
            set { this.SetRefId(OutputBatchIdProperty, value); }
        }

        /// <summary>
        /// 出站批次
        /// </summary>
        public static readonly RefEntityProperty<OutputBatch> OutputBatchProperty =
            P<RelationBatch>.RegisterRef(e => e.OutputBatch, OutputBatchIdProperty);

        /// <summary>
        /// 出站批次
        /// </summary>
        public OutputBatch OutputBatch
        {
            get { return this.GetRefEntity(OutputBatchProperty); }
            set { this.SetRefEntity(OutputBatchProperty, value); }
        }
        #endregion 

        #region 入站批次 InputBatch
        /// <summary>
        /// 入站批次Id
        /// </summary>
        [Label("入站批次")]
        public static readonly IRefIdProperty InputBatchIdProperty =
            P<RelationBatch>.RegisterRefId(e => e.InputBatchId, ReferenceType.Normal);

        /// <summary>
        /// 入站批次Id
        /// </summary>
        public double InputBatchId
        {
            get { return (double)this.GetRefId(InputBatchIdProperty); }
            set { this.SetRefId(InputBatchIdProperty, value); }
        }

        /// <summary>
        /// 入站批次
        /// </summary>
        public static readonly RefEntityProperty<InputBatch> InputBatchProperty =
            P<RelationBatch>.RegisterRef(e => e.InputBatch, InputBatchIdProperty);

        /// <summary>
        /// 入站批次
        /// </summary>
        public InputBatch InputBatch
        {
            get { return this.GetRefEntity(InputBatchProperty); }
            set { this.SetRefEntity(InputBatchProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        [MinValue(0)]
        public static readonly Property<decimal> QtyProperty = P<RelationBatch>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 产品缺陷记录 BatchWipProductDefect
        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        [Label("产品缺陷记录")]
        public static readonly ListProperty<EntityList<BatchWipProductDefect>> BatchWipProductDefectsProperty = P<RelationBatch>.RegisterList(e => e.BatchWipProductDefects, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchWipProductDefect>()
        });

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public EntityList<BatchWipProductDefect> BatchWipProductDefects
        {
            get { return this.GetLazyList(BatchWipProductDefectsProperty); }
        }
        #endregion 
    }

    /// <summary>
    /// 转出批次数量验证规则
    /// </summary>
    [DisplayName("转出批次数量验证规则")]
    [Description("数量不能大于载具最大数量")]
    public class OutputBatchQtyRule : PropertyRule<OutputBatch>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return OutputBatch.QtyProperty;
            }
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">转出批次</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var batch = entity as OutputBatch;
            if (batch.MaxQty.HasValue && batch.Qty > batch.MaxQty)
                e.BrokenDescription = "数量不能大于{0}最大容量[{1}]".L10nFormat(batch.BarcodeType.ToLabel(), batch.MaxQty);
        }
    }
}