using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.APS.FactoryConfirms.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class FactoryConfirmsSaveCommand : ViewCommand
    {
        /// <summary>
        ///  修改工厂
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<List<FactoryConfirmsViewModel>>();
            List<double> splitIdList = args.SelectedIds.ToList();
            RT.Service.Resolve<FactoryConfirmsController>().SaveFactory(data, splitIdList);
            DateTime datetime = DateTime.Now;
            CapacityMapDataQueryer capacityMapDataQueryer = new CapacityMapDataQueryer();
            capacityMapDataQueryer.GetYearQty(datetime.Year.ToString());
            return data;
        }
    }
}
