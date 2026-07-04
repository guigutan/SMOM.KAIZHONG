using SIE.Rbac.Users;

namespace SIE.Web.EMS.DevicePurs
{
    internal class UserGroupViewConfig: WebViewConfig<UserGroup>
    {
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
