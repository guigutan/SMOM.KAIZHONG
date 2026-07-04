using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 批次打印设置命令
    /// </summary>
    [Command(ImageName = "ListConfig", Label = "批次条码设置", ToolTip = "批次条码设置", GroupType = CommandGroupType.Edit)]
    public class PrintSettingCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = view.Current as BatchDataCollectionViewModel;
            return model != null && model.WorkOrder != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Current as BatchDataCollectionViewModel;
            if (model == null || !model.WorkOrderId.HasValue)
                throw new ValidationException("产线在生产工单为空".L10N());
            var setting = RT.Service.Resolve<BatchManageController>().GetOrCreateBatchPrintSetting(model.WorkOrderId.Value);
            var viewId = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(BatchPrintSetting), setting);
            var template = new DetailsUITemplate(typeof(BatchPrintSetting));
            var ui = template.CreateUI();
            ui.MainView.Data = setting;
            var result = CRT.Workbench.ShowDialog(viewId, ui.Control, w =>
              {
                  w.Title = "批次条码设置".L10N();
                  w.Height = 400;
                  w.Width = 800;
              });
            if (result == 0)
            {
                RF.Save(setting);
            }
        }
    }
}