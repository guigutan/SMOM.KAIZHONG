using SIE.MES.Workbench.EmployeeMarks;

namespace SIE.Wpf.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// 个人评分视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EmployeeMarkViewConfig : WPFViewConfig<EmployeeMark>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("个人作业评分");
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.InlineEdit();
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Shift);
                View.Property(p => p.Resource);
                View.Property(p => p.Employee);
                View.Property(p => p.Mark);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Shift);
            View.Property(p => p.Resource);
            View.Property(p => p.Employee);
        }
    }
}
