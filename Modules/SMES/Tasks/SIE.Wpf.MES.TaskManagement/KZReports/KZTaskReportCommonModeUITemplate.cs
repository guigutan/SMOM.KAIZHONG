using SIE.Wpf.MES.TaskManagement.KZReports.Controls;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// 任务单报工 UI模板
    /// </summary>
    public class KZTaskReportCommonModeUITemplate : KZCollectionUITemplate
    {
        /// <summary>
        /// 报工主界面
        /// </summary>
        private ReportMainControl reportMainCtl;

        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportCommonModeUITemplate() : base(typeof(KZTaskReportCommonModeViewModel))
        {
        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui">UI</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new KZTaskReportCommonModeViewModel();
            ui.MainView.Data = model;
            var layout = ui.Control as DockLayout;

            foreach (var item in layout.Children)
            {
                if (item.GetType() == typeof(DevExpress.Xpf.LayoutControl.LayoutControl))
                {
                    var layoutControl = item as DevExpress.Xpf.LayoutControl.LayoutControl;
                    layoutControl.Height = 0;
                }
            }
            //layout.Children.Add(CreateOperationControl(model));
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        
    }
}
