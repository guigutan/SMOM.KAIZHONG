using SIE.EMS.Purchases.EquipmentSetups;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试备件申请视图配置
    /// </summary>
    internal class SetupSparePartApplyViewConfig : WebViewConfig<SetupSparePartApply>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.MaterialApplyCommand");
            View.Property(p => p.SparePartCode).Readonly();
            View.Property(p => p.SparePartName).ShowInList(150).Readonly();
            View.Property(p => p.Specification).Readonly();
            View.Property(p => p.PartType).Readonly();
            View.Property(p => p.ControlMethod).ShowInList(80).Readonly();
            View.Property(p => p.ApplyQty).UseSpinEditor(p =>
            {
                p.MinValue = 0.01;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.UnitName).Readonly();
            View.Property(p => p.IssueQty).ShowInList(80).Readonly();
            View.Property(p => p.ConsumeQty).ShowInList(80).Readonly();
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}