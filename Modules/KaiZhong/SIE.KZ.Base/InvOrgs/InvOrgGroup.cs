using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.SmomControl;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.InvOrgs
{
    [Serializable]
    [RootEntity]
    [CriteriaQuery]
    [Label("库存组织")]
    [DisplayMember("Code")]
    public class InvOrgGroup : InvOrg
    {
        #region Web站点 WebSite
        /// <summary>
        /// Web站点
        /// </summary>
        [Label("Web站点")]
        public static readonly Property<string> WebSiteProperty = P<InvOrgGroup>.Register(e => e.WebSite);

        /// <summary>
        /// Web站点
        /// </summary>
        public string WebSite
        {
            get { return this.GetProperty(WebSiteProperty); }
            set { this.SetProperty(WebSiteProperty, value); }
        }
        #endregion

    }


    internal class InvOrgGroupEntityConfig : EntityConfig<InvOrgGroup>
    {
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.Property(InvOrgGroup.WebSiteProperty).DontMapColumn();
        }
    }
}
