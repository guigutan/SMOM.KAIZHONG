using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ItemEquipAccount;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount.Commands
{
    /// <summary>
    /// 禁用模具与产品
    /// </summary>
    public class EquipAccountItemDisableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<EquipAccountItemController>().DisableEquipAccountItem(args.ToList());
            return true;
        }
    }
}
