using Microsoft.Extensions.FileProviders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.Products
{

    /// <summary>
    /// 产品维修记录缺陷列表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品维修记录换料列表")]
    public partial class WipProductRepairReplaceRecord : DataEntity
    {

        #region 产品维修记录 WipProductRepair
        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public static readonly IRefIdProperty WipProductRepairIdProperty = P<WipProductRepairReplaceRecord>.RegisterRefId(e => e.WipProductRepairId, ReferenceType.Parent);

        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public double WipProductRepairId
        {
            get { return (double)GetRefId(WipProductRepairIdProperty); }
            set { SetRefId(WipProductRepairIdProperty, value); }
        }

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductRepair> WipProductRepairProperty = P<WipProductRepairReplaceRecord>.RegisterRef(e => e.WipProductRepair, WipProductRepairIdProperty);

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public WipProductRepair WipProductRepair
        {
            get { return GetRefEntity(WipProductRepairProperty); }
            set { SetRefEntity(WipProductRepairProperty, value); }
        }
        #endregion


        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<WipProductRepairReplaceRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<WipProductRepairReplaceRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 原标签号 LabelNo
        /// <summary>
        /// 原标签号
        /// </summary>
        [Label("原标签号")]
        public static readonly Property<string> LabelNoProperty = P<WipProductRepairReplaceRecord>.Register(e => e.LabelNo);

        /// <summary>
        /// 原标签号
        /// </summary>
        public string LabelNo
        {
            get { return GetProperty(LabelNoProperty); }
            set { SetProperty(LabelNoProperty, value); }
        }
        #endregion


        #region 新标签号 NewLabeNo
        /// <summary>
        /// 新标签号
        /// </summary>
        [Label("新标签号")]
        public static readonly Property<string> NewLabeNoProperty = P<WipProductRepairReplaceRecord>.Register(e => e.NewLabeNo);

        /// <summary>
        /// 原标签号
        /// </summary>
        public string NewLabeNo
        {
            get { return GetProperty(NewLabeNoProperty); }
            set { SetProperty(NewLabeNoProperty, value); }
        }
        #endregion
       
        #region 换料量 Qty
        /// <summary>
        /// 换料量
        /// </summary>
        [Label("换料量")]
        public static readonly Property<decimal> QtyProperty = P<WipProductRepairReplaceRecord>.Register(e => e.Qty);

        /// <summary>
        /// 换料量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion


        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WipProductRepairReplaceRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WipProductRepairReplaceRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 产品维修记录缺陷列表 实体配置
    /// </summary>
    internal class WipProductRepairReplaceRecordConfig : EntityConfig<WipProductRepairReplaceRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_REP_REPLACE_REC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
