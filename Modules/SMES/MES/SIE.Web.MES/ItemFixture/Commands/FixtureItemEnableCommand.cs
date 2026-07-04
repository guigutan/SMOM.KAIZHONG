using SIE.MES.ItemFixture;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemFixture.Commands
{
    /// <summary>
    /// 工装与产品关系保存命令
    /// </summary>
    public class FixtureItemEnableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<FixtureItemController>().EnableFixtureItem(args.ToList());
            return true;
        }
    }
}
