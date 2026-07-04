using SIE.MES.WorkOrders.Reworks;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 这个是工单管理，条码关联里面的列表移除按钮
    /// </summary>
    [Command(ImageName = "DeleteEmpty", Label = "移除条码", GroupType = CommandGroupType.Edit)]
    public class RemoveBarcodeCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count > 0 && view.SelectedEntities.OfType<UnionBarcode>().All(p => p.CodeState == CodeState.NotAssociated);
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
            view.SelectedEntities.OfType<UnionBarcode>().ForEach(e =>
            {
                var barcode = vm.BarcodeList.FirstOrDefault(p => p.Id == e.Id);
                vm.BarcodeList.Remove(barcode);
            });
            vm?.UpdateKeyItemConfigs();
        }
    }
}