using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.CallMatchSaveItemCommand")]
    public class CallMatchSaveItemCommand : SaveCommand
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
            var callMatchWOs = entityList as EntityList<CallMatchWorkOrder>;
            if (callMatchWOs.Count > 0)
            {
                foreach (var callMatchWO in callMatchWOs)
                {
                    callMatchWO.PersistenceStatus = PersistenceStatus.Modified;
                }

                RT.Service.Resolve<CallMaterialController>().SaveUseCallMatchItem(callMatchWOs);
            }

            return "操作成功";
        }
    }
}
