using SIE.Equipments.EquipAccounts;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 设备台账视图配置
    /// </summary>
    internal class EquipAccountViewConfig : WPFViewConfig<EquipAccount>
    {
        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(gridWidth: 100).Readonly();
            View.Property(p => p.Name).ShowInList(gridWidth: 100).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.UseState).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.SupplierId).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.EquipTypeCode).Readonly();
            View.Property(p => p.EquipTypeName).Readonly();
            View.Property(p => p.EquipTypeCategory).Readonly();
            View.Property(p => p.WorkShopId).HasLabel("车间");
            View.Property(p => p.ProcessId).HasLabel("工序");
            View.Property(p => p.InstallationLocation);
        }
    }
}
