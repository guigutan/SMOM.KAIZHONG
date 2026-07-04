using SIE.Domain;
using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.Resources.UserGroups;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.KzBoard.RegionBoards.Commands
{
    /// <summary>
    /// 选择
    /// </summary>
    public class SelectResourceCommand: ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var items = args.Data.ToJsonObject<List<RegionBoardDetail>>();
            Check.NotNullOrEmpty(items, nameof(items));

            if (null == items || items.Count == 0)
                throw new ArgumentNullException(nameof(items));

            RT.Service.Resolve<RegionBoardsController>().SaveRegionBoardDetail(items);

            return true;
        }
    }
}
