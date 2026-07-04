using SIE.Wpf.MES.BatchWIP.Commands;

namespace SIE.Wpf.MES.BatchWIP.Inspects.Commands
{
    /// <summary>
    /// 批次打印设置命令
    /// </summary>
    [Command(ImageName = "ListConfig", Label = "批次条码设置", ToolTip = "批次条码设置", GroupType = CommandGroupType.Edit)]
    public class PrintSettingCommandInspect : PrintSettingCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            ////return base.CanExecute(view); ////批次检验--不考虑"出站是否生成子批次"条件
            var model = view.Current as BatchDataCollectionViewModel;
            return model != null && model.WorkOrder != null; ////&& model.Step.IsGenerateBatch
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            base.Execute(view);
        }
    }
}
