using SIE.Tech.Stations;
using SIE.Web.Command;
using System;

namespace SIE.Web.Tech.Stations.Commands
{
    /// <summary>
    /// 设置主设备
    /// </summary>
    public class StationEquipmentSetMasterCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<StationEquipment>();
            if (null == entity)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(entity)));
            }
            RT.Service.Resolve<StationController>().SetEquipmentMaster(entity);
            return true;
        }
    }
}
