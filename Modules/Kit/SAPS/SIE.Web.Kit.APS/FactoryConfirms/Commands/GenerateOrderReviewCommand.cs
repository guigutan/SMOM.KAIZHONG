using SIE.Kit.APS.FactoryConfirms;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Kit.APS.FactoryConfirms.Commands
{
    /// <summary>
    /// 生成订单评审命令
    /// </summary>
    public class GenerateOrderReviewCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> ids = args.Data.ToJsonObject<List<double>>();
            RT.Service.Resolve<FactoryConfirmsController>().GenerateFactoryConfirms(ids);
            return "";
        } 
    }
}
