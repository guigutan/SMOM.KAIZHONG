using SIE.MES.PrepareProducts;
using SIE.MetaModel.View;
using SIE.Web.MES.PrepareProducts.Commands;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备项目维护视图配置
    /// </summary>
    public class PrepareProjectViewConfig : WebViewConfig<PrepareProject>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(PrepareProjectAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete,
                typeof(PrepareProjectSaveCommand).FullName, typeof(PrepareProjectImportCommand).FullName,WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProCode).UseListSetting(p => p.HelpInfo= "根据配置项-产前准备编码生成规则带出默认值,可手动编辑。").ShowInList(width: 150);
                View.Property(p => p.ProName).ShowInList(width: 150);
                View.Property(p => p.ProType).ShowInList(width: 150);
                View.Property(p => p.ProDesc).ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProCode).ShowInList(width: 150);
                View.Property(p => p.ProName).ShowInList(width: 150); 
                View.Property(p => p.ProType).ShowInList(width: 150);
                View.Property(p => p.ProDesc).ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProCode).ShowInList(width: 150);
            View.Property(p => p.ProName).ShowInList(width: 150);
            View.Property(p => p.ProType).ShowInList(width: 150);
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.ProCode).BeforeImportRequireFunc("项目编码").ImportIndexer();
            View.Property(p => p.ProName).BeforeImportRequireFunc("项目名称");
            View.Property(p => p.ProType).BeforeImportRequireFunc("项目类型");
            View.Property(p => p.ProDesc);
        }
    }
}
