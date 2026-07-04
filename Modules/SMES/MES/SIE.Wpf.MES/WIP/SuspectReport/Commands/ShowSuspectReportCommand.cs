using SIE.MetaModel.View;
using SIE.Security;
using SIE.Wpf.Command;
using SIE.Wpf.MES.PackingQC;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.WIP.SuspectReport.Commands
{
    /// <summary>
    /// 可疑品报工
    /// </summary>
    [Command(ImageName = "Question", Label = "可疑品报工", ToolTip = "可疑品报工", GroupType = CommandGroupType.Edit)]
    public class ShowSuspectReportCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return true;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(DetailLogicalView view)
        {

            //采集信息
            var vm = view.Current as KZDataCollectionViewModel;
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(PackingQcViewModel));
            var model = new SuspectReportViewModel(vm.KZWorkstation, "包装");
            var template = new DetailsUITemplate<SuspectReportViewModel>(moduleKey);
            template.ViewGroup = ViewConfig.DetailsView;
            var ui = template.CreateUI();

            ui.MainView.Data = model;
            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "可疑品报工".L10N();
                w.Height = 600;
                w.Width = 800;
                w.Commands.Clear();
                w.Closed += (s, e) =>
                {
                    vm.FocuseBarcode();
                };

                CRT.MainThread.InvokeIfRequired(() =>
                {
                    model.FocuseBarcode();
                });
            });
        }
    }
}
