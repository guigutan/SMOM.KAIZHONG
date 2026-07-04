using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspRecords;
using System;

namespace SIE.ProductIntfc.ProductInsps
{
    /// <summary>
    /// 成品报检
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductInspCriteria))]
    [EntityWithConfig(typeof(NoConfig), "成品报检单单号生成规则", "用于配置成品报检单单号规则")]
    [Label("成品报检")]
    public class ProductInsp : InspRecord
    {
    }

    /// <summary>
    /// 成品报检 实体配置
    /// </summary>
    internal class ProductInspConfig : EntityConfig<ProductInsp>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_INSP_RECORD").MapAllProperties();
        }
    }
}