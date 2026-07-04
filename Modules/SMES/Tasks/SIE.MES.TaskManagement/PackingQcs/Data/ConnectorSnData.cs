using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs.Data
{
    /// <summary>
    /// 连接器包装采集
    /// </summary>
    public class ConnectorSnData
    {
        /// <summary>
        /// 扫码蓝标还是批次还是刻码||0等于蓝标 1等于批次标签 2等于刻码标签
        /// </summary>
        public int Scan { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 已装箱总数量
        /// </summary>
        public int BlueInt { get; set; }

        /// <summary>
        /// 蓝标装箱总数量
        /// </summary>
        public int BlueZInt { get; set; }

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchLable { get; set; }

        /// <summary>
        /// 批次总数量
        /// </summary>
        public int BatchZInt { get; set; }

        /// <summary>
        /// 批次已装箱数量
        /// </summary>
        public int BatchInt { get; set; }

        

        /// <summary>
        /// 蓝标
        /// </summary>
        public string XtBlue { get; set; }

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string YXtBlue { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 超出字段：0不超出，1超出
        /// </summary>
        public int Exceed { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 正确提示
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 物料成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int SurplusCount { get; set; }

        /// <summary>
        /// 是否可以使用扫码
        /// </summary>
        public bool IsUse { get; set; } = true;

        public List<ConnectorPacking> PackingDetail { get; set; }
    }
}
