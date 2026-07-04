using SIE.Inventory.Strategy;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 周转定义视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class TurnOverDefinitionViewConfig : WebViewConfig<TurnOverDefinition>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == TurnOverRuleDetailViewConfig.TurnOverRuleReadOnlyGroup)
            {
                CheckConfigView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.InlineEdit();
            using (View.DeclareBand(string.Empty))
            {
                View.Property(p => p.Sequence).Readonly().Show();
                View.Property(p => p.SortField).Show();
                View.Property(p => p.FieldType).Readonly().Show();
                View.Property(p => p.SortType).Show();
            }
            using (View.DeclareBand("文本型字段"))
            {
                View.Property(p => p.EqualValue).Readonly().Show();
            }
            using (View.DeclareBand("数值型字段"))
            {
                View.Property(p => p.LowerLimit).Readonly().Show();
                View.Property(p => p.UpperLimit).Readonly().Show();
            }
            using (View.DeclareBand("日期型字段和当前日期比较"))
            {
                View.Property(p => p.LowerLimitDay).Readonly().Show();
                View.Property(p => p.UpperLimitDay).Readonly().Show();
            }
        }

        /// <summary>
        /// 扩展查看视图
        /// </summary>
        protected void CheckConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                using (View.DeclareBand(string.Empty))
                {
                    View.Property(p => p.Sequence).Readonly().Show();
                    View.Property(p => p.SortField).Readonly().Show();
                    View.Property(p => p.FieldType).Readonly().Show();
                    View.Property(p => p.SortType).Readonly().Show();
                    View.Property(p => p.LowerLimit).Readonly().Show();
                    View.Property(p => p.UpperLimit).Readonly().Show();
                }
                using (View.DeclareBand("和当前日期比较"))
                {
                    View.Property(p => p.LowerLimitDay).Readonly().Show();
                    View.Property(p => p.UpperLimitDay).Readonly().Show();
                }
            }
        }
    }
}
