using SIE.Barcodes.WipBatchs;
using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.ItemLine;
using SIE.MES.PackingQC.Configs;
using SIE.MES.WIP.Pressure.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// 装箱QC确认
    /// </summary>
    /// <summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PackingQcCriterial))]
    [EntityWithConfig(typeof(PackingQCVerifyCodeConfig))]
    [Label("装箱QC确认")]
    public class PackingQc : DataEntity
    {
        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Required]
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<PackingQc>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 原蓝标 OldBlueLabel
        /// <summary>
        /// 原蓝标
        /// </summary>
        [Label("原蓝标")]
        public static readonly Property<string> OldBlueLabelProperty = P<PackingQc>.Register(e => e.OldBlueLabel);

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string OldBlueLabel
        {
            get { return this.GetProperty(OldBlueLabelProperty); }
            set { this.SetProperty(OldBlueLabelProperty, value); }
        }
        #endregion

        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<PackingQc>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<PackingQc>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
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
        public static readonly Property<string> ItemNameProperty = P<PackingQc>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 装箱标识 PackIdent
        /// <summary>
        /// 装箱标识
        /// </summary>
        [Label("装箱标识")]
        public static readonly Property<PackIdentEnum?> PackIdentProperty = P<PackingQc>.Register(e => e.PackIdent);

        /// <summary>
        /// 装箱标识
        /// </summary>
        public PackIdentEnum? PackIdent
        {
            get { return this.GetProperty(PackIdentProperty); }
            set { this.SetProperty(PackIdentProperty, value); }
        }
        #endregion

        #region (QC)是否确认 Confirm
        /// <summary>
        /// (QC)是否确认
        /// </summary>
        [Label("(QC)是否确认")]
        public static readonly Property<ConfirmEnum?> ConfirmProperty = P<PackingQc>.Register(e => e.Confirm);

        /// <summary>
        /// (QC)是否确认
        /// </summary>
        public ConfirmEnum? Confirm
        {
            get { return this.GetProperty(ConfirmProperty); }
            set { this.SetProperty(ConfirmProperty, value); }
        }
        #endregion

        #region 箱子状态 BoxState
        /// <summary>
        /// 箱子状态
        /// </summary>
        [Label("箱子状态")]
        public static readonly Property<BoxStateEnum?> BoxStateProperty = P<PackingQc>.Register(e => e.BoxState);

        /// <summary>
        /// 箱子状态
        /// </summary>
        public BoxStateEnum? BoxState
        {
            get { return this.GetProperty(BoxStateProperty); }
            set { this.SetProperty(BoxStateProperty, value); }
        }
        #endregion

        #region 蓝标数 BlueLableNum
        /// <summary>
        /// 蓝标数
        /// </summary>
        [Label("蓝标数")]
        public static readonly Property<int> BlueLableNumProperty = P<PackingQc>.Register(e => e.BlueLableNum);

        /// <summary>
        /// 蓝标数
        /// </summary>
        public int BlueLableNum
        {
            get { return this.GetProperty(BlueLableNumProperty); }
            set { this.SetProperty(BlueLableNumProperty, value); }
        }
        #endregion

        #region 装箱数 PackingNum
        /// <summary>
        /// 装箱数
        /// </summary>
        [Label("装箱数")]
        public static readonly Property<int> PackingNumProperty = P<PackingQc>.Register(e => e.PackingNum);

        /// <summary>
        /// 装箱数
        /// </summary>
        public int PackingNum
        {
            get { return this.GetProperty(PackingNumProperty); }
            set { this.SetProperty(PackingNumProperty, value); }
        }
        #endregion

        #region 未装箱数 UnboxedQty
        /// <summary>
        /// 未装箱数
        /// </summary>
        [Label("未装箱数")]
        public static readonly Property<decimal> UnboxedQtyProperty = P<PackingQc>.RegisterReadOnly(
            e => e.UnboxedQty, e => e.GetUnboxedQty(), BlueLableNumProperty, PackingNumProperty);
        /// <summary>
        /// 未装箱数
        /// </summary>

        public decimal UnboxedQty
        {
            get { return this.GetProperty(UnboxedQtyProperty); }
        }
        private decimal GetUnboxedQty()
        {
            var qty = BlueLableNum - PackingNum;
            return qty;
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PackingQc>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<PackingQc>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 报工类型 ReportsType
        /// <summary>
        /// 报工类型
        /// </summary>
        [Label("报工类型")]
        public static readonly Property<ReportsTypeEnum> ReportsTypeProperty = P<PackingQc>.Register(e => e.ReportsType);

        /// <summary>
        /// 报工类型
        /// </summary>
        public ReportsTypeEnum ReportsType
        {
            get { return this.GetProperty(ReportsTypeProperty); }
            set { this.SetProperty(ReportsTypeProperty, value); }
        }
        #endregion

        #region 附件 DocList
        /// <summary>
        /// 附件
        /// </summary>
        [Label("附件")]
        public static readonly ListProperty<EntityList<PackingQcAttachment>> DocListProperty = P<PackingQc>.RegisterList(e => e.DocList);

        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<PackingQcAttachment> DocList
        {
            get { return this.GetLazyList(DocListProperty); }
        }
        #endregion

        #region 装箱明细 PackingDetailList
        /// <summary>
        /// 装箱明细
        /// </summary>
        [Label("装箱明细")]
        public static readonly ListProperty<EntityList<PackingDetail>> PackingDetailListProperty = P<PackingQc>.RegisterList(e => e.PackingDetailList);

        /// <summary>
        /// 装箱明细
        /// </summary>
        public EntityList<PackingDetail> PackingDetailList
        {
            get { return this.GetLazyList(PackingDetailListProperty); }
        }
        #endregion


        #region 是否已上传 IsUploadSap
        /// <summary>
        /// 是否已上传
        /// </summary>
        [Label("是否已上传")]
        public static readonly Property<bool?> IsUploadSapProperty = P<PackingQc>.Register(e => e.IsUploadSap);

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool? IsUploadSap
        {
            get { return this.GetProperty(IsUploadSapProperty); }
            set { this.SetProperty(IsUploadSapProperty, value); }
        }
        #endregion

        #region 上传结果 UploadResult
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<PackingQc>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion

        #region 不映射到数据库
        #region 工序标签 ProductLabel
        /// <summary>
        /// 工序标签(多个标签的话用逗号隔开)
        /// </summary>
        [Label("工序标签")]
        public static readonly Property<string> ProductLabelProperty = P<PackingQc>.Register(e => e.ProductLabel);

        /// <summary>
        /// 工序标签(多个标签的话用逗号隔开)
        /// </summary>
        public string ProductLabel
        {
            get { return this.GetProperty(ProductLabelProperty); }
            set { this.SetProperty(ProductLabelProperty, value); }
        }
        #endregion
        #endregion

        #region 视图属性
        #region 旧料号(凯中旧料号) ShortDescription
        /// <summary>
        /// 旧料号(凯中旧料号)
        /// </summary>
        [Label("旧料号(凯中旧料号)")]
        public static readonly Property<string> ShortDescriptionProperty = P<PackingQc>.RegisterView(e => e.ShortDescription, p => p.Item.ShortDescription);

        /// <summary>
        /// 旧料号(凯中旧料号)
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #endregion

    }

    /// <summary>
    /// 装箱QC附件 实体配置
    /// </summary>
    public class PackingQcConfig : EntityConfig<PackingQc>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACKING_QC").MapAllProperties();
            Meta.Property(PackingQc.ProductLabelProperty).DontMapColumn();
            Meta.Property(PackingQc.UnboxedQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 装箱标识
    /// </summary>
    public enum PackIdentEnum
    {
        /// <summary>
        /// 满箱
        /// </summary>
        [Label("满箱")]
        FullTank = 0,

        /// <summary>
        /// 不满箱
        /// </summary>
        [Label("不满箱")]
        NotFullTank = 1,
    }

    /// <summary>
    /// QC确认
    /// </summary>
    public enum ConfirmEnum
    {
        /// <summary>
        /// 是
        /// </summary>
        [Label("是")]
        YES = 0,

        /// <summary>
        /// 否
        /// </summary>
        [Label("否")]
        NO = 1,
    }

    /// <summary>
    /// 箱子状态
    /// </summary>
    public enum BoxStateEnum
    {
        /// <summary>
        /// 开箱
        /// </summary>
        [Label("开箱")]
        YES = 0,

        /// <summary>
        /// 封箱
        /// </summary>
        [Label("封箱")]
        NO = 1,
    }

    /// <summary>
    /// 标签状态
    /// </summary>
    public enum LabelTypeEnum
    {
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        BatchLabel = 0,

        /// <summary>
        /// 耐压SN标签
        /// </summary>
        [Label("耐压SN标签")]
        SnLabel = 1,

        /// <summary>
        /// 刻码SN标签
        /// </summary>
        [Label("刻码SN标签")]
        KmLabel =2,
    }

    /// <summary>
    /// 标签状态
    /// </summary>
    public enum ReportsTypeEnum
    {
        /// <summary>
        /// 是
        /// </summary>
        [Label("是")]
        YES = 0,

        /// <summary>
        /// 否
        /// </summary>
        [Label("否")]
        NO = 1,
    }
}
