namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class WarehouseData : ErpInfoData
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int LibraryType { get; set; }

        /// <summary>
        /// 简码
        /// </summary>
        public string SimpleCode { get; set; }

        /// <summary>
        /// 状态  
        /// </summary>
        public int State { get; set; }
    }
}
