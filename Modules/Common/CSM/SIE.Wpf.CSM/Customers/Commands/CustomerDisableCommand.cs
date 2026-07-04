using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.CSM.Customers.Commands
{
    /// <summary>
    /// 客户禁用命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "禁用", ToolTip = "禁用", GroupType = CommandGroupType.Business)]
    public class CustomerDisableCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Customer customer = null;
            if (view != null && view.Current != null)
            {
                customer = view.Current as Customer;
            }

            return customer != null && customer.State != State.Disable;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            Customer customer = null;
            if (view != null && view.Current != null)
            {
                customer = view.Current as Customer;
                if (CRT.MessageService.AskQuestion("确定禁用选中的资料?".L10N()))
                {
                    customer.State = State.Disable;
                    RF.Save(customer);
                }
            }
        }
    }
}
