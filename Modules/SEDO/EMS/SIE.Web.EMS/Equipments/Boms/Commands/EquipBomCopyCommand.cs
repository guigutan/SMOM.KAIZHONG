using SIE.EMS.Equipments.Boms;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Equipments.Boms.Commands
{
    /// <summary>
    /// 设备bom复制新增
    /// </summary>
    public class EquipBomCopyCommand : ViewCommand
    {
        /// <summary>
        /// 实现
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<EquipBomCopyInfo>();
            RT.Service.Resolve<EquipBomController>().CopyDetailCommand(data.NewAddCopyIds, data.CopyDataSourceId);
            return true;
        }
    }

    /// <summary>
    /// 复制命令数据
    /// </summary>
    public class EquipBomCopyInfo
    {
        /// <summary>
        /// 需要复制的数据Ids
        /// </summary>
        public List<double> NewAddCopyIds { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public double CopyDataSourceId { get; set; }
    }
}
