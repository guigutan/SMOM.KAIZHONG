using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.MetaModel.View;
using SIE.Web.Fixtures.Abnormals.Commands;

namespace SIE.Web.Fixtures.Abnormals
{
    /// <summary>
    /// 工治具异常类型视图配置
    /// </summary>
    internal class FixtureAbnormalViewConfig : WebViewConfig<FixtureAbnormal>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAbnormalCommand).FullName);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New);
            View.Property(p => p.AbnormalType).UseEnumEditor();
            View.Property(p => p.Description);
            View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
            }).HasLabel("工治具类型");
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.AbnormalType).UseEnumEditor();
            View.Property(p => p.Description);
            View.Property(p => p.FixtureType).HasLabel("工治具类型");
        }
    }
}