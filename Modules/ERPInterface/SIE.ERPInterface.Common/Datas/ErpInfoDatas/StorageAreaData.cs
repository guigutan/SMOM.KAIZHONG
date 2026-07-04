namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 库区信息
    /// </summary>
    public class StorageAreaData : ErpInfoData
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int LibraryType { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 状态  
        /// </summary>
        public int State { get; set; }
    }
}
