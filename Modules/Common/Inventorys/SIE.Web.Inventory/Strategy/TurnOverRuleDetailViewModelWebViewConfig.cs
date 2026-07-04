using SIE.Inventory.Strategy;
using SIE.Web.Inventory.Strategy.Commands;

namespace SIE.Web.Inventory.Strategy
{
    internal class TurnOverRuleDetailViewModelWebViewConfig : WebViewConfig<TurnOverRuleDetailSortRuleViewModel>
    {
        /// <summary>
        /// 字段类型不等于数值不可编辑
        /// </summary>
        private const string NUMBERCANEDIT = "字段类型不等于数值不可编辑";

        /// <summary>
        /// 字段类型不等于日期不可编辑
        /// </summary>
        private const string DATECANEDIT = "字段类型不等于日期不可编辑";

        /// <summary>
        /// 字段类型不等于文本不可编辑
        /// </summary>
        private const string TEXTCANEDIT = "字段类型不等于文本不可编辑";

        /// <summary>
        /// 排序字段为批次号和生产批次才可编辑
        /// </summary>
        private const string LOTORPRUCTLOTCANEDIT = "排序字段为批次号和生产批次才可编辑";
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(TurnOverRule));
            View.AssignAuthorize(typeof(TurnOverRuleDetail));
        }
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.WithoutPaging();
            View.UseCommands(typeof(TurnOverRuleDetailSortRuleViewModelEditCommand).FullName);
            View.Property(p => p.SortName).HasLabel("排序名称").Readonly().DisableSort().ShowInList(width: 140);
            View.Property(p => p.SortField).HasLabel("排序字段").DisableSort().ShowInList(width: 140);
            View.Property(p => p.FieldType).Readonly().HasLabel("字段类型").DisableSort().ShowInList(width: 140);
            View.Property(p => p.SortType).HasLabel("排序方式").DisableSort().ShowInList(width: 140);
            View.Property(p => p.EqualValue).Readonly(p => p.FieldType != SIE.Inventory.Commom.DataType.Text).HasLabel("相等值")
                .UseListSetting(e => { e.HelpInfo = TEXTCANEDIT; }).DisableSort().ShowInList(width: 140);
            View.Property(p => p.LowerLimit).Readonly(p => p.FieldType != SIE.Inventory.Commom.DataType.Numerical).HasLabel("下限值")
                .UseListSetting(e => { e.HelpInfo = NUMBERCANEDIT; }).DisableSort().ShowInList(width: 140);
            View.Property(p => p.UpperLimit).Readonly(p => p.FieldType != SIE.Inventory.Commom.DataType.Numerical).HasLabel("上限值")
                .UseListSetting(e => { e.HelpInfo = NUMBERCANEDIT; }).DisableSort().ShowInList(width: 140);
            View.Property(p => p.LowerLimitDay).Readonly(p => p.FieldType != SIE.Inventory.Commom.DataType.Date).HasLabel("下限天数")
                .UseListSetting(e => { e.HelpInfo = DATECANEDIT; }).DisableSort().ShowInList(width: 140);
            View.Property(p => p.UpperLimitDay).Readonly(p => p.FieldType != SIE.Inventory.Commom.DataType.Date).HasLabel("上限天数")
                .UseListSetting(e => { e.HelpInfo = DATECANEDIT; }).DisableSort().ShowInList(width: 140);           
        }
    }
}
