using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking
{
    /// <summary>
    /// 包装QC确认明细
    /// </summary>
    [RootEntity]
    [Label("包装QC确认明细")]
    public class ConnectorSnPackingModel : ViewModel
    {
        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Required]
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<ConnectorSnPackingModel>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ConnectorSnPackingModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<ConnectorSnPackingModel>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemNameProperty = P<ConnectorSnPackingModel>.Register(e => e.ItemName);

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
        public static readonly Property<PackIdentEnum?> PackIdentProperty = P<ConnectorSnPackingModel>.Register(e => e.PackIdent);

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
        public static readonly Property<ConfirmEnum?> ConfirmProperty = P<ConnectorSnPackingModel>.Register(e => e.Confirm);

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
        public static readonly Property<BoxStateEnum?> BoxStateProperty = P<ConnectorSnPackingModel>.Register(e => e.BoxState);

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
        public static readonly Property<int> BlueLableNumProperty = P<ConnectorSnPackingModel>.Register(e => e.BlueLableNum);

        /// <summary>
        /// 蓝标数
        /// </summary>
        public int BlueLableNum
        {
            get { return this.GetProperty(BlueLableNumProperty); }
            set { this.SetProperty(BlueLableNumProperty, value); }
        }
        #endregion

        #region 装箱数量 PackingNum
        /// <summary>
        /// 装箱数量
        /// </summary>
        [Label("装箱数量")]
        public static readonly Property<int> PackingNumProperty = P<ConnectorSnPackingModel>.Register(e => e.PackingNum);

        /// <summary>
        /// 装箱数量
        /// </summary>
        public int PackingNum
        {
            get { return this.GetProperty(PackingNumProperty); }
            set { this.SetProperty(PackingNumProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<ConnectorSnPackingModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<ConnectorSnPackingModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序标签 ProductLabel
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("工序标签")]
        public static readonly Property<string> ProductLabelProperty = P<ConnectorSnPackingModel>.Register(e => e.ProductLabel);

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
        [Label("批次标签")]
        public static readonly Property<string> BatchLabelProperty = P<ConnectorSnPackingModel>.Register(e => e.BatchLabel);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchLabel
        {
            get { return this.GetProperty(BatchLabelProperty); }
            set { this.SetProperty(BatchLabelProperty, value); }
        }
        #endregion
    }
}
