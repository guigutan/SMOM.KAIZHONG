using DevExpress.Xpf.Core;
using Resources.IconPacks;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 这个是工单管理，关键件列表里面的解除按钮
    /// </summary>
    [Command(ImageName = "CheckmarkUncrossed", Label = "全选", ToolTip = "解绑所有关键件", GroupType = CommandGroupType.Edit)]
    public class KeyItemAllSelectCommand : ListViewCommand
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
            if (!vm.KeyItemList.Any())
                return;
            try
            {
                vm.IsFromCmd = true;
                vm.KeyItemList.ForEach(p => p.IsUnbound = !vm.SelectKeyItems);
                SetPackIcon(!vm.SelectKeyItems);
                vm.SelectKeyItems = !vm.SelectKeyItems;
            }
            catch (System.Exception exc)
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
                if (cmd == null || (cmd.Command as UICommand)?.ClientCommand?.GetType() != typeof(KeyItemAllSelectCommand))
                    continue;
                packIcon = (cmd.Content as Controls.CommandContentControl)?.Icon as PackIcon;
                break;
            }
        }
    }
}