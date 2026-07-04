using SIE.Fixtures.FixtureDemands;

namespace SIE.Web.Fixtures.Demands
{
    /// <summary>
    /// 出库明细视图配置
    /// </summary>
    internal class FixtureUnloadViewConfig : WebViewConfig<FixtureUnload>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.WarehouseCode).Readonly();
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.LocationCode).Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.UnloadQty).Readonly();
            View.Property(p => p.NgQty).ShowInList(width: 150).Readonly();
            View.Property(p => p.TurnoverToolCode).Readonly();
            View.Property(p => p.UnloadByName).Readonly();
            View.Property(p => p.UnloadDate).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.TaskNo).Readonly();
            View.Property(p => p.ReturnQty).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
