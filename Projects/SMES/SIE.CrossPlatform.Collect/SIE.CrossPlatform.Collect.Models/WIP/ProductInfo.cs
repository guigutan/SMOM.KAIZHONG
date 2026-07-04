using SIE.CrossPlatform.Collect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 产品信息
    /// </summary>
    [Serializable]
    public class ProductInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductInfo()
        {
            Context = new CollectionContext();
        }

        /// <summary>
        /// 产品Puid
        /// </summary>
        public string Puid { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 拼板码信息
        /// </summary>
        public PanelInfo PanelInfo { get; set; } = new PanelInfo();

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; }

        /// <summary>
        /// 过站记录状态
        /// </summary>
        public WipProductProcessState WipProductProcessState { get; set; }

        /// <summary>
        /// 采集结果 Result (上一次采集的采集结果)
        /// </summary>
        public ResultType? LastResultType { get; set; }
    }
}
