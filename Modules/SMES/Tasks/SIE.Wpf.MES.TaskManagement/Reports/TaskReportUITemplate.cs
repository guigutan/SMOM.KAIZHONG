

namespace SIE.Wpf.MES.TaskManagement.Reports
{
    /// <summary>
    /// 任务单报工 UI模板
    /// </summary>
    public class TaskReportUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskReportUITemplate() : base(typeof(TaskReportViewModel))
        {
        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui">UI</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new TaskReportViewModel();
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}
