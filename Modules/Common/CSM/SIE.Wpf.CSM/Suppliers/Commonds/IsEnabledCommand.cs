using System;
using System.Linq;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Wpf.Command;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商启用门户
    /// </summary>
    [Command(ImageName = "Play", Label = "启用门户")]
    public class PortalEnabledCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<Supplier>().All(p => !p.IsPortal);
        }

        /// <summary>
        /// 启用门户
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var result = CRT.MessageService.AskQuestion("确认启用选中的资料？".L10N());
            if (result)
            {
                var selected = view.SelectedEntities.OfType<Supplier>().ToList();
                foreach (Supplier supplier in selected)
                {
                    supplier.IsPortal = true;
                    supplier.UpdateBy = RT.IdentityId;
                    supplier.UpdateDate = DateTime.Now;
                    RF.Save(supplier);
                }
            }
        }
    }

    /// <summary>
    /// 供应商禁用门户
    /// </summary>
    [Command(ImageName = "Block", Label = "禁用门户")]
    public class PortalDisEnabledCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<Supplier>().All(p => p.IsPortal);
        }

        /// <summary>
        /// 禁用门户
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var result = CRT.MessageService.AskQuestion("确认禁用选中的资料？".L10N());
            if (result)
            {
                var selected = view.SelectedEntities.OfType<Supplier>().ToList();
                foreach (Supplier supplier in selected)
                {
                    supplier.IsPortal = false;
                    supplier.UpdateBy = RT.IdentityId;
                    supplier.UpdateDate = DateTime.Now;
                    RF.Save(supplier);
                }
            }
        }
    }
}
