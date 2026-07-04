using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Dao;
using SIE.Dock.DockMaintains.Service;
using SIE.Domain;
using SIE.MetaModel;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Dock.DockMaintains.Commands
{
    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteDockMaintainCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 选择仓库命令
    /// </summary>
    public class SelectWarehouseCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var dockMaintainWhList = args.Data.ToJsonObject<List<DockMaintainWh>>();
            Check.NotNullOrEmpty(dockMaintainWhList, nameof(dockMaintainWhList));
            if (null == dockMaintainWhList || dockMaintainWhList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(dockMaintainWhList)));
            }
            foreach (var item in dockMaintainWhList)
            {
                DockMaintainWh dockMaintainWh = new DockMaintainWh();
                dockMaintainWh.DockMaintainId = item.DockMaintainId;
                dockMaintainWh.WarehouseId = item.Id;
                savedData.Add(dockMaintainWh);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 适应仓库删除命令
    /// </summary>
    public class DeleteDockWhCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DockMaintainWhService>().DeleteDockWh(args.ToList());
            return true;
        }
    }
}
