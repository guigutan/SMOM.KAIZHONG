using SIE.CSM.Customers;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.CSM.Customers.Commands
{
    /// <summary>
    /// 客户地址默认按钮
    /// </summary>
    [Command(ImageName = "FlagTriangle", Label = "设为缺省", ToolTip = "设为缺省", GroupType = CommandGroupType.Business)]
    public class CustomerAddressDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            CustomerAddress customerAddress = view.Current as CustomerAddress;
            return customerAddress != null && !customerAddress.IsDefault && customerAddress.PersistenceStatus == Domain.PersistenceStatus.Unchanged;
        }

        /// <summary>
        /// 设置默认逻辑
        /// </summary>
        /// <param name="view">逻辑视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            if (!CRT.MessageService.AskQuestion("是否设置当前选择地址为默认地址？".L10N()))
            {
                return;
            }

            CustomerAddress customerAddress = view.Current as CustomerAddress;
            RT.Service.Resolve<CustomerController>().SetDefaultAddress(customerAddress.CustomerId.Value, customerAddress.Id);
            CRT.MessageService.ShowInstantMessage("设置默认地址成功！".L10N(), "提示".L10N(), 3);
            view.Parent.QueryView.TryExecuteQuery();
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    public class CustomerAddressDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var address = view.Current as CustomerAddress;
            if (address == null || view.SelectedEntities == null || address.State != Domain.State.Disable)
                return false;
            if (view.SelectedEntities.OfType<CustomerAddress>().Any(p => p.State != Domain.State.Disable))
                return false;

            return true;
        }
    }
}
