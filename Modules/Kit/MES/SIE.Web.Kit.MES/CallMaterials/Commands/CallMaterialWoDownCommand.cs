using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 叫料工单下移
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.CallMaterialWoDownCommand")]
    public class CallMaterialWoDownCommand : SaveCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entityList = GetDeserializeData(args, scope);
            var callMaterialWOs = entityList as EntityList<CallMaterialWorkOrder>;
            RT.Service.Resolve<CallMaterialController>().SaveCallMaterialWorkOrderUp(callMaterialWOs);

            return "操作成功";
        }
    }
}
