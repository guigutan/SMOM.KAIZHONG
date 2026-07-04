using SIE.EMS.Purchases.EquipmentSetups;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 操作记录视图配置
    /// </summary>
    internal class SetupLogViewConfig : WebViewConfig<SetupLog>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.EmployeeShow).ShowInList(200);
            View.Property(p => p.OperationText).ShowInList(200);
            View.Property(p => p.OperationDateTime).ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}