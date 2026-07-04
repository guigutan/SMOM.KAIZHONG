using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Common.Commands;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 目标值设定视图配置
    /// </summary>
    public class QuotaTargetDetailViewConfig : WPFViewConfig<QuotaTargetDetail>
    {
        /// <summary>
        /// 周期为年的附加视图名称
        /// </summary>
        public const string ComYear = "ComYear";

        /// <summary>
        /// 周期为月的附加视图名称
        /// </summary>
        public const string ComMonth = "ComMonth";

        /// <summary>
        /// 周期为周的附加视图名称
        /// </summary>
        public const string ComWeek = "ComWeek";

        /// <summary>
        /// 周期为月的无命令附加视图名称
        /// </summary>
        public const string UnComMonth = "UnComMonth";

        /// <summary>
        /// 周期为周的无命令附加视图名称
        /// </summary>
        public const string UnComWeek = "UnComWeek";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(QuotaTargetDetail.YearProperty);
            View.InlineEdit();
            View.ClearCommands();
            View.DeclareExtendViewGroup(UnComMonth, UnComWeek, ComYear, ComMonth, ComWeek);

            if (ViewGroup == ComYear || ViewGroup == ComMonth || ViewGroup == ComWeek)
            {
                View.UseCommands(typeof(AddQuotaTargetDetailCommand), WPFCommandNames.ListEdit, typeof(DeleteQuotaTargetDetailCommand), typeof(BlukAddQuotaTargetDetailCommand), typeof(EnableQuotaTargetDetailCommand), typeof(DisableCommand));
            }

            if (ViewGroup == UnComMonth || ViewGroup == ComMonth)
            {
                ConfigComMonth();
            }
            else if (ViewGroup == UnComWeek || ViewGroup == ComWeek)
            {
                ConfigComWeek();
            }
            else
            {
                ConfigListView();
            }
        }

        /// <summary>
        /// 周期为月份的配置列表视图
        /// </summary>
        private void ConfigComMonth()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Year).UseDimensionEditorEditor(e => { e.DateType = DateType.YEAR; e.Format = "{0}年"; }).ShowInList();
                View.Property(p => p.Month).UseDimensionEditorEditor(e => { e.DateType = DateType.MONTH; e.Format = "{0}月"; }).ShowInList();
                View.Property(p => p.Target).ShowInList();
                if (ViewGroup == UnComMonth)
                    View.Property(p => p.QuotaTargetSetting.ValueType).UseEnumEditor().HasLabel("目标值格式").ShowInList();
                View.Property(p => p.KpiOperators).ShowInList().HasLabel("条件");
                View.Property(p => p.State).ShowInList().HasLabel("启用状态").Readonly();
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 周期为周的配置列表视图
        /// </summary>
        private void ConfigComWeek()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Year).UseDimensionEditorEditor(e => { e.DateType = DateType.YEAR; e.Format = "{0}年"; }).ShowInList();
                View.Property(p => p.Week).UseDimensionEditorEditor(e => { e.DateType = DateType.WEEK; e.Format = "{0}周"; }).ShowInList();
                View.Property(p => p.Target).ShowInList();
                if (ViewGroup == UnComWeek)
                    View.Property(p => p.QuotaTargetSetting.ValueType).UseEnumEditor().HasLabel("目标值格式").ShowInList();
                View.Property(p => p.KpiOperators).ShowInList().HasLabel("条件");
                View.Property(p => p.State).ShowInList().HasLabel("启用状态").Readonly();
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Year).UseDimensionEditorEditor(e => { e.DateType = DateType.YEAR; e.Format = "{0}年"; }).ShowInList();
                View.Property(p => p.Target).ShowInList();
                View.Property(p => p.QuotaTargetSetting.ValueType).UseEnumEditor().HasLabel("目标值格式");
                View.Property(p => p.KpiOperators).ShowInList().HasLabel("条件");
                View.Property(p => p.State).ShowInList().HasLabel("启用状态").Readonly();
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            }
        }
    }
}
