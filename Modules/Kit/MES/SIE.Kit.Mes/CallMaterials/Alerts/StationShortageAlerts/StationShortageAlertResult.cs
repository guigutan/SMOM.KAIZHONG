using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工位缺料预警参数类
    /// </summary>
    [Serializable]
    public class StationShortageAlertResult : CallMaterialAlertResult
    {

        /// <summary>
        /// 工位编号
        /// </summary>
        [Label("工位编码")]
        public string StationCode { get; set; }

        /// <summary>
        /// 工位预警物料编码
        /// </summary>
        [Label("物料编码")]
        public string AlertMaterialCode { get; set; }

        /// <summary>
        /// 工位预警物料编码
        /// </summary>
        [Label("物料名称")]
        public string AlertMaterialName { get; set; }
    }

    /// <summary>
    /// 工位缺料预警参数集合类
    /// </summary>
    [Serializable]
    public class StationShortageAlertResultList : CallMaterialAlertResultList
    {
    }
}