using SIE.Fixtures.Repairs;

namespace SIE.Web.Fixtures.Repairs
{
    /// <summary>
    /// 维修记录-视图配置
    /// </summary>
    public class FixtureRepairRecordViewConfig : WebViewConfig<FixtureRepairRecord>
    {
        /// <summary>
        /// 自定义维修记录
        /// </summary>
        public const string FixtureRepairDetail = "FixtureRepairDetail";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(FixtureRepairDetail);
            if (ViewGroup == FixtureRepairDetail)
                FixtureRepairDetailView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.AccountCode);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Qty).UseSpinEditor(p => { p.MinValue = 0; p.Step = 1; });
            View.Property(p => p.Description);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 工治具报修-维修-维修记录视图配置
        /// </summary>
        protected virtual void FixtureRepairDetailView()
        {
            using (View.OrderProperties())
            {
                View.AssignAuthorize(typeof(FixtureRepair));
                View.ClearCommands();
                View.UseCommands("SIE.Web.Fixtures.Repairs.Commands.AddRecordCommand", "SIE.Web.Fixtures.Repairs.Commands.EditRecordCommand", "SIE.Web.Fixtures.Repairs.Commands.DeleteRecordCommand");
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Qty).UseSpinEditor(p => { p.MinValue = 0; p.Step = 1; }).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
