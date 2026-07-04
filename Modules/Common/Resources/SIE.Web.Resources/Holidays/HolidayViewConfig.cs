using SIE.MetaModel.View;
using SIE.Resources.Holidays;
using SIE.Web.Resources.Holidays.Commands;

namespace SIE.Web.Resources.Holidays
{
    /// <summary>
    /// 法定假期视图配置
    /// </summary>
    internal class HolidayViewConfig : WebViewConfig<Holiday>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(Holiday.RemarkProperty);
            View.InlineEdit().ClearCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(HolidayAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.BeginDate).HasLabel("开始日期*").UseDateEditor();
            View.Property(p => p.EndDate).HasLabel("结束日期*").UseDateEditor();
            View.Property(p => p.Remark);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Remark).HasLabel("假期名称");
        }
    }
}
