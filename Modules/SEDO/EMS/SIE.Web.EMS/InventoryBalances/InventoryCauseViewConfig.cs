using SIE.EMS.InventoryTasks;

namespace SIE.Web.EMS.InventoryBalances
{
    /// <summary>
    /// 原因分析界面
    /// </summary>
    internal class InventoryCauseViewConfig : WebViewConfig<InventoryCause>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommand("SIE.Web.EMS.InventoryBalances.Commands.EditInventoryCauseCommand");
            View.Property(p => p.EquipmentCode).ShowInList(130);
            View.Property(p => p.EquipmentName).ShowInList(150);
            View.Property(p => p.InventoryResult).ShowInList(80);
            View.Property(p => p.Cause).ShowInList(250);
            View.Property(p => p.Improvements).ShowInList(250);
            View.Property(p => p.KeeperId).ShowInList();
            View.Property(p => p.ResponsibeId).ShowInList();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(4);
            View.Property(p => p.EquipmentCode).Readonly();
            View.Property(p => p.EquipmentName).Readonly();
            View.Property(p => p.KeeperId).Readonly();
            View.Property(p => p.InventoryResult).Readonly();
            View.Property(p => p.Cause).UseMemoEditor().ShowInDetail(columnSpan: 4);
            View.Property(p => p.Improvements).UseMemoEditor().ShowInDetail(columnSpan: 4);
            View.Property(p => p.ResponsibeId);
        }
    }
}
