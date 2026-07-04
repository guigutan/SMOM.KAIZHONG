using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品Bom明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺资料-产品Bom明细")]
    public class DesignTreeBomDetail : DataEntity
    {
        #region 产品Bom DesignTreeBom
        /// <summary>
        /// 产品BomId
        /// </summary>
        [Label("产品Bom")]
        public static readonly IRefIdProperty DesignTreeBomIdProperty =
            P<DesignTreeBomDetail>.RegisterRefId(e => e.DesignTreeBomId, ReferenceType.Parent);

        /// <summary>
        /// 产品BomId
        /// </summary>
        public double DesignTreeBomId
        {
            get { return (double)this.GetRefId(DesignTreeBomIdProperty); }
            set { this.SetRefId(DesignTreeBomIdProperty, value); }
        }

        /// <summary>
        /// 产品Bom
        /// </summary>
        public static readonly RefEntityProperty<DesignTreeBom> DesignTreeBomProperty =
            P<DesignTreeBomDetail>.RegisterRef(e => e.DesignTreeBom, DesignTreeBomIdProperty);

        /// <summary>
        /// 产品Bom
        /// </summary>
        public DesignTreeBom DesignTreeBom
        {
            get { return this.GetRefEntity(DesignTreeBomProperty); }
            set { this.SetRefEntity(DesignTreeBomProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<DesignTreeBomDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<DesignTreeBomDetail>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemNameProperty = P<DesignTreeBomDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 单位耗用量 UnitQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> UnitQtyProperty = P<DesignTreeBomDetail>.Register(e => e.UnitQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal UnitQty
        {
            get { return this.GetProperty(UnitQtyProperty); }
            set { this.SetProperty(UnitQtyProperty, value); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<DesignTreeBomDetail>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 损耗率 LossRate
        /// <summary>
        /// 损耗率
        /// </summary>
        [Label("损耗率")]
        public static readonly Property<decimal> LossRateProperty = P<DesignTreeBomDetail>.Register(e => e.LossRate);

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate
        {
            get { return this.GetProperty(LossRateProperty); }
            set { this.SetProperty(LossRateProperty, value); }
        }
        #endregion

        #region 是否反冲物料 IsRecoilItem
        /// <summary>
        /// 是否反冲物料
        /// </summary>
        [Label("是否反冲物料")]
        public static readonly Property<bool> IsRecoilItemProperty = P<DesignTreeBomDetail>.Register(e => e.IsRecoilItem);

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem
        {
            get { return this.GetProperty(IsRecoilItemProperty); }
            set { this.SetProperty(IsRecoilItemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeBomDetailConfig : EntityConfig<DesignTreeBomDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_TREEBOMDTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
