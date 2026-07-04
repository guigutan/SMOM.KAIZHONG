using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 设置缺省命令
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.SetDefaultSolSettingCommand")]
    public class SetDefaultSolSettingCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (!double.TryParse(args.Data, out double solutionId))
                return false;
            RT.Service.Resolve<CallMaterialController>().SetDefaultSortSolution(solutionId);
            return true;
        }
    }
}