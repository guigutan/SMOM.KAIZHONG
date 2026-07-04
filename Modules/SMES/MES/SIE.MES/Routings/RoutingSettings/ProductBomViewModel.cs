using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 产品生产BOM ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品工序BOM")]
    public class ProductBomViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductBomViewModel()
        {
            Qty = 1;
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProductBomViewModel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ProductBomViewModel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ProductBomViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ProductBomViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ProductBomViewModel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料名称 Name
        /// <summary>
        /// 物料名称 用作显示使用
        /// </summary>
        [Label("物料名称")]
        [MaxLength(240)]
        public static readonly Property<string> NameProperty = P<ProductBomViewModel>.Register(e => e.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 物料编码 Code
        /// <summary>
        /// 物料编码 用作显示使用
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<ProductBomViewModel>.Register(e => e.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<ProductBomViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性名称 ItemExtPropName
        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ProductBomViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 是否允许编辑扩展属性 IsAllowEdit
        /// <summary>
        /// 是否允许编辑扩展属性
        /// </summary>
        [Label("是否允许编辑扩展属性")]
        public static readonly Property<bool> IsAllowEditProperty = P<ProductBomViewModel>.Register(e => e.IsAllowEdit);

        /// <summary>
        /// 是否允许编辑扩展属性
        /// </summary>
        public bool IsAllowEdit
        {
            get { return this.GetProperty(IsAllowEditProperty); }
            set { this.SetProperty(IsAllowEditProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量"), MinValue(0.000000000001)]
        public static readonly Property<decimal> QtyProperty = P<ProductBomViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 是否扣料 IsBuckleMaterial
        /// <summary>
        /// 是否扣料
        /// </summary>
        [Label("是否扣料")]
        public static readonly Property<bool> IsBuckleMaterialProperty = P<ProductBomViewModel>.Register(e => e.IsBuckleMaterial);

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool IsBuckleMaterial
        {
            get { return this.GetProperty(IsBuckleMaterialProperty); }
            set { this.SetProperty(IsBuckleMaterialProperty, value); }
        }
        #endregion

        #region 工步 WorkStep
        /// <summary>
        /// 工步Id
        /// </summary>
        [Label("工步")]
        public static readonly IRefIdProperty WorkStepIdProperty =
            P<ProductBomViewModel>.RegisterRefId(e => e.WorkStepId, ReferenceType.Normal);

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? WorkStepId
        {
            get { return (double?)this.GetRefNullableId(WorkStepIdProperty); }
            set { this.SetRefNullableId(WorkStepIdProperty, value); }
        }

        /// <summary>
        /// 工步
        /// </summary>
        public static readonly RefEntityProperty<WorkStep> WorkStepProperty =
            P<ProductBomViewModel>.RegisterRef(e => e.WorkStep, WorkStepIdProperty);

        /// <summary>
        /// 工步
        /// </summary>
        public WorkStep WorkStep
        {
            get { return this.GetRefEntity(WorkStepProperty); }
            set { this.SetRefEntity(WorkStepProperty, value); }
        }
        #endregion

        #region 工步代码 WorkStepCode
        /// <summary>
        /// 工步代码
        /// </summary>
        [Label("工步代码")]
        public static readonly Property<string> WorkStepCodeProperty = P<ProductBomViewModel>.Register(e => e.WorkStepCode);

        /// <summary>
        /// 工步代码
        /// </summary>
        public string WorkStepCode
        {
            get { return this.GetProperty(WorkStepCodeProperty); }
            set { this.SetProperty(WorkStepCodeProperty, value); }
        }
        #endregion

        #region 工步名称 WorkStepName
        /// <summary>
        /// 工步名称
        /// </summary>
        [Label("工步名称")]
        public static readonly Property<string> WorkStepNameProperty = P<ProductBomViewModel>.Register(e => e.WorkStepName);

        /// <summary>
        /// 工步名称
        /// </summary>
        public string WorkStepName
        {
            get { return this.GetProperty(WorkStepNameProperty); }
            set { this.SetProperty(WorkStepNameProperty, value); }
        }
        #endregion

        #region 替代组 Alter
        /// <summary>
        /// 替代组
        /// </summary>
        [Label("替代组")]
        public static readonly Property<string> AlterProperty = P<ProductBomViewModel>.Register(e => e.Alter);

        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter
        {
            get { return this.GetProperty(AlterProperty); }
            set { this.SetProperty(AlterProperty, value); }
        }
        #endregion

        #region 替代组合分组 AlternativeGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlternativeGroupProperty = P<ProductBomViewModel>.Register(e => e.AlternativeGroup);

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlternativeGroup
        {
            get { return this.GetProperty(AlternativeGroupProperty); }
            set { this.SetProperty(AlternativeGroupProperty, value); }
        }
        #endregion

        #region 是否附件 IsAttachment
        /// <summary>
        /// 是否附件
        /// </summary>
        [Label("是否附件")]
        public static readonly Property<bool> IsAttachmentProperty = P<ProductBomViewModel>.Register(e => e.IsAttachment);

        /// <summary>
        /// 是否附件
        /// </summary>
        public bool IsAttachment
        {
            get { return this.GetProperty(IsAttachmentProperty); }
            set { this.SetProperty(IsAttachmentProperty, value); }
        }
        #endregion

        #region 系统外条码 IsExternal
        /// <summary>
        /// 系统外条码
        /// </summary>
        [Label("系统外条码")]
        public static readonly Property<bool> IsExternalProperty = P<ProductBomViewModel>.Register(e => e.IsExternal);

        /// <summary>
        /// 系统外条码
        /// </summary>
        public bool IsExternal
        {
            get { return this.GetProperty(IsExternalProperty); }
            set { this.SetProperty(IsExternalProperty, value); }
        }
        #endregion

        #region 单体条码 IsSingleLabel
        /// <summary>
        /// 单体条码
        /// </summary>
        [Label("单体条码")]
        public static readonly Property<bool> IsSingleLabelProperty = P<ProductBomViewModel>.Register(e => e.IsSingleLabel);

        /// <summary>
        /// 单体条码
        /// </summary>
        public bool IsSingleLabel
        {
            get { return this.GetProperty(IsSingleLabelProperty); }
            set { this.SetProperty(IsSingleLabelProperty, value); }
        }
        #endregion

        #region 是否可重复 IsRepeat
        /// <summary>
        /// 是否可重复
        /// </summary>
        [Label("是否可重复")]
        public static readonly Property<bool> IsRepeatProperty = P<ProductBomViewModel>.Register(e => e.IsRepeat);

        /// <summary>
        /// 是否可重复
        /// </summary>
        public bool IsRepeat
        {
            get { return this.GetProperty(IsRepeatProperty); }
            set { this.SetProperty(IsRepeatProperty, value); }
        }
        #endregion

        #region 条码解析 HasBarcodeRule
        /// <summary>
        /// 条码解析
        /// </summary>
        [Label("条码解析")]
        public static readonly Property<bool> HasBarcodeRuleProperty = P<ProductBomViewModel>.Register(e => e.HasBarcodeRule);

        /// <summary>
        /// 条码解析
        /// </summary>
        public bool HasBarcodeRule
        {
            get { return this.GetProperty(HasBarcodeRuleProperty); }
            set { this.SetProperty(HasBarcodeRuleProperty, value); }
        }
        #endregion
    }
}