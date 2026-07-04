using SIE.Fixtures.Warns;

namespace SIE.Web.Fixtures.Warns
{
    /// <summary>
    /// 工治具保养预警-界面
    /// </summary>
    internal class FixtureWarnViewConfig : WebViewConfig<FixtureWarn>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.FixtureType).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.AccountState).Readonly();
            View.Property(p => p.WarnNum).Readonly();
            View.Property(p => p.WarnHour).Readonly();
            View.Property(p => p.MaintainedNum).Readonly();
            View.Property(p => p.MaintainedHour).Readonly();
            View.Property(p => p.RestNum).Readonly();
            View.Property(p => p.RestHour).Readonly();
            View.Property(p => p.TotalUseNum).Readonly();
            View.Property(p => p.TotalThrowQty).Readonly();
            View.Property(p => p.WipResource).Readonly();
            View.Property(p => p.WorkOrder).Readonly();
            View.Property(p => p.EquipAccount).Readonly();
            View.Property(p => p.Subarea).Readonly();
            View.Property(p => p.Stance).Readonly();
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }
    }
}
