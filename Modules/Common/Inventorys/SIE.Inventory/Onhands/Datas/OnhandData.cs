using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SIE.Domain;
using SIE.Inventory.TransactionProcessing;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存查询参数
    /// </summary>
    public class OnhandData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OnhandData()
        {
            Param = new InvOptionalParam();
        }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 批次编码
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 库存查询可选参数（如果为空，默认保存为空格）:货主、LPN、项目号、任务号
        /// </summary>
        public InvOptionalParam Param { get; set; }
    }

    /// <summary>
    /// 包含库存9个属性
    /// </summary>
    public class OnhandQueryDataBase
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// Lpn
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 比较数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? OnhandState { get; set; }

        /// <summary>
        /// 允许没有库存不报错
        /// </summary>
        public bool AllowEmpty { get; set; }
    }

    /// <summary>
    /// 库存查询数据
    /// </summary>
    public class SearchOnHandData
    {
        /// <summary>
        /// 总数量(现有量)
        /// </summary>
        public decimal TotalCount { get; set; }

        /// <summary>
        /// 数据条数
        /// </summary>
        public int ListCount { get; set; }

        /// <summary>
        /// 库存数据
        /// </summary>
        public EntityList<LotLpnOnhand> LotLpnOnhandList { get; set; } = new EntityList<LotLpnOnhand>();
    }
}
