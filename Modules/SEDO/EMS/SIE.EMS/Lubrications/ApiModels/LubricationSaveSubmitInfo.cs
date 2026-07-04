using SIE.EMS.Common.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Lubrications.ApiModels
{
    /// <summary>
    /// 润滑保存提交信息
    /// </summary>
    [Serializable]
    public class LubricationSaveSubmitInfo
    {
        /// <summary>
        /// 润滑记录id
        /// </summary>
        public double LubricationId { get; set; }

        /// <summary>
        /// 是否提交(true:提交;false:保存)
        /// </summary>
        public bool IsSubmit { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 润滑小结
        /// </summary>
        public string Implementation { get; set; }

        /// <summary>
        /// 项目明细
        /// </summary>
        public List<LubricationSaveSubmitProjectInfo> ProjectDetails { get; set; } = new List<LubricationSaveSubmitProjectInfo>();

        /// <summary>
        /// 备件更换明细
        /// </summary>
        public List<LubricationSaveSparePartInfo> SparePartDetails { get; set; } = new List<LubricationSaveSparePartInfo>();

        /// <summary>
        /// 备件申请明细
        /// </summary>
        public List<LubricationSparePartAplInfo> SparePartAplDetails { get; set; } = new List<LubricationSparePartAplInfo>();

        /// <summary>
        /// 工时登记明细
        /// </summary>
        public List<LubricationWorkHourInfo> WorkHourDetails { get; set; } = new List<LubricationWorkHourInfo>();

        /// <summary>
        /// 照片列表
        /// </summary>
        public List<PhotoesInfo> Photoes { get; set; } = new List<PhotoesInfo>();
    }

    /// <summary>
    /// 润滑保存提交项目信息
    /// </summary>
    [Serializable]
    public class LubricationSaveSubmitProjectInfo
    {
        /// <summary>
        /// 实际加油量
        /// </summary>
        public string ActualValue { get; set; }

        /// <summary>
        /// 延期天数
        /// </summary>
        public string DelayDays { get; set; }

        /// <summary>
        /// 检验项目ID
        /// </summary>
        public double ProjectId { get; set; }
    }

    /// <summary>
    /// 备件更换明细
    /// </summary>
    [Serializable]
    public class LubricationSaveSparePartInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 备件更换项目ID
        /// </summary>
        public double LubricationSparePartId { get; set; }

        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public int UseQty { get; set; }

        /// <summary>
        /// 备件申请单号
        /// </summary>
        public string OutDepotNo { get; set; }

        /// <summary>
        /// 备件出库单状态
        /// </summary>
        public int? OutDepotState { get; set; }

        /// <summary>
        /// 备件出库单状态名称
        /// </summary>
        public string OutDepotStateName { get; set; }

        /// <summary>
        /// 出库仓库ID
        /// </summary>
        public double? OutStockWarehouseId { get; set; }

        /// <summary>
        /// 出库仓库编码
        /// </summary>
        public string OutStockWarehouseCode { get; set; }

        /// <summary>
        /// 出库仓库名称
        /// </summary>
        public string OutStockWarehouseName { get; set; }

        /// <summary>
        /// 备件出库单明细ID
        /// </summary>
        public double? OutDtlId { get; set; }

        /// <summary>
        /// 更换数量
        /// </summary>
        public int ChangeQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备件行状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 备件行状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNo { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 剩余数
        /// </summary>
        public int RemainingQty { get; set; }
    }

    /// <summary>
    /// 备件申请明细
    /// </summary>
    [Serializable]
    public class LubricationSparePartAplInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public int StoreQty { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public int UseQty { get; set; }

        /// <summary>
        /// 备件申请单号
        /// </summary>
        public string SparePartApplyNo { get; set; }

        /// <summary>
        /// 备件申请单状态
        /// </summary>
        public int? SparePartApplyState { get; set; }

        /// <summary>
        /// 备件申请单状态名称
        /// </summary>
        public string ApplyStateName { get; set; }

        /// <summary>
        /// 申请数量
        /// </summary>
        public int ApplyQty { get; set; }

        /// <summary>
        /// 出库仓库ID
        /// </summary>
        public double? OutStockWarehouseId { get; set; }

        /// <summary>
        /// 出库仓库编码
        /// </summary>
        public string OutStockWarehouseCode { get; set; }

        /// <summary>
        /// 出库仓库名称
        /// </summary>
        public string OutStockWarehouseName { get; set; }

        /// <summary>
        /// 备件申请单明细ID
        /// </summary>
        public double? AppDtlId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备件更换项目ID
        /// </summary>
        public double LubricationSparePartId { get; set; }

        /// <summary>
        /// 是否已申请
        /// </summary>
        public bool IsApply { get; set; }
    }

    /// <summary>
    /// 设备润滑工时登记信息实体
    /// </summary>
    [Serializable]
    public class LubricationWorkHourInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 工时登记ID
        /// </summary>
        public double LubricationWorkHourId { get; set; }

        /// <summary>
        /// 执行人ID
        /// </summary>
        public double? EmployeeId { get; set; }

        /// <summary>
        /// 执行人编码
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 执行人名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 润滑开始日期
        /// </summary>
        public string BeginDay { get; set; }

        /// <summary>
        /// 润滑结束日期
        /// </summary>
        public string EndDay { get; set; }

        /// <summary>
        /// 工时(H)
        /// </summary>
        public decimal WorkHours { get; set; }
    }
}
