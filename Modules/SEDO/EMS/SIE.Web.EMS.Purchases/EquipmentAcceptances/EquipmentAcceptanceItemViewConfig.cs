using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 验收项目视图配置
    /// </summary>
    public class EquipmentAcceptanceItemViewConfig : WebViewConfig<EquipmentAcceptanceItem>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.ItemName).ShowInList(400);
            View.Property(p => p.AcceptanceValue).ShowInList(400);
            View.Property(p => p.AcceptanceResult).ShowInList(80);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(EquipmentAcceptance));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Copy, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemName).ShowInList(400);
                View.Property(p => p.AcceptanceValue).ShowInList(400);
                View.Property(p => p.AcceptanceResult).ShowInList(80);
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}