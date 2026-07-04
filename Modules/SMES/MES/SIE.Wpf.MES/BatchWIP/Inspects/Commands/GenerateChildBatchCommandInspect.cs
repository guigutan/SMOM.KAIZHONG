using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.Wpf.MES.BatchWIP.Commands;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Inspects.Commands
{
    /// <summary>
    /// 生成子批次命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "生成子批次", ToolTip = "生成子批次", GroupType = CommandGroupType.Edit)]
    public class GenerateChildBatchCommandInspect : GenerateChildBatchCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            ////批次检验--出站列表中正常子批次的数量<1(出站列表排除不良后的数量)
            ////return base.CanExecute(view);
            var vm = view.Relations.Find("mainView")?.Current as BatchDataCollectionViewModel;
            var outputBatchs = view.Data as EntityList<OutputBatch>;
            var normalOutBatchCount = outputBatchs.Count(x => !x.IsNg);
            return vm != null && normalOutBatchCount < 1 && vm.Step.IsGenerateBatch && vm.InputBatchList.Count > 0;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
        }
    }
}
