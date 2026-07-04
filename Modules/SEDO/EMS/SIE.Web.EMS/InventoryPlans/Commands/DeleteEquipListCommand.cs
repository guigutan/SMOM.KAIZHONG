using SIE.EMS.InventoryPlans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 删除设备列表数据
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.DeleteEquipListCommand")]
    public class DeleteEquipListCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(double[] args, string scope)
        {
            var selectIds = args.ToList();
            RT.Service.Resolve<InventoryPlanController>().DeleteEquipList(selectIds);
            return true;
        }
    }
}
