using SIE.Common.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.Configs;
using SIE.ProductIntfc.InspLogs;
using System;

namespace SIE.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首检实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FirstInspCriteria))]
    [EntityWithConfig(typeof(FirstInspNoConfig))]
    [Label("首件报检")]
    public class FirstInsp : InspLog
    {

    }
}
