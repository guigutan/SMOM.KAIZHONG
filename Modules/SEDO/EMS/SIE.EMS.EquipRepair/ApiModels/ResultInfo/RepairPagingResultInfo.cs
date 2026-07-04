using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修单信息查询结果实体
    /// </summary>
    [Serializable]
    public class RepairPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 维修单信息查询结果明细实体
        /// </summary>
        public List<RepairResultInfo> RepairResultInfos { get; set; } = new List<RepairResultInfo>();
    }

    /// <summary>
    /// 维修单信息查询结果，单据信息实体
    /// </summary>
    [Serializable]
    public class RepairResultInfo
    {
        /// <summary>
        /// 维修单信息ID
        /// </summary>
        public double RepairId { get; set; }

        /// <summary>
        /// 维修单单号
        /// </summary>
        public string RepairNo { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public double? EquipId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 备件ID
        /// </summary>
        public double? SparePartId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipTypeCode { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName { get; set; }

        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double? EquipModelId { get; set; }
        
        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 保修期
        /// </summary>
        public DateTime? WarrantyPeriod { get; set; }

        /// <summary>
        /// 维修类型(0:设备维修，1:备件维修)
        /// </summary>
        public int RepairType { get; set; }

        /// <summary>
        /// 紧急程序(0:紧急，1:高，2:一般)
        /// </summary>
        public int UrgentDegree { get; set; }

        /// <summary>
        /// 生产状态(0:停机，1:生产)
        /// </summary>
        public int ProduceState { get; set; }

        /// <summary>
        /// 故障现象ID
        /// </summary>
        public double? DeviceAbnormalId { get; set; }

        /// <summary>
        /// 故障现象描述
        /// </summary>
        public string DeviceAbnormalDesc { get; set; }

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        public string DeviceAbnormalCode { get; set; }

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime ApplyRepairDate { get; set; }

        /// <summary>
        /// 维修责任人
        /// </summary>
        public string ApplyRepairEmployee { get; set; }

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime? RepairBeginDate { get; set; }

        /// <summary>
        /// 维修状态
        /// 0:(ApplyRepair,/// 报修)
        /// 1:(WaitRepair,/// 待维修)
        /// 2:(Repairing,/// 维修中)
        /// 3:(WaitConfirm,/// 待确认)
        /// 4:(WaitScore,/// 待评分)
        /// 5:(Completed,/// 已完成)
        /// 6:(Suspending,/// 暂停中)
        /// 7:(Cancel,/// 取消)
        /// 8:(Closed,/// 关闭)
        /// </summary>
        public int RepairState { get; set; }

        /// <summary>
        /// 接单派工时间
        /// </summary>
        public DateTime? ReceiveOrderDate { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? EstimateFinishDate { get; set; }

       
        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceTypeStr { get; set; }
    }
}
