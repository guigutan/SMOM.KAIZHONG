using System.Linq;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Wpf.Common.Commands;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 启用供应商地址
    /// </summary>
    [Command(ImageName = "Play", Label = "启用")]
    public class AddressEnabledCommand : EnableCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            SupplierAddress supplierAddress = view.Current as SupplierAddress;
            return view.SelectedEntities.Any()  && view.SelectedEntities.OfType<SupplierAddress>().All(p => p.State == State.Disable)  && supplierAddress != null && supplierAddress.PersistenceStatus == Domain.PersistenceStatus.Unchanged;
        }
    }

    /// <summary>
    /// 禁用供应商地址
    /// </summary>
    [Command(ImageName = "Block", Label = "禁用")]
    public class AddressDisEnabledCommand : DisableCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            SupplierAddress supplierAddress = view.Current as SupplierAddress;
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<SupplierAddress>().All(p => p.State == State.Enable) && supplierAddress != null && supplierAddress.PersistenceStatus == Domain.PersistenceStatus.Unchanged;
        }
    }
}
