using SIE.EMS.Common.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检计划保存提交实体数据
    /// </summary>
    [Serializable]
    public class CheckSaveSubmitInfo
    {
        /// <summary>
        /// 点检计划ID
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 点检小结
        /// </summary>
        public string CheckSummary { get; set; }

        /// <summary>
        /// 是否提交(true:提交;false:保存)
        /// </summary>
        public bool IsSubmit { get; set; }

        /// <summary>
        /// 是否异常推送
        /// </summary>
        public bool IsAbnormalInfoPush { get; set; }

        /// <summary>
        /// 项目明细
        /// </summary>
        public List<CheckSaveSubmitProjectInfo> ProjectDetails { get; set; } = new List<CheckSaveSubmitProjectInfo>();

        /// <summary>
        /// 备件更换明细
        /// </summary>
        public List<CheckSaveSubmitSparePartInfo> SparePartDetails { get; set; } = new List<CheckSaveSubmitSparePartInfo>();

        /// <summary>
        /// 备件申请明细
        /// </summary>
        public List<CheckSaveSubmitSparePartAplInfo> SparePartAplDetails { get; set; } = new List<CheckSaveSubmitSparePartAplInfo>();

        /// <summary>
        /// 照片列表
        /// </summary>
        public List<PhotoesInfo> Photoes { get; set; } = new List<PhotoesInfo>();
    }

    /// <summary>
    /// 点检计划保存提交项目数据
    /// </summary>
    [Serializable]
    public class CheckSaveSubmitProjectInfo
    {
        /// <summary>
        /// 检验值
        /// </summary>
        public string ActualValue { get; set; }

        /// <summary>
        /// 检验结果(0:NG,1:OK)
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
    /// 点检计划保存提交备件更换数据
    /// </summary>
    [Serializable]
    public class CheckSaveSubmitSparePartInfo : CheckSparePartInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }
    }

    /// <summary>
    /// 点检计划保存提交备件申请数据
    /// </summary>
    [Serializable]
    public class CheckSaveSubmitSparePartAplInfo : CheckSparePartAplInfo
    {
        /// <summary>
        /// 动作类型(0：新建，1：修改，2：删除)
        /// </summary>
        public int ActionType { get; set; }
    }
}
