using SIE.Warehouses;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 库区添加命令
    /// </summary>
    public class StorageAreaAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var warehousearea = args.Data.ToJsonObject<StorageArea>();
            warehousearea.Code = RT.Service.Resolve<WarehouseController>().GetStorageAreaCode();

            return warehousearea;
        }
    }

    /// <summary>
    /// 库区删除命令
    /// </summary>
    public class StorageAreaDeleteCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 库区启用命令
    /// </summary>
    public class StorageAreaEnableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //库区Id列表
            RT.Service.Resolve<WarehouseController>().EnabelStorageAreas(idlist);

            return true;
        }
    }

    /// <summary>
    /// 库区禁用命令
    /// </summary>
    public class StorageAreaDisableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //库区Id列表  

            RT.Service.Resolve<WarehouseController>().DisableStorageAreas(idlist);

            return true;
        }
    }

    /// <summary>
    /// 库区更改冻结状态
    /// </summary>
    public class StorageAreaFrozenCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList();  //库区Id列表
            RT.Service.Resolve<WarehouseController>().FrozenStorageAreas(idlist);

            return true;
        }
    }
}


