using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 资源
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery(typeof(CriteriaProvider))]
    [Label("资源")]
    [DisplayMember(nameof(Name))]
    public partial class Resource : Enterprise
    {
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class ResourceConfig : EntityConfig<Resource>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<Enterprise>()
                .Where(p => p.Level.IsResource && p.InvOrgId == RT.InvOrg)
                .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
        }
    }
}