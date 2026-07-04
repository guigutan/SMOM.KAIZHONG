using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 设置紧急
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.SetUrgencyCommand")]
    public class SetUrgencyCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<CallMaterialBill>();
            RT.Service.Resolve<CallMaterialController>().SetUrgency(bill.Id);
            return "操作成功";
        }
    }
}
