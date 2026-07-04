using SIE.EMS.Purchases.EquipmentSetups;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试设备明细视图配置
    /// </summary>
    public class EquipmentDetailViewConfig : WebViewConfig<EquipmentDetail>
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
            View.Property(p => p.EquipAccountId).HasLabel("设备编码");
            View.Property(p => p.EquipAccountName).ShowInList(150);
            View.Property(p => p.EquipAccountAlias);
            View.Property(p => p.EquipAccountModelCode);
            View.Property(p => p.EquipAccountModelName).ShowInList(150);
            View.Property(p => p.Specifications);
            View.Property(p => p.ManageDepartmentName);
            View.Property(p => p.UseDepartmentName);
            View.Property(p => p.Manufacturer);
            View.Property(p => p.OriginalSerialNumber);
            View.Property(p => p.WarrantyPeriod).ShowInList(150);
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
            View.AssignAuthorize(typeof(EquipmentSetup));
            View.DisableEditing();
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.SelectEquipDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountId).ShowInList().HasLabel("设备编码");
                View.Property(p => p.EquipAccountName).ShowInList(150);
                View.Property(p => p.EquipAccountAlias).ShowInList();
                View.Property(p => p.EquipAccountModelCode).ShowInList();
                View.Property(p => p.EquipAccountModelName).ShowInList(150);
                View.Property(p => p.Specifications).ShowInList();
                View.Property(p => p.ManageDepartmentName).ShowInList();
                View.Property(p => p.UseDepartmentName).ShowInList();
                View.Property(p => p.Manufacturer).ShowInList();
                View.Property(p => p.OriginalSerialNumber).ShowInList();
                View.Property(p => p.WarrantyPeriod).ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}