using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.InventoryControl
{
    /// <summary>
    /// 库存对照表所有视图
    /// </summary>
    [Serializable]
    public class InventroyControlAllData
    {
        /// <summary>
        /// 库存对照表父列表
        /// </summary>
        public List<InventoryControlViewModel> ParentListData { get; set; } = new List<InventoryControlViewModel>();

        /// <summary>
        /// (WMS)库存对照表子列表
        /// </summary>
        public List<InventoryControlDetailViewModel> DetailListData { get; set; } = new List<InventoryControlDetailViewModel>();

        /// <summary>
        /// (ERP)库存对照表子列表
        /// </summary>
        public List<InventoryControlErpDetaiViewModel> ErpListData { get; set; } = new List<InventoryControlErpDetaiViewModel>();
    }
}
