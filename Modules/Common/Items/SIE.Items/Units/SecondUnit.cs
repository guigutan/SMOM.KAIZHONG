using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 辅助单位，多增加一个是为了join查询同时有主单位和辅助单位
    /// </summary>
    [RootEntity, Serializable]    
    [Label("辅助单位")]
    [DisplayMember(nameof(Name))]
    public partial class SecondUnit : Unit
    {                
    }

    /// <summary>
    /// 单位 实体配置
    /// </summary>
    internal class SecondUnitConfig : EntityConfig<SecondUnit>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BD_UNIT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}