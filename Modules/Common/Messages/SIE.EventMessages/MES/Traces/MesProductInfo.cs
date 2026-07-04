using SIE.Domain;
using SIE.EventMessages.WMS.Traces;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 反向追溯-Mes产品列表
    /// </summary>
    [Serializable]
    public class MesProductInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<MesProductItemInfo> Data { get; set; } = new List<MesProductItemInfo>();
    }

    /// <summary>
    /// 反向追溯-Mes产品详细信息
    /// </summary>
    [Serializable]
    public class MesProductItemInfo
    {
        /// <summary>
        /// 产品版本Id（主键）
        /// </summary>
        public double VersionId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductName { get; set; }

        
        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductionLot { get; set; }

       
        /// <summary>
        /// 产品扩展属性
        /// </summary>
        public string ProductExtPropName { get; set; }

       
        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName { get; set; }
       

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string VersionName { get; set; }

        
        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }


        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate { get; set; }

    }
}
