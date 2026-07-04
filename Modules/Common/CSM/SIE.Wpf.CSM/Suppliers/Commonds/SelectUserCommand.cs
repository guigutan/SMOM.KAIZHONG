using SIE.CSM.Suppliers;
using SIE.Wpf.Command;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商选择账户按钮
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", GroupType = CommandGroupType.Edit)]

    public class DetaisViewSelectUserCommand : LookupCommand
    {
        /// <summary>
        /// 监听按钮是否可用
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回值表示按钮是否可用</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Parent.Current as Supplier == null)
            {
                return false;
            }

            return base.CanExecute(view);
        }

        /// <summary>
        /// 点击按钮操作
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
        }
    }
}
