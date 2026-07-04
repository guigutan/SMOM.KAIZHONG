using SIE.Core.Prints;
using SIE.Domain;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.WIP.Pressure
{
    /// <summary>
    /// 打印设置 命令
    /// </summary>
    [Command(ImageName = "PrinterSettings", Label = "打印设置", ToolTip = "打印设置", GroupType = CommandGroupType.Edit)]
    public class PrintSettingCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return view.Current != null && (view.Current as PressureViewModel)?.WorkOrder != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as PressureViewModel;
            if (vm == null)
                return;
            if (vm?.PrinterSettingTpl == null)
            {
                var tplName = $"{RT.Identity.Name}-{vm.WorkOrder?.ProductCode}";
                vm.PrinterSettingTpl = RT.Service.Resolve<PrinterSettingController>().CreatePrinterSettingTpl(tplName, RT.IdentityId, vm.WorkOrder?.ProductCode);
                RF.Save(vm.PrinterSettingTpl);
            }
            var data = new PrinterSettingTpl();
            data.Clone(vm.PrinterSettingTpl);
            data.PrinterName = vm.Printer;
            var template = new DetailsUITemplate(typeof(PrinterSettingTpl), ViewConfig.DetailsView);
            var detailView = template.CreateUI();

            detailView.MainView.Data = data;
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), detailView.Control, w =>
            {
                w.Title = Meta.Label.L10N();
                w.Height = 300;
                w.Width = 400;
                w.DefaultButton = 1994;  ////避免回车关闭对话框
                //w.Commands.Clear();
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        data.Id = vm.PrinterSettingTpl.Id;
                        data.PersistenceStatus = Domain.PersistenceStatus.Modified;
                        RF.Save(data);
                        vm.PrinterSettingTpl = data;

                    }
                };
            });
        }
    }

}
