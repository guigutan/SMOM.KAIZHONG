using System;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.ShopFPY
{
    /// <summary>
    /// 直通率数据模型
    /// </summary>
    public class ShopDirectRateInfo
    {
        /// <summary>
        /// 车间名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 是否车间
        /// </summary>
        public bool IsShop { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal DirectRate { get; set; }
    }

    /// <summary>
    ///  折线图信息模型
    /// </summary>
    public class ReportRateInfo
    {
        /// <summary>
        /// 采集日期
        /// </summary>
        public string XDate { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal YData { get; set; }

        /// <summary>
        /// 目标值
        /// </summary>
        public decimal YDesired { get; set; }

        /// <summary>
        /// 预警值
        /// </summary>
        public decimal YAlarm { get; set; }
    }

    public class ShopFpySettingInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 直通率期望值
        /// </summary>
        public decimal Desired { get; set; }

        /// <summary>
        /// 直通率预警值
        /// </summary>
        public decimal Alarm { get; set; }

        /// <summary>
        /// 是否车间
        /// </summary>
        public bool IsShop { get; set; }
    }

    public class BarBasicInfo
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 良品通过率
        /// </summary>
        public decimal PasssRate { get; set; }

    }

    public class ColumnStackedInfo
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 良品值
        /// </summary>
        public decimal PassQty { get; set; }

        /// <summary>
        /// 不良品值
        /// </summary>
        public decimal FailedQty { get; set; }
    }

    /// <summary>
    /// 车间直通率数据通信模型
    /// </summary>
    public class ShopFypInfo
    {
        public List<ShopDirectRateInfo> DirectRateVMList { get; } = new List<ShopDirectRateInfo>();

        public List<ShopFpySettingInfo> ShopFpySettingList { get; } = new List<ShopFpySettingInfo>();
    }

    /// <summary>
    /// 车间直通率下钻图通信数据
    /// </summary>
    public class ShopFypDetailInfo
    {
        public List<BarBasicInfo> BarBasicViewModel { get; } = new List<BarBasicInfo>();

        public List<ColumnStackedInfo> ColumnStackedViewModel { get; } = new List<ColumnStackedInfo>();

        public List<DefectCategoryInfo> DefectCategory { get; } = new List<DefectCategoryInfo>();
    }
}
