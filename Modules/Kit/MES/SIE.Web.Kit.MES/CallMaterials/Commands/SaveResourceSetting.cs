using SIE.Kit.MES.CallMaterials;
using SIE.Security;
using SIE.Web.Command;
using System;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 资源保存命令
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.SaveResourceSetting")]
    [AllowAnonymous]
    public class SaveResourceSetting : SaveCommand
    {
        /// <summary>
        /// 成品报检命令执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>报检结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var resourceId = Convert.ToDouble(args.Data);
            RT.Service.Resolve<CallMaterialController>().SaveQuerySetting(resourceId);
            return "保存成功";
        }
    }
}
