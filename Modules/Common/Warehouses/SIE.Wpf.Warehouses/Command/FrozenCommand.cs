using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 更改冻结状态
    /// </summary>
    [Command(ImageName = "TableEdit",
            Label = "冻结/释放",
            ToolTip = "冻结/释放",
            GroupType = CommandGroupType.Business)]
    public class FrozenCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>返回true代表可以执行，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var warehouse = view.Current as Warehouse;
            if (view.SelectedEntities.Any())
            {
                var warehouselist = view.SelectedEntities.OfType<Warehouse>().ToList();
                foreach (var item in warehouselist)
                {
                    if (item.IsFrozen != warehouse.IsFrozen || item.PersistenceStatus != PersistenceStatus.Unchanged) return false;
                }

                return true;
            }
            else
            {
                return warehouse != null && warehouse.PersistenceStatus == PersistenceStatus.Unchanged;
            }
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">此命令对应的视图对象。</param>
        public override void Execute(ListLogicalView view)
        {
            try
            {
                var warehouse = view.Current as Warehouse;
                if (!warehouse.IsFrozen)
                {
                    if (!CRT.MessageService.AskQuestion("冻结仓库后，该仓库下的物料将不能进行出入库操作，正在操作的任务在解冻前不能保存，是否继续？".L10N()))
                        return;
                }

                IEnumerable<Warehouse> selected = view.SelectedEntities.OfType<Warehouse>();
                List<double> idlist = selected.Select(p => p.Id).ToList(); //仓库Id列表  
                RT.Service.Resolve<WarehouseController>().FrozenWarehouses(idlist);
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowError(exc.Message);
            }
        }
    }
}
