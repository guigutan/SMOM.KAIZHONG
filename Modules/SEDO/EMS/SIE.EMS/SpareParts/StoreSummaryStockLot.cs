using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 库位明细(带批次信息)
    /// </summary>
    [RootEntity, Serializable]
    [Label("库位明细(带批次信息)")]
    public class StoreSummaryStockLot : StoreSummaryStock
    {
        #region 批次 LotName
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotNameProperty = P<StoreSummaryStockLot>.Register(e => e.LotName);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotName
        {
            get { return this.GetProperty(LotNameProperty); }
            set { this.SetProperty(LotNameProperty, value); }
        }
        #endregion

    }
}
