using System.Collections.Generic;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 排序优先级设置信息
    /// </summary>
    public class PriorityInfo
    {
        /// <summary>
        /// 排序方案设置Id
        /// </summary>
        public double SolutionSettingId { get; set; }

        /// <summary>
        /// 排序名称
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        public string SortMode { get; set; }

        /// <summary>
        /// 排序优先级
        /// </summary>
        public int Priority { get; set; }
    }

    /// <summary>
    /// 排序优先级设置信息
    /// </summary>
    public class PrioritySetInfo
    {
        /// <summary>
        /// 可选排序优先级列表
        /// </summary>
        public List<PriorityInfo> SelectPriorityInfos { get; } = new List<PriorityInfo>();

        /// <summary>
        /// 已选排序优先级列表
        /// </summary>
        public List<PriorityInfo> SelectedPriortityInfos { get; } = new List<PriorityInfo>();

        /// <summary>
        /// 排序信息列表
        /// </summary>
        public List<SolSettingInfo> SolSettingInfos { get; } = new List<SolSettingInfo>();
    }

    /// <summary>
    /// 排序信息
    /// </summary>
    public class SolSettingInfo
    {
        /// <summary>
        /// 排序方案设置Id
        /// </summary>
        public double SolutionSettingId { get; set; }

        /// <summary>
        /// 已选排序优先级列表
        /// </summary>
        public List<PriorityInfo> SelectedPriortityInfos { get; } = new List<PriorityInfo>();
    }
}
