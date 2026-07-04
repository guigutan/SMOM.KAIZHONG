namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 库位信息
    /// </summary>
    public class StorageLocationData : ErpInfoData
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int LibraryType { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 状态  
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 对应ERP库存组织  
        /// </summary>
        public string ErpInvOrg { get; set; }

        /// <summary>
        /// 对应ERP子库  
        /// </summary>
        public string ErpSubLibrary { get; set; }

        /// <summary>
        /// 对应ERP库位  
        /// </summary>
        public string ErpLocation { get; set; }
    }
}
