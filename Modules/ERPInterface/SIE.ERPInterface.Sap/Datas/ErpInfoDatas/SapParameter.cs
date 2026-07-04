using System;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// Sap参数
    /// </summary>
    [Serializable]
    public class SapParameter
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string ZBLDH { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public string ZDJLX { get; set; }
        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }
        /// <summary>
        /// 行项目号
        /// </summary>
        public string ZBLDHXM { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MATNR { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MAKTX { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string ZBDMNG { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string MEINS { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string ABLAD { get; set; }

        /// <summary>
        /// 记录调拨出库字段
        /// </summary>
        //[SapIgnore]
        //public string InOut { get; set; }

        /// <summary>
        /// 事务上传ID
        /// </summary>
        public double UploadTransactionId { get; set; }
    }
}
