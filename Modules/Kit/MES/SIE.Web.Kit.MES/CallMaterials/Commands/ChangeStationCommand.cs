using SIE.Kit.MES.CallMaterials;
using SIE.Web.Command;
using System;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 转移工位
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.ChangeStationCommand")]
    public class ChangeStationCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = "操作成功".L10N();
            try
            {
                var bill = args.Data.ToJsonObject<CallMaterialBill>();
                errMsg = RT.Service.Resolve<CallMaterialController>().SaveCallMaterialBill(bill);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }
    }
}
