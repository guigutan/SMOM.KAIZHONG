using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;
using System;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 叫料命令
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.CallMaterialCommand")]
    public class CallMaterialCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var callMaterialWO = args.Data.ToJsonObject<CallMaterialWorkOrder>();
            if (null == callMaterialWO)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(callMaterialWO)));
            }

            RT.Service.Resolve<CallMaterialController>().AddCallMaterialBill(callMaterialWO.Id);

            return "操作成功";
        }
    }
}
