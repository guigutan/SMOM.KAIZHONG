using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.InventoryControl
{
    /// <summary>
    /// 库存对照表子表
    /// </summary>
    [RootEntity, Serializable]
    public class InventoryControlDetailViewModel: ViewModel
    {
        #region 序号
        /// <summary>
        /// 序号
        /// </summary>
        [Label("序号")]
        public static readonly Property<int> LineNoProperty = P<InventoryControlDetailViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 序号
        /// </summary>
        public int LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region ERP子库
        /// <summary>
        /// ERP子库
        /// </summary>
        [Label("ERP子库")]
        public static readonly Property<string> ErpWareHouseCodeProperty = P<InventoryControlDetailViewModel>.Register(e => e.ErpWareHouseCode);

        /// <summary>
        /// ERP子库
        /// </summary>
        public string ErpWareHouseCode
        {
            get { return this.GetProperty(ErpWareHouseCodeProperty); }
            set { this.SetProperty(ErpWareHouseCodeProperty, value); }
        }
        #endregion

        #region 仓库名称
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WareHouseCodeProperty = P<InventoryControlDetailViewModel>.Register(e => e.WareHouseCode);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseCode
        {
            get { return this.GetProperty(WareHouseCodeProperty); }
            set { this.SetProperty(WareHouseCodeProperty, value); }
        }
        #endregion

        #region 现有量
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal?> QtyProperty = P<InventoryControlDetailViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal? Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 差异数
        /// <summary>
        /// 差异数
        /// </summary>
        [Label("差异数")]
        public static readonly Property<decimal> DifferenceQtyProperty = P<InventoryControlDetailViewModel>.Register(e => e.DifferenceQty);

        /// <summary>
        /// 差异数
        /// </summary>
        public decimal DifferenceQty
        {
            get { return this.GetProperty(DifferenceQtyProperty); }
            set { this.SetProperty(DifferenceQtyProperty, value); }
        }
        #endregion

        #region 暂收数
        /// <summary>
        /// 暂收数
        /// </summary>
        [Label("暂收数")]
        public static readonly Property<decimal?> RecQtyProperty = P<InventoryControlDetailViewModel>.Register(e => e.RecQty);

        /// <summary>
        /// 暂收数
        /// </summary>
        public decimal? RecQty
        {
            get { return this.GetProperty(RecQtyProperty); }
            set { this.SetProperty(RecQtyProperty, value); }
        }
        #endregion

        #region ERP现有量
        /// <summary>
        /// ERP现有量
        /// </summary>
        [Label("ERP现有量")]
        public static readonly Property<decimal> ErpQtyProperty = P<InventoryControlDetailViewModel>.Register(e => e.ErpQty);

        /// <summary>
        /// ERP现有量
        /// </summary>
        public decimal ErpQty
        {
            get { return this.GetProperty(ErpQtyProperty); }
            set { this.SetProperty(ErpQtyProperty, value); }
        }
        #endregion

        #region 父序号
        /// <summary>
        /// 父序号
        /// </summary>
        [Label("父序号")]
        public static readonly Property<int> ParentLineNoProperty = P<InventoryControlDetailViewModel>.Register(e => e.ParentLineNo);

        /// <summary>
        /// 父序号
        /// </summary>
        public int ParentLineNo
        {
            get { return this.GetProperty(ParentLineNoProperty); }
            set { this.SetProperty(ParentLineNoProperty, value); }
        }
        #endregion

        //#region 子库明细 InventoryControlErpDetaiViewModel
        ///// <summary>
        ///// 子库明细
        ///// </summary>
        //[Label("子库明细")]
        //public static readonly ListProperty<EntityList<InventoryControlErpDetaiViewModel>> InventoryControlErpListProperty = P<InventoryControlDetailViewModel>.RegisterList(e => e.InventoryControlErpList);

        ///// <summary>
        ///// 子库明细
        ///// </summary>
        //public EntityList<InventoryControlErpDetaiViewModel> InventoryControlErpList
        //{
        //    get { return this.GetLazyList(InventoryControlErpListProperty); }
        //}
        //#endregion
    }
}
