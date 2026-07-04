using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.Tasks;

namespace SIE.Wpf.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 任务信息视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class TaskInfoViewConfig : WPFViewConfig<TaskInfo>
    {
        /// <summary>
        /// 设置查看单据视图
        /// </summary>
        public const string ReadonlyViewGroup = "ReadonlyViewGroup";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadonlyViewGroup);
            if (ViewGroup == ReadonlyViewGroup)
            {
                ReadonlyView();
            }
            else
            {
                base.ConfigView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Title);
            View.Property(p => p.Content);
            View.Property(p => p.AssignTo);
            View.Property(p => p.PlanBegin);
            View.Property(p => p.PlanEnd);
            View.Property(p => p.ActualStart);
            View.Property(p => p.ActualEnd);
            View.Property(p => p.Parameter);
            View.Property(p => p.Importance);
            View.Property(p => p.Status);
            View.Property(p => p.Notifications);
            View.Property(p => p.TaskType);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty, 3, false))
                {
                    View.Property(p => p.Title).ShowInDetail(columnSpan: 3);
                    View.Property(p => p.AssignTo);
                    View.Property(p => p.CopyTo);
                    View.Property(p => p.Status).Readonly(DataEntityStatus.IsNewStatusProperty)
                        .UseListSetting(e => { e.HelpInfo = "新增状态不可编辑"; });
                    View.Property(p => p.PlanBegin);
                    View.Property(p => p.PlanEnd);
                    View.Property(p => p.Importance);
                    View.Property(p => p.ActualStart);
                    View.Property(p => p.ActualEnd);
                    
                    View.Property(p => p.Notifications);
                    View.Property(p => p.TaskType);
                    
                }

                using (View.DeclareGroup(" ", 3, false))
                {
                    View.Property(p => p.Content).UseMemoEditor().ShowInDetail(rowSpan: 5, columnSpan: 2);
                    View.Property(p => p.Pic).UseImageEditor().ShowInDetail(rowSpan: 5);
                }
            }
        }

        /// <summary>
        /// 配置只读视图
        /// </summary>
        protected void ReadonlyView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty, 3, false))
                {
                    View.Property(p => p.Title).ShowInDetail(columnSpan: 3).Readonly();
                    View.Property(p => p.AssignTo).Show().Readonly();
                    View.Property(p => p.CopyTo).Show().Readonly();
                    View.Property(p => p.Status).Show().Readonly();
                    View.Property(p => p.PlanBegin).Show().Readonly();
                    View.Property(p => p.PlanEnd).Show().Readonly();
                    View.Property(p => p.Importance).Show().Readonly();
                    View.Property(p => p.ActualStart).Show().Readonly();
                    View.Property(p => p.ActualEnd).Show().Readonly();
                    
                    View.Property(p => p.Notifications).Show().Readonly();
                    View.Property(p => p.TaskType).Show().Readonly();
                }

                using (View.DeclareGroup(" ", 3, false))
                {
                    View.Property(p => p.Content).UseMemoEditor().ShowInDetail(rowSpan: 5, columnSpan: 2).Readonly();
                    View.Property(p => p.Pic).UseImageEditor().ShowInDetail(rowSpan: 5).Readonly();
                }
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Title);
            View.Property(p => p.AssignTo);
            View.Property(p => p.PlanEnd).UseDateRangeEditor();
            View.Property(p => p.Importance).UseEnumEditor(p => p.AllowNullInput = true);
            View.Property(p => p.Status).UseEnumEditor(p => p.AllowNullInput = true);
        }
    }
}
