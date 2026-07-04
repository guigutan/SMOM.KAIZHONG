using SIE.EMS.Lubrications.ViewModels;

namespace SIE.Web.EMS.Lubrications.ViewModels
{
    /// <summary>
    /// 选择润滑项目明细视图
    /// </summary>
    internal class SelLubricationDetailViewModelViewConfig : WebViewConfig<SelLubricationDetailViewModel>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();

            View.Property(p => p.ProjectCycle).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.WarningPeriod).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.LubricatingType).Show(ShowInWhere.All).Readonly();

            View.Property(p => p.Part).HasLabel("部位").Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Consumable).HasLabel("项目耗材").Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Standard).Show(ShowInWhere.All).Readonly(); 
            View.Property(p => p.MinValue).HasLabel("最小值").Show(ShowInWhere.All);
            View.Property(p => p.MaxValue).HasLabel("最大值").Show(ShowInWhere.All);
            View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.UseTime).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.CycleType).HasLabel("周期类型").Show(ShowInWhere.All).Readonly();
        }
    }
}
