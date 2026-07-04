using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;

namespace SIE.Web.Fixtures.FixtureTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class FixtureTypeViewConfig : WebViewConfig<SIE.Fixtures.FixtureTypes.FixtureType>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Delete,typeof(ImmediateDeleteCommand).FullName);
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
        }
        /// <summary>
        /// 配置下拉
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
        }
    }
}
