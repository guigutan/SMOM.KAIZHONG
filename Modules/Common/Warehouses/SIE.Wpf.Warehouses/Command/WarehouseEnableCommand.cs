using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 仓库启用命令
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用", GroupType = CommandGroupType.Business)]
    public class WarehouseEnableCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Warehouse warehouse = null;
            if (view != null && view.Current != null)
            {
                warehouse = view.Current as Warehouse;
                var warehouselist = view.SelectedEntities.OfType<Warehouse>().ToList();
                foreach (var item in warehouselist)
                {
                    if (item.State != warehouse.State) return false;
                }

                return warehouse != null && warehouse.State != State.Enable && warehouse.PersistenceStatus == PersistenceStatus.Unchanged;
            }
            return false;
        }

        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (CRT.MessageService.AskQuestion("确定启用选中的资料?".L10N()))
            {
                IEnumerable<Warehouse> selected = view.SelectedEntities.OfType<Warehouse>();
                List<double> idlist = selected.Select(p => p.Id).ToList(); //仓库Id列表  

                RT.Service.Resolve<WarehouseController>().EnabelWarehouses(idlist);
            }
        }
    }

    /// <summary>
    /// 仓库禁用命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "停用", ToolTip = "停用", GroupType = CommandGroupType.Business)]
    public class WarehouseDisableCommand : ListViewCommand
    {
        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            Warehouse warehouse = null;
            if (view != null && view.Current != null)
            {
                warehouse = view.Current as Warehouse;
                var warehouselist = view.SelectedEntities.OfType<Warehouse>().ToList();
                foreach (var item in warehouselist)
                {
                    if (item.State != warehouse.State) return false;
                }

                return warehouse != null && warehouse.State != State.Disable && warehouse.PersistenceStatus == PersistenceStatus.Unchanged;
            }
            return false;
        }

        /// <summary>
        /// 命令是否可执行
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (view != null && view.Current != null && CRT.MessageService.AskQuestion("确定禁用选中的资料?".L10N()))
            {
                IEnumerable<Warehouse> selected = view.SelectedEntities.OfType<Warehouse>();
                List<double> idlist = selected.Select(p => p.Id).ToList(); //仓库Id列表  

                RT.Service.Resolve<WarehouseController>().DisableWarehouses(idlist);
            }
        }
    }
}
