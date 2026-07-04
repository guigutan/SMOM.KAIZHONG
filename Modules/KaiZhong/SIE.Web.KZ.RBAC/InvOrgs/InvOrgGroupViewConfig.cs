using SIE.KZ.Base.InvOrgs;

namespace SIE.Web.KZ.RBAC.InvOrgs
{
    /// <summary>
    /// 
    /// </summary>
    public class InvOrgGroupViewConfig : WebViewConfig<InvOrgGroup>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name).ShowInList(200);
            View.Property(p => p.ExternalId);
            View.Property(p => p.Remark).ShowInList(120);
            View.Property(p => p.WebSite).ShowInList(200);
        }

    }
}
