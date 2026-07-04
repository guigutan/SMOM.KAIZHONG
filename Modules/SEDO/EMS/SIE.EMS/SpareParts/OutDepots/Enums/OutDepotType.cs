using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.EMS.SpareParts.OutDepots.Enums
{
    /// <summary>
    /// 出库类型
    /// </summary>
    [Label("出库类型")]
    public enum OutDepotType
    {
        /// <summary>
        /// 其他出库
        /// </summary>
        [Category("Create")]
        [Label("其他出库")]
        Other = 0,

        /// <summary>
        /// 维修出库
        /// </summary>
        [Label("维修出库")]
        Repair = 1,

        /// <summary>
        /// 保养出库
        /// </summary>   
        [Label("保养出库")]
        Maintain = 2,

        /// <summary>
        /// 点检出库
        /// </summary>
        [Label("点检出库")]
        Check = 3,

        /// <summary>
        /// 采购退货
        /// </summary>
        [Category("Create")]
        [Label("采购退货")]
        Pucharse = 4,
        /// <summary>
        /// 委外维修出库
        /// </summary>
        [Label("委外维修出库")]
        [Category("Create")]
        DgMaintain = 5,
        /// <summary>
        /// 安装调试
        /// </summary>
        [Label("安装调试")]
        Setup = 6,
        /// <summary>
        /// 润滑出库
        /// </summary>
        [Label("润滑出库")]
        Lubrication = 7,

        /// <summary>
        /// 报废出库
        /// </summary>
        [Label("报废出库")] 
        Scrap = 8
    }
}
