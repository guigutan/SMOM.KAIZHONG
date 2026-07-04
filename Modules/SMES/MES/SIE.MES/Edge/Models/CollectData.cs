using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 采集数据实体
    /// </summary>
    [Serializable]
    public class EdgeCollectData
    {
        /// <summary>
        /// 唯一码
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public string WorkOrderId { get; set; }

        /// <summary>
        /// 生产条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 组件条码
        /// </summary>
        public string ComBarcode { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public Core.Barcodes.BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// Ng数量
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// Puid
        /// </summary>
        public string Puid { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 生产产线
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 过站工序
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 下一工序
        /// </summary>
        public string NextProcessId { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public SIE.Common.ResultType ResultType { get; set; }


        /// <summary>
        /// 过站状态,0.完成(出站)，1,开始（进站）
        /// </summary>
        public MES.WIP.Products.WipProductProcessState State { get; set; }

        /// <summary>
        /// 是否完工
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 创建SKU
        /// </summary>
        public bool CreateSku { get; set; }

        /// <summary>
        /// 工序标记
        /// </summary>
        public RoutingProcessSign Sign { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 是否移除（包装采集使用）
        /// </summary>
        public bool IsRemove { get; set; }

        /// <summary>
        /// 采集bom
        /// </summary>
        public List<EdgeMaterial> MaterialLabels { get; set; }

        /// <summary>
        /// 缺陷信息
        /// </summary>
        public List<CollectDefect> Defects { get; set; }

        /// <summary>
        /// 检验项目
        /// </summary>
        public List<CollectInspectionItem> InspectionItems { get; set; }

        /// <summary>
        /// 更新的关键件
        /// </summary>
        public List<MaterialCollectData> UpdatedKeyMaterials { get; set; }

        /// <summary>
        /// 包装记录
        /// </summary>
        public List<EdgePackRecord> PackageList { get; set; }
    }

}
