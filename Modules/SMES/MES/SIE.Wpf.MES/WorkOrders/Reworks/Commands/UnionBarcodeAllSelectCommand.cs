using DevExpress.Xpf.Core;
using Resources.IconPacks;
using SIE.Domain;
using SIE.MES.WorkOrders.Reworks;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 这个是工单管理，条码关联里面的全选按钮，只全选未关联的条码
    /// </summary>
    [Command(ImageName = "CheckmarkUncrossed", Label = "全选", ToolTip = "全选未关联条码", GroupType = CommandGroupType.Edit)]
    public class UnionBarcodeAllSelectCommand : ListViewCommand
    {
        /// <summary>
        /// 图标控件
        /// </summary>
        PackIcon packIcon;

        /// <summary>
        /// 全选命令执行逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
            if (vm == null) return;
            if (!vm.BarcodeList.Any() || vm.BarcodeList.All(p => p.CodeState == CodeState.Associated) || vm.BarcodeList.All(p => p.CodeState == CodeState.NotAssociated))
                return;
            try
            {
                vm.IsFromCmd = true;
                if (vm.SelectBarcodes)
                    vm.BarcodeList.Where(p => p.CodeState == CodeState.NotAssociated).ForEach(e => view.UnSelectEntities(e));
                else
                {
                    vm.BarcodeList.Where(p => p.CodeState == CodeState.NotAssociated).ForEach(e =>
                    {
                        if (!view.SelectedEntities.OfType<UnionBarcode>().Any(p => p.Id == e.Id))
                            view.SelectEntities(e);
                    });
                }

                SetPackIcon(!vm.SelectBarcodes);
                vm.SelectBarcodes = !vm.SelectBarcodes;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                vm.IsFromCmd = false;
            }
        }

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="isAllSelect">是否全选</param>
        void SetPackIcon(bool isAllSelect)
        {
            if (packIcon == null)
                GetCmdPackIcon();
            if (packIcon == null)
                return;
            packIcon.Kind = isAllSelect ? PackIconKind.Checkmark : PackIconKind.CheckmarkUncrossed;
        }

        /// <summary>
        /// 获取全选命令图标控件
        /// </summary>
        private void GetCmdPackIcon()
        {
            foreach (var item in View.CommandsContainer.Items)
            {
                var cmd = item as SimpleButton;
                if (cmd == null || (cmd.Command as UICommand)?.ClientCommand?.GetType() != typeof(UnionBarcodeAllSelectCommand))
                    continue;
                packIcon = (cmd.Content as Controls.CommandContentControl)?.Icon as PackIcon;
                break;
            }
        }
    }
}