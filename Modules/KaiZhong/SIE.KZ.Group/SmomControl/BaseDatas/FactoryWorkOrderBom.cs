using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Group.SmomControl.BaseDatas
{
    /// <summary>
    /// 工厂工单BOM
    /// </summary>
    [RootEntity, Serializable]
    [Label("工厂工单BOM")]
    public class FactoryWorkOrderBom : FactoryBase
    {
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<FactoryWorkOrderBom>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FactoryWorkOrderBom>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FactoryWorkOrderBom>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 移动类型 Bwart
        /// <summary>
        /// 移动类型
        /// </summary>
        [Label("移动类型")]
        public static readonly Property<string> BwartProperty = P<FactoryWorkOrderBom>.Register(e => e.Bwart);

        /// <summary>
        /// 移动类型
        /// </summary>
        public string Bwart
        {
            get { return this.GetProperty(BwartProperty); }
            set { this.SetProperty(BwartProperty, value); }
        }
        #endregion

        #region 需求量 RequireQty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> RequireQtyProperty = P<FactoryWorkOrderBom>.Register(e => e.RequireQty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty
        {
            get { return GetProperty(RequireQtyProperty); }
            set { SetProperty(RequireQtyProperty, value); }
        }
        #endregion

        #region 单位耗用量 SingleQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> SingleQtyProperty = P<FactoryWorkOrderBom>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 工单完工数 FinishQty
        /// <summary>
        /// 工单完工数
        /// </summary>
        [Label("工单完工数")]
        public static readonly Property<decimal> FinishQtyProperty = P<FactoryWorkOrderBom>.Register(e => e.FinishQty);

        /// <summary>
        /// 工单完工数
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

    }

    internal class FactoryWorkOrderBomConfig : EntityConfig<FactoryWorkOrderBom>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("FACTORY_WO_BOM_V").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisablePhantoms();
        }
    }


}
