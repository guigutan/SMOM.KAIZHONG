using SIE.Equipments.FinancialCategorys;
using SIE.MetaModel.View;

namespace SIE.Web.Equipments.FinancialCategorys
{
    /// <summary>
    /// 财务分类页面配置
    /// </summary>
    public class FinancialCategoryViewConfig : WebViewConfig<FinancialCategory>
    {
        /// <summary>
        ///配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.Code).ShowInList(80);
            View.Property(p => p.Name).ShowInList(80);
            View.Property(p => p.Depreciation).UseSpinEditor(m=> { m.MinValue = 1;m.AllowDecimals = false;}).ShowInList(80);
            View.Property(p => p.Desc).ShowInList(400);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(80);
            View.Property(p => p.Name).ShowInList(80);
            View.Property(p => p.Depreciation).ShowInList(80);
            View.Property(p => p.Desc).ShowInList(400);
        }
    }
}
