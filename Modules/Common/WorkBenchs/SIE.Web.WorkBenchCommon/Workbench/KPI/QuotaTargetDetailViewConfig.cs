using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Web.Common.Commands;
using SIE.MetaModel.View;

namespace SIE.Web.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 目标值设定视图配置
    /// </summary>
    public class QuotaTargetDetailViewConfig : WebViewConfig<QuotaTargetDetail>
    {

        /// <summary>
        /// 周期为周的无命令附加视图名称
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(QuotaTargetDetail.YearProperty);
            View.InlineEdit();
            View.ClearCommands();
            View.DeclareExtendViewGroup(ReadonlyView);

            if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }
        }


        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands("SIE.Web.WorkBenchCommon.Workbench.KPI.Commands.AddDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Year).UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetYearDic(); }).ShowInList();
                View.Property(p => p.Week).UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetWeekDic(); }).ShowInList();
                View.Property(p => p.Month).UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetMonthDic(); }).ShowInList();
                View.Property(p => p.Target).ShowInList();
                View.Property(p => p.DataType).ShowInList().Readonly();
                View.Property(p => p.KpiOperators).ShowInList().HasLabel("条件");
                View.Property(p => p.State).ShowInList().HasLabel("启用状态").Readonly();
                //View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            }
        }


        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Year).ShowInList();
                View.Property(p => p.Week).ShowInList();
                View.Property(p => p.Month).ShowInList();
                View.Property(p => p.Target).ShowInList();
                View.Property(p => p.QuotaTargetSettingValueType).ShowInList();
                View.Property(p => p.KpiOperators).ShowInList().HasLabel("条件");
                View.Property(p => p.State).ShowInList().HasLabel("启用状态").Readonly();
                //View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                //View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            }
        }
    }
}
