using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 历史安灯
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonManageHisCriterial))]
    [Label("历史安灯")]

    public class AndonManageHistory : AndonManage
    {
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class AndonManageHistoryConfig : EntityConfig<AndonManageHistory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONMANAGE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
