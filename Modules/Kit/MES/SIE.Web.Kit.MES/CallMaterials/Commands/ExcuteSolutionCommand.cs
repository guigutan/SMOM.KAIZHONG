using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 执行方案命令
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.ExcuteSolutionCommand")]
    public class ExcuteSolutionCommand : SaveCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<CallMaterialController>().ExcuteSortSolution();
            return "执行成功";
        }
    }
}
