using SIE.EMS.Common.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养计划保存提交实体数据
    /// </summary>
    [Serializable]
    public class MaintainSaveSubmitInfo
    {
        /// <summary>
        /// 保养计划ID
        /// </summary>
        public double MaintainPlanId { get; set; }

        /// <summary>
        /// 保养小结
        /// </summary>
        public string MaintainSummary { get; set; }

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
        /// 是否异常推送
        /// </summary>
        public bool IsAbnormalInfoPush { get; set; }

        /// <summary>
        /// 项目明细
        /// </summary>
        public List<MaintainSaveSubmitProjectInfo> ProjectDetails { get; set; } = new List<MaintainSaveSubmitProjectInfo>();

        /// <summary>
        /// 备件更换明细
        /// </summary>
        public List<MaintainSaveSubmitSparePartInfo> SparePartDetails { get; set; } = new List<MaintainSaveSubmitSparePartInfo>();

        /// <summary>
        /// 备件申请明细
        /// </summary>
        public List<MaintainSaveSubmitSparePartAplInfo> SparePartAplDetails { get; set; } = new List<MaintainSaveSubmitSparePartAplInfo>();

        /// <summary>
        /// 工时登记明细
        /// </summary>
        public List<MaintainSaveSubmitWorkHourInfo> WorkHourDetails { get; set; } = new List<MaintainSaveSubmitWorkHourInfo>();

        /// <summary>
        /// 照片列表
        /// </summary>
        public List<PhotoesInfo> Photoes { get; set; } = new List<PhotoesInfo>();
    }

    /// <summary>
    /// 保养计划保存提交项目数据
    /// </summary>
    [Serializable]
    public class MaintainSaveSubmitProjectInfo
    {
        /// <summary>
        /// 检验值
        /// </summary>
        public string ActualValue { get; set; }

        /// <summary>
        /// 检验结果(0:NG,1:OK, 2:不适用)
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc { get; set; }

        /// <summary>
        /// 检验项目ID
        /// </summary>
        public double ProjectId { get; set; }
    }

    /// <summary>
    /// 保养计划保存提交备件更换数据
    /// </summary>
    [Serializable]
    public class MaintainSaveSubmitSparePartInfo : MaintainSparePartInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }
    }

    /// <summary>
    /// 保养计划保存提交备件申请数据
    /// </summary>
    [Serializable]
    public class MaintainSaveSubmitSparePartAplInfo : MaintainSparePartAplInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }
    }

    /// <summary>
    /// 保养计划保存提交工时登记数据
    /// </summary>
    [Serializable]
    public class MaintainSaveSubmitWorkHourInfo : MaintainWorkHourInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }
    }
}
