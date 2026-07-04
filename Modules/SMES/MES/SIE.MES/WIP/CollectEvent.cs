using SIE.MES.WIP.Runtime;
using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 在制品报废
    /// </summary>
    [Serializable]
    public class WipScrapedEvent : WipCollectEvent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="data">采集数据</param>
        public WipScrapedEvent(CollectEventData data) : base(data) { }
    }

    /// <summary>
    /// 在制品在线
    /// </summary>
    [Serializable]
    public class WipOnlineEvent : WipCollectEvent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="data">采集数据</param>
        public WipOnlineEvent(CollectEventData data) : base(data) { }
    }

    /// <summary>
    /// 首件在制品
    /// </summary>
    [Serializable]
    public class WipFirstArticleEvent : WipCollectEvent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="data">采集数据</param>
        public WipFirstArticleEvent(CollectEventData data) : base(data) { }
    }

    /// <summary>
    /// 在制品采集后
    /// </summary>
    [Serializable]
    public class WipCollectedEvent : WipCollectEvent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="data">采集数据</param>
        public WipCollectedEvent(CollectEventData data) : base(data) { }
    }

    /// <summary>
    /// 在制品采集中
    /// </summary>
    [Serializable]
    public class WipCollectingEvent : WipCollectEvent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="data">采集数据</param>
        public WipCollectingEvent(CollectEventData data) : base(data) { }
    }

    /// <summary>
    /// 在制品采集
    /// </summary>
    [Serializable]
    public class WipCollectEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected WipCollectEvent()
        { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="data">采集数据</param>
        public WipCollectEvent(CollectEventData data) { Data = data; }

        /// <summary>
        /// 采集数据
        /// </summary>
        public CollectEventData Data { get; }
    }

    /// <summary>
    /// 采集数据
    /// </summary>
    [Serializable]
    public class CollectEventData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="product">采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息</param>
        /// <param name="barcodes">采集的条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        /// <param name="collectDate">采集时间（数据库时间）</param>
        public CollectEventData(product product, CollectBarcode[] barcodes, CollectData collectData, Workcell workcell, DateTime collectDate)
        {
            Barcodes = barcodes;
            CollectData = collectData;
            Workcell = workcell;
            Product = product;
            CollectDate = collectDate;
        }

        /// <summary>
        /// 条码
        /// </summary>
        public CollectBarcode[] Barcodes { get; }

        /// <summary>
        /// 产品
        /// </summary>
        public product Product { get; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public CollectData CollectData { get; }

        /// <summary>
        /// 采集客户端信息
        /// </summary>
        public Workcell Workcell { get; }

        /// <summary>
        /// 采集时间（数据库时间）
        /// </summary>
        public DateTime CollectDate { get; }

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int CurrentProcessIndex { get; set; }
    }
}
