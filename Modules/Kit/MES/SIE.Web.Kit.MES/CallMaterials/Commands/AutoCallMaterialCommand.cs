using SIE.Domain.Validation;
using SIE.Kit.MES.CallMaterials;
using SIE.Security;
using SIE.Web.Command;
using System;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 自动叫料
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.AutoCallMaterialCommand")]
    [AllowAnonymous]
    public class AutoCallMaterialCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            double flag = -1;
            var autoCallMaterialVM = args.Data.ToJsonObject<AutoCallMaterialVM>();
            flag = autoCallMaterialVM.Flag;
            if (autoCallMaterialVM.ResourceName.IsNullOrEmpty())
            {
                throw new ValidationException("资源不能为空");
            }
            var resourceName = autoCallMaterialVM.ResourceName;
            if (flag == 0)
            {
                RT.Service.Resolve<CallMaterialController>().SetAuto(resourceName, true);
            }
            else
            {
                RT.Service.Resolve<CallMaterialController>().SetAuto(resourceName, false);
            }

            return "{0}".L10nFormat(flag);
        }
    }

    public class AutoCallMaterialVM
    {
        /// <summary>
        /// 标记
        /// </summary>
        public double Flag { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceName { get; set; }
    }
}
