using SIE.Web.MES.DashBoard.Reports.ShopFPY;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 直通率数据模型
    /// </summary>
    public class ProductDirectRateInfo
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 是否机型
        /// </summary>
        public bool IsProductModel { get; set; }

        /// <summary>
        /// 机型
        /// </summary>
        public string ProductModelName { get; set; }

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
    /// 产品直通率设置视图实体
    /// </summary>
    public class ProductFpySettingInfo : ShopFpySettingInfo
    {
        /// <summary>
        /// 是否机型
        /// </summary>
        public bool IsProductModel { get; set; }
    }

    /// <summary>
    /// 车间直通率数据通信模型
    /// </summary>
    public class ProductFypInfo
    {
        /// <summary>
        /// 直通率数据列表
        /// </summary>
        public List<ProductDirectRateInfo> DirectRateVMList { get; } = new List<ProductDirectRateInfo>();

        /// <summary>
        /// 产品直通率设置列表
        /// </summary>
        public List<ProductFpySettingInfo> ProductFpySettingList { get; } = new List<ProductFpySettingInfo>();
    }

    /// <summary>
    /// 车间直通率下钻图通信数据
    /// </summary>
    public class ProductFpyDetailInfo
    {
        /// <summary>
        /// 柱状图视图实体
        /// </summary>
        public List<BarBasicInfo> BarBasicViewModel { get; } = new List<BarBasicInfo>();

        /// <summary>
        /// 柱状图视图实体
        /// </summary>
        public List<ColumnStackedInfo> ColumnStackedViewModel { get; } = new List<ColumnStackedInfo>();

        /// <summary>
        /// 缺陷代码视图实体
        /// </summary>
        public List<DefectCategoryInfo> DefectCategory { get; } = new List<DefectCategoryInfo>();
    }
}