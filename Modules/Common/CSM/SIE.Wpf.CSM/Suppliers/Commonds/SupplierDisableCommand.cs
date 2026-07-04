using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商禁用命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "禁用", ToolTip = "禁用", GroupType = CommandGroupType.Business)]
    public class SupplierDisableCommand : ListViewCommand
    {
        /// <summary>
        /// 是否执行命令
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Supplier supplier = null;
            if (view != null && view.Current != null)
            {
                supplier = view.Current as Supplier;
            }

            return supplier != null && supplier.State != State.Disable;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            Supplier supplier = null;
            if (view != null && view.Current != null)
            {
                supplier = view.Current as Supplier;

                if (CRT.MessageService.AskQuestion("确定禁用选中的资料?".L10N()))
                {
                    supplier.State = State.Disable;
                    RF.Save(supplier);
                }
            }
        }
    }
}
