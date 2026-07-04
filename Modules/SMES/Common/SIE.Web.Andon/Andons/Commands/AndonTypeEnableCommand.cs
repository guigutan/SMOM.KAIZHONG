using SIE.Andon.Andons;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 启用安灯类型维护
    /// </summary>
    public class AndonTypeEnableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<AndonTypeController>().EnableAndonType(args.ToList());
            return true;
        }
    }
}
