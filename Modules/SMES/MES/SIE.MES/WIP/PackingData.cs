using Newtonsoft.Json;
using SIE.MES.WIP.Packings.Configs;
using SIE.Packages.Packages;
using SIE.Packages.Packings.Enums;
using SIE.Tech.Stations.Configs;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 包装采集数据
    /// </summary>
    [Serializable]
    public class PackingData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingData()
        {
            ScannedPackageNos = new Queue<string>();
            CurrentPackingUnit = new PackingUnit();
        }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weigh { get; set; }

        /// <summary>
        /// 包装号打印模式
        /// </summary>
        public PrintMode PrintMode { get; set; }

        /// <summary>
        /// 包装号数组
        /// </summary>
        public Queue<string> ScannedPackageNos { get; set; }

        /// <summary>
        /// 当前的包装单位
        /// </summary>        
        public PackingUnit CurrentPackingUnit { get; set; }

        /// <summary>
        /// 指派包装单位
        /// </summary> 
        [JsonIgnore]
        public PackingUnit DesignatedOuterPackingUnit { get; set; }

        /// <summary>
        /// 外包装
        /// </summary>
        public double? OuterPackingRelationId { get; set; }

        /// <summary>
        /// 打包模式
        /// </summary>
        public AutoDoPackingMode PackingMode { get; set; }
    }
}