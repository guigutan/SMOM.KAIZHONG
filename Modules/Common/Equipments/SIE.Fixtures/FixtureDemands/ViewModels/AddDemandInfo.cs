using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.FixtureDemands.ViewModels
{
    /// <summary>
    /// 工治具需求清单信息
    /// </summary>
    [Serializable]
    public class AddDemandInfo
    {
        /// <summary>
        /// 工治具需求清单
        /// </summary>
        public FixtureDemand Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }

    /// <summary>
    /// 工治具需求清单信息
    /// </summary>
    [Serializable]
    public class FixtureDemandInfo
    {
        /// <summary>
        /// 工治具需求清单
        /// </summary>
        public FixtureDemand FixtureDemand { get; set; }

        /// <summary>
        /// 工治具需求清单需求明细列表
        /// </summary>
        public List<FixtureDemandDetail> FixtureDemandDetailList { get; set; }
    }

    /// <summary>
    /// 工治具上架信息
    /// </summary>
    [Serializable]
    public class UnloadDemandInfo
    {
        /// <summary>
        /// 工治具需求清单ViewModel
        /// </summary>
        public FixtureDemandViewModel fixDemandVM { get; set; } = new FixtureDemandViewModel();

        /// <summary>
        /// 错误信息
        /// </summary>

        public string ErrMsg { get; set; }
    }

    /// <summary>
    /// 工治具编码信息
    /// </summary>
    [Serializable]
    public class DemandEncodeInfo
    {
        /// <summary>
        /// 工治具编码列表
        /// </summary>
        public List<FixtureEncode> FixtureEncodes { get; set; } = new List<FixtureEncode>();

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }

    /// <summary>
    /// 工艺面信息
    /// </summary>
    [Serializable]
    public class DeckInfo
    {
        /// <summary>
        /// 工治具类型
        /// </summary>
        public double? FixtureTypeId { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeCode { get; set; }
        /// <summary>
        /// 工治具型号Id
        /// </summary>
        public double? FixtureModelId { get; set; }

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工艺面
        /// </summary>
        public int? Deck { get; set; }


        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
