using SIE.Domain;
using SIE.Items;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工序Bom
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序Bom")]
    public partial class RoutingBomDetail : DataEntity
    {
        #region 工艺路线工序 RoutingProcess
        /// <summary>
        /// 工艺路线工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty RoutingProcessIdProperty =
            P<RoutingBomDetail>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线工序Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)this.GetRefId(RoutingProcessIdProperty); }
            set { this.SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线工序
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty =
            P<RoutingBomDetail>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工艺路线工序
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return this.GetRefEntity(RoutingProcessProperty); }
            set { this.SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 单位用量 Amount
        /// <summary>
        /// 单位用量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("单位用量")]
        public static readonly Property<decimal> AmountProperty = P<RoutingBomDetail>.Register(e => e.Amount);

        /// <summary>
        /// 单位用量
        /// </summary>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 是否附件 IsAttachment
        /// <summary>
        /// 是否附件
        /// </summary>
        [Required]
        [Label("是否附件")]
        public static readonly Property<bool> IsAttachmentProperty = P<RoutingBomDetail>.Register(e => e.IsAttachment);

        /// <summary>
        /// 是否附件
        /// </summary>
        public bool IsAttachment
        {
            get { return GetProperty(IsAttachmentProperty); }
            set { SetProperty(IsAttachmentProperty, value); }
        }
        #endregion

        #region 物料 Material
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty MaterialIdProperty =
            P<RoutingBomDetail>.RegisterRefId(e => e.MaterialId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double MaterialId
        {
            get { return (double)GetRefId(MaterialIdProperty); }
            set { SetRefId(MaterialIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> MaterialProperty =
            P<RoutingBomDetail>.RegisterRef(e => e.Material, MaterialIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Material
        {
            get { return GetRefEntity(MaterialProperty); }
            set { SetRefEntity(MaterialProperty, value); }
        }
        #endregion

        #region 主料编码 MainMaterial
        /// <summary>
        /// 主料编码Id
        /// </summary>
        [Label("主料编码")]
        public static readonly IRefIdProperty MainMaterialIdProperty =
            P<RoutingBomDetail>.RegisterRefId(e => e.MainMaterialId, ReferenceType.Normal);

        /// <summary>
        /// 主料编码Id
        /// </summary>
        public double? MainMaterialId
        {
            get { return (double?)GetRefNullableId(MainMaterialIdProperty); }
            set { SetRefNullableId(MainMaterialIdProperty, value); }
        }

        /// <summary>
        /// 主料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> MainMaterialProperty =
            P<RoutingBomDetail>.RegisterRef(e => e.MainMaterial, MainMaterialIdProperty);

        /// <summary>
        /// 主料编码
        /// </summary>
        public Item MainMaterial
        {
            get { return GetRefEntity(MainMaterialProperty); }
            set { SetRefEntity(MainMaterialProperty, value); }
        }
        #endregion

        #region 工艺路线版本 RoutingBom
        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public static readonly IRefIdProperty RoutingBomIdProperty =
            P<RoutingBomDetail>.RegisterRefId(e => e.RoutingBomId, ReferenceType.Parent);

        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public double RoutingBomId
        {
            get { return (double)GetRefId(RoutingBomIdProperty); }
            set { SetRefId(RoutingBomIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public static readonly RefEntityProperty<RoutingBom> RoutingBomProperty =
            P<RoutingBomDetail>.RegisterRef(e => e.RoutingBom, RoutingBomIdProperty);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public RoutingBom RoutingBom
        {
            get { return GetRefEntity(RoutingBomProperty); }
            set { SetRefEntity(RoutingBomProperty, value); }
        }
        #endregion

        #region 工步 WorkStep
        /// <summary>
        /// 工步Id
        /// </summary>
        [Label("工步")]
        public static readonly IRefIdProperty WorkStepIdProperty =
            P<RoutingBomDetail>.RegisterRefId(e => e.WorkStepId, ReferenceType.Normal);

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? WorkStepId
        {
            get { return (double?)GetRefNullableId(WorkStepIdProperty); }
            set { SetRefNullableId(WorkStepIdProperty, value); }
        }

        /// <summary>
        /// 工步
        /// </summary>
        public static readonly RefEntityProperty<WorkStep> WorkStepProperty =
            P<RoutingBomDetail>.RegisterRef(e => e.WorkStep, WorkStepIdProperty);

        /// <summary>
        /// 工步
        /// </summary>
        public WorkStep WorkStep
        {
            get { return GetRefEntity(WorkStepProperty); }
            set { SetRefEntity(WorkStepProperty, value); }
        }
        #endregion

        #region 备注 Description
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(300)]
        [Label("备注")]
        public static readonly Property<string> DescriptionProperty = P<RoutingBomDetail>.Register(e => e.Description);

        /// <summary>
        /// 备注
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 日志附件 Attachment
        /// <summary>
        /// 日志附件Id
        /// </summary>
        [Label("日志附件")]
        public static readonly IRefIdProperty AttachmentIdProperty =
            P<RoutingBomDetail>.RegisterRefId(e => e.AttachmentId, ReferenceType.Normal);

        /// <summary>
        /// 日志附件Id
        /// </summary>
        public double? AttachmentId
        {
            get { return (double?)this.GetRefNullableId(AttachmentIdProperty); }
            set { this.SetRefNullableId(AttachmentIdProperty, value); }
        }

        /// <summary>
        /// 日志附件
        /// </summary>
        public static readonly RefEntityProperty<RoutingBomAttachment> AttachmentProperty =
            P<RoutingBomDetail>.RegisterRef(e => e.Attachment, AttachmentIdProperty);

        /// <summary>
        /// 日志附件
        /// </summary>
        public RoutingBomAttachment Attachment
        {
            get { return this.GetRefEntity(AttachmentProperty); }
            set { this.SetRefEntity(AttachmentProperty, value); }
        }
        #endregion

        #region 单体条码 IsSingleLabel
        /// <summary>
        /// 单体条码
        /// </summary>
        [Required]
        [Label("单体条码")]
        public static readonly Property<bool> IsSingleLabelProperty = P<RoutingBomDetail>.Register(e => e.IsSingleLabel);

        /// <summary>
        /// 单体条码
        /// </summary>
        public bool IsSingleLabel
        {
            get { return GetProperty(IsSingleLabelProperty); }
            set { SetProperty(IsSingleLabelProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessNameForImport
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameForImportProperty = P<RoutingBomDetail>.Register(e => e.ProcessNameForImport);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessNameForImport
        {
            get { return this.GetProperty(ProcessNameForImportProperty); }
            set { this.SetProperty(ProcessNameForImportProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<RoutingBomDetail>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示值 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示值
        /// </summary>
        [Label("物料扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<RoutingBomDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 扩展属性编辑 IsAllowEdit
        /// <summary>
        /// 扩展属性编辑
        /// </summary>
        [Label("扩展属性编辑")]
        public static readonly Property<bool> IsAllowEditProperty = P<RoutingBomDetail>.Register(e => e.IsAllowEdit);

        /// <summary>
        /// 扩展属性编辑
        /// </summary>
        public bool IsAllowEdit
        {
            get { return this.GetProperty(IsAllowEditProperty); }
            set { this.SetProperty(IsAllowEditProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 MaterialCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> MaterialCodeProperty
            = P<RoutingBomDetail>.RegisterView(e => e.MaterialCode, p => p.Material.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode
        {
            get { return this.GetProperty(MaterialCodeProperty); }
        }
        #endregion

        #region 物料名称 MaterialName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> MaterialNameProperty =
            P<RoutingBomDetail>.RegisterView(e => e.MaterialName, p => p.Material.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName
        {
            get { return this.GetProperty(MaterialNameProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty =
            P<RoutingBomDetail>.RegisterView(e => e.ProductCode, p => p.RoutingBom.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 工艺路线版本 RoutingVersion
        /// <summary>
        /// 工艺路线版本
        /// </summary>
        [Label("工艺路线版本")]
        public static readonly Property<string> RoutingVersionProperty =
            P<RoutingBomDetail>.RegisterView(e => e.RoutingVersion, p => p.RoutingBom.RoutingVersion.Name);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string RoutingVersion
        {
            get { return this.GetProperty(RoutingVersionProperty); }
        }
        #endregion

        #region 工段 Segment
        /// <summary>
        /// 工段
        /// </summary>
        [Label("工段")]
        public static readonly Property<string> SegmentProperty =
            P<RoutingBomDetail>.RegisterView(e => e.Segment, p => p.RoutingBom.ProcessSegment.Name);

        /// <summary>
        /// 工段
        /// </summary>
        public string Segment
        {
            get { return this.GetProperty(SegmentProperty); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty =
            P<RoutingBomDetail>.RegisterView(e => e.SpecificationModel, p => p.Material.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
        }
        #endregion

        #region 单位Id MaterialUnitId
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位Id")]
        public static readonly Property<double> MaterialUnitIdProperty =
            P<RoutingBomDetail>.RegisterView(e => e.MaterialUnitId, p => p.Material.UnitId);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double MaterialUnitId
        {
            get { return this.GetProperty(MaterialUnitIdProperty); }
        }
        #endregion

        #region 单位 MaterialUnit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> MaterialUnitProperty =
            P<RoutingBomDetail>.RegisterView(e => e.MaterialUnit, p => p.Material.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string MaterialUnit
        {
            get { return this.GetProperty(MaterialUnitProperty); }
        }
        #endregion

        #region 工序顺序 ProcessIndex
        /// <summary>
        /// 工序顺序
        /// </summary>
        [Label("工序顺序")]
        public static readonly Property<int> ProcessIndexProperty = P<RoutingBomDetail>.RegisterView(e => e.ProcessIndex, p => p.RoutingProcess.Index);

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int ProcessIndex
        {
            get { return this.GetProperty(ProcessIndexProperty); }
        }
        #endregion

        #region 主料物料编码 MainMaterialCode
        /// <summary>
        /// 主料物料编码
        /// </summary>
        [Label("主料物料编码")]
        public static readonly Property<string> MainMaterialCodeProperty
            = P<RoutingBomDetail>.RegisterView(e => e.MainMaterialCode, p => p.MainMaterial.Code);

        /// <summary>
        /// 主料物料编码
        /// </summary>
        public string MainMaterialCode
        {
            get { return this.GetProperty(MainMaterialCodeProperty); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<RoutingBomDetail>.RegisterView(e => e.ProcessId, p => p.RoutingProcess.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<RoutingBomDetail>.RegisterView(e => e.ProcessName, p => p.RoutingProcess.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工序Bom 实体配置
    /// </summary>
    internal class RoutingBomDetailConfig : EntityConfig<RoutingBomDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECT_ROUTING_BOM_DETAIL").MapAllPropertiesExcept(RoutingBomDetail.IsAllowEditProperty);
            Meta.Property(RoutingBomDetail.ProcessNameForImportProperty).DontMapColumn();            
            Meta.Property(RoutingBomDetail.RoutingBomIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}