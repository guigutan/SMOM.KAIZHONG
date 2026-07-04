using SIE.CSM.ItemInspCharacteristicses;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EventMessages.QMS;
using SIE.Wpf.Command;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商选择物料关系
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", GroupType = CommandGroupType.Edit)]
    internal class SelectItemCommand : LookupCommand
    {
        /// <summary>
        /// 判断按钮是否可用
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回值表示按钮是否可用</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var supplier = view.Parent.Current as Supplier;
            if (supplier == null)
                return false;

            //else if (supplier.State == State.Disable)
            //    return false;
            return base.CanExecute(view);
        }

        /// <summary>
        /// 点击按钮方法
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var supplier = view.Parent?.Current as Supplier;
            if (supplier != null) RF.Save(supplier);
            base.Execute(view);
        }

        protected override void OnAccept()
        {
            base.OnAccept();
            RT.Service.Resolve<ItemInspCharacteristicsController>().SaveItemInspCharacteristics(SelectedView.Data);
        }
    }
}