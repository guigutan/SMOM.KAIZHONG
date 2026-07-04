using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 策略
    /// </summary>
    public enum StrategyType
    {
        /// <summary>
        /// 01.根据来源库位上架到目标库位
        /// </summary>
        [Label("01.根据来源库位上架到目标库位")]
        Strategy01 = 10,

        /// <summary>
        /// 02.根据来源库位寻找目标库区的库位上架
        /// </summary>
        [Label("02.根据来源库位寻找目标库区的库位上架")]
        Strategy02 = 20,

        /// <summary>
        /// 03.直接寻找目标库区的库位上架
        /// </summary>
        [Label("03.直接寻找目标库区的库位上架")]
        Strategy03 = 30,

        /// <summary>
        /// 04.直接上架到目标库位
        /// </summary>
        [Label("04.直接上架到目标库位")]
        Strategy04 = 40,

        /// <summary>
        /// 05.寻找物料档案指定库区的库位上架
        /// </summary>
        [Label("05.寻找物料档案指定库区的库位上架")]
        Strategy05 = 50,

        /// <summary>
        /// 06.直接上架到物料档案指定的库位
        /// </summary>
        [Label("06.直接上架到物料档案指定的库位")]
        Strategy06 = 60,

        /// <summary>
        /// 07.直接寻找目标逻辑分区的库位上架
        /// </summary>
        [Label("07.直接寻找目标逻辑分区的库位上架")]
        Strategy07 = 70,

        /// <summary>
        /// 08.根据来源逻辑分区寻找目标逻辑分区的库位上架
        /// </summary>
        [Label("08.根据来源逻辑分区寻找目标逻辑分区的库位上架")]
        Strategy08 =80,

        /// <summary>
        /// 09.直接上架到目标站台
        /// </summary>
        [Label("09.直接上架到目标站台")]
        Strategy09 =90,

        /// <summary>
        /// 10.直接上架到目标站台组 
        /// </summary>
        [Label("10.直接上架到目标站台组")]
        Strategy10 =100
    }
}