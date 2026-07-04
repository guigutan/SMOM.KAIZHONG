using System;
using System.Collections.Generic;

namespace SIE.Fixtures.FixtureDemands.ViewModels
{
    /// <summary>
    /// 出库信息
    /// </summary>
    [Serializable]
    public class UnloadInfo
    {
        /// <summary>
        /// 当前主界面仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 工治具需求清单Id
        /// </summary>
        public double DemandId { get; set; }

        /// <summary>
        /// 工治具需求明细
        /// </summary>
        public FixtureDemandDetail DemandDetail { get; set; }

        /// <summary>
        /// 库存情况ViewModel
        /// </summary>
        public UnloadStockViewModel UnloadStockVM { get; set; }

        /// <summary>
        /// 出库明细ViewModel
        /// </summary>
        public FixtureUnloadViewModel FixtureUnloadVM { get; set; }

        /// <summary>
        /// 剩下的出库明细ViewModel列表
        /// </summary>
        public List<FixtureUnloadViewModel> RestUnloadVMList { get; set; } = new List<FixtureUnloadViewModel>();

        /// <summary>
        /// 库存情况ViewModel列表
        /// </summary>
        public List<UnloadStockViewModel> UnloadStockVMList { get; set; } = new List<UnloadStockViewModel>();

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
