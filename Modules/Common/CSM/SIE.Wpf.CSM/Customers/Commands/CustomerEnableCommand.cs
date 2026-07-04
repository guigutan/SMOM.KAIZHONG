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
    /// 客户启用命令
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用", GroupType = CommandGroupType.Business)]
    public class CustomerEnableCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Customer customer = null;
            if (view != null && view.Current != null)
            {
                customer = view.Current as Customer;
            }

            return customer != null && customer.State != State.Enable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(ListLogicalView view)
        {
            Customer customer = null;
            if (view != null && view.Current != null)
            {
                customer = view.Current as Customer;

                if (CRT.MessageService.AskQuestion("确定启用选中的资料?".L10N()))
                {
                    customer.State = State.Enable;
                    RF.Save(customer);
                }
            }
        }
    }
}
