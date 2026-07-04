using System;
using System.Collections.Generic;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检相关图片 
    /// </summary>
    [Serializable]
    public class CheckPictureInfos
    {
        /// <summary>
        /// 点检计划id
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 类型 0-点检图片 1-维修图片
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 图片文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件base64
        /// </summary>
        public string Src { get; set; }
    }

    /// <summary>
    /// 点检维修信息
    /// </summary>
    [Serializable]
    public class CheckPlanRepairInfo
    {
        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 生产状态 0-停机 1-生产
        /// </summary>
        public int ProduceState { get; set; }

        /// <summary>
        /// 紧急程度 0-紧急 1-高 2-一般
        /// </summary>
        public int UrgentDegree { get; set; }

        /// <summary>
        /// 异常现象Id
        /// </summary>
        public double? AbnormalId { get; set; }

        /// <summary>
        /// 异常编码
        /// </summary>
        public string AbnormalCode { get; set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string AbnormalDesc { get; set; }
    }

    /// <summary>
    /// 点检更换信息
    /// </summary>
    [Serializable]
    public class CheckPlanSpareChgInfos
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 备件出库单明细
        /// </summary>
        public double OutDtlId { get; set; }

        /// <summary>
        /// 备件出库单号
        /// </summary>
        public string OutDepotNo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainingQty { get; set; }

        /// <summary>
        /// 更换数量
        /// </summary>
        public int ChangeQty { get; set; }

        /// <summary>
        /// 备件行状态 0-新建 1-完成
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
        /// 序列号Id
        /// </summary>
        public double? SeriaNoId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 批次号Id
        /// </summary>
        public double? BatchNoId { get; set; }
    }

    /// <summary>
    /// 离线盘点数据
    /// </summary>
    [Serializable]
    public class CheckPlanOfflineSubmit
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public CheckPlanOfflineSubmit()
        {
            CheckPlanInfo = new CheckPlanInfos();
            CheckPlanProList = new List<CheckPlanProjectInfo>();
            CheckPlanPictureList = new List<CheckPictureInfos>();
            RepairInfo = new CheckPlanRepairInfo();
            RepairPictureList = new List<CheckPictureInfos>();
            CheckSpareChgList = new List<CheckPlanSpareChgInfos>();
        }

        /// <summary>
        /// 点检计划主信息
        /// </summary>
        public CheckPlanInfos CheckPlanInfo { get; set; }

        /// <summary>
        /// 点检项目
        /// </summary>
        public List<CheckPlanProjectInfo> CheckPlanProList { get; set; }

        /// <summary>
        /// 点检图片
        /// </summary>
        public List<CheckPictureInfos> CheckPlanPictureList { get; set; }

        /// <summary>
        /// 维修信息
        /// </summary>
        public CheckPlanRepairInfo RepairInfo { get; set; }

        /// <summary>
        /// 维修图片
        /// </summary>
        public List<CheckPictureInfos> RepairPictureList { get; set; }

        /// <summary>
        /// 备件更换信息
        /// </summary>
        public List<CheckPlanSpareChgInfos> CheckSpareChgList { get; set; }

    }
}
