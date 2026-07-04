using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 资源
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Name))]
    [Label("车间")]
    public partial class WorkShop : Enterprise
    {
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class WorkShopConfig : EntityConfig<WorkShop>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.IsTreeEntity = false;
        }
    }
}
