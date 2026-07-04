using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Smom.InventoryControl
{
    /// <summary>
    /// 库存对照表视图
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InventoryControlViewModelCriteria))]
    [Label("库存对照表")]
    public class InventoryControlViewModel:Entity<double>
    {
        #region 物料编码
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<InventoryControlViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<InventoryControlViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion 

        #region ERP批次号
        /// <summary>
        /// ERP批次号
        /// </summary>
        [Label("ERP批次号")]
        public static readonly Property<string> ErpLotCodeProperty = P<InventoryControlViewModel>.Register(e => e.ErpLotCode);

        /// <summary>
        /// ERP批次号
        /// </summary>
        public string ErpLotCode
        {
            get { return this.GetProperty(ErpLotCodeProperty); }
            set { this.SetProperty(ErpLotCodeProperty, value); }
        }
        #endregion

        #region 现有量
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal> QtyProperty = P<InventoryControlViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region ERP现有量
        /// <summary>
        /// ERP现有量
        /// </summary>
        [Label("ERP现有量")]
        public static readonly Property<decimal> ErpQtyProperty = P<InventoryControlViewModel>.Register(e => e.ErpQty);

        /// <summary>
        /// ERP现有量
        /// </summary>
        public decimal ErpQty
        {
            get { return this.GetProperty(ErpQtyProperty); }
            set { this.SetProperty(ErpQtyProperty, value); }
        }
        #endregion

        #region 差异数
        /// <summary>
        /// 差异数
        /// </summary>
        [Label("差异数")]
        public static readonly Property<decimal> DifferenceQtyProperty = P<InventoryControlViewModel>.Register(e => e.DifferenceQty);

        /// <summary>
        /// 差异数
        /// </summary>
        public decimal DifferenceQty
        {
            get { return this.GetProperty(DifferenceQtyProperty); }
            set { this.SetProperty(DifferenceQtyProperty, value); }
        }
        #endregion

        #region 暂收库存
        /// <summary>
        /// 暂收库存
        /// </summary>
        [Label("暂收库存")]
        public static readonly Property<decimal> TemporaryQtyProperty = P<InventoryControlViewModel>.Register(e => e.TemporaryQty);

        /// <summary>
        /// 暂收库存
        /// </summary>
        public decimal TemporaryQty
        {
            get { return this.GetProperty(TemporaryQtyProperty); }
            set { this.SetProperty(TemporaryQtyProperty, value); }
        }
        #endregion

        #region 仓库名称
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WareHouseCodeProperty = P<InventoryControlViewModel>.Register(e => e.WareHouseCode);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseCode
        {
            get { return this.GetProperty(WareHouseCodeProperty); }
            set { this.SetProperty(WareHouseCodeProperty, value); }
        }
        #endregion

        #region 仓库Id
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库Id")]
        public static readonly Property<double> WareHouseIdProperty = P<InventoryControlViewModel>.Register(e => e.WareHouseId);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public double WareHouseId
        {
            get { return this.GetProperty(WareHouseIdProperty); }
            set { this.SetProperty(WareHouseIdProperty, value); }
        }
        #endregion

        #region 物料ID
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料ID")]
        public static readonly Property<double> ItemIdProperty = P<InventoryControlViewModel>.Register(e => e.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
            set { this.SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region ERP子库
        /// <summary>
        /// ERP子库
        /// </summary>
        [Label("ERP子库")]
        public static readonly Property<string> ErpWareHouseCodeProperty = P<InventoryControlViewModel>.Register(e => e.ErpWareHouseCode);

        /// <summary>
        /// ERP子库
        /// </summary>
        public string ErpWareHouseCode
        {
            get { return this.GetProperty(ErpWareHouseCodeProperty); }
            set { this.SetProperty(ErpWareHouseCodeProperty, value); }
        }
        #endregion

        #region 单位
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitCodeProperty = P<InventoryControlViewModel>.Register(e => e.UnitCode);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitCode
        {
            get { return this.GetProperty(UnitCodeProperty); }
            set { this.SetProperty(UnitCodeProperty, value); }
        }
        #endregion

        #region 规格型号
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<InventoryControlViewModel>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 序号
        /// <summary>
        /// 序号
        /// </summary>
        [Label("序号")]
        public static readonly Property<int> LineNoProperty = P<InventoryControlViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 序号
        /// </summary>
        public int LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        //#region 仓库明细 InventoryControlDetailList
        ///// <summary>
        ///// 仓库明细
        ///// </summary>
        //[Label("仓库明细")]
        //public static readonly ListProperty<EntityList<InventoryControlDetailViewModel>> InventoryControlDetailListProperty = P<InventoryControlViewModel>.RegisterList(e => e.InventoryControlDetailList);

        ///// <summary>
        ///// 送货明细列表
        ///// </summary>
        //public EntityList<InventoryControlDetailViewModel> InventoryControlDetailList
        //{
        //    get { return this.GetLazyList(InventoryControlDetailListProperty); }
        //}
        //#endregion

        #region 批次号
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<InventoryControlViewModel>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

    }
}
