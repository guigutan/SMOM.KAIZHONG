using SIE.EMS.FixedAssets.Accounts;
using SIE.Web.Command;

namespace SIE.Web.EMS.FixedAssets.Accounts.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [JsCommand("SIE.Web.EMS.FixedAssets.Accounts.Commands.AddFixedAssetAccountCommand")]
    public class AddFixedAssetAccountCommand:ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<FixedAssetsAccount>();
            data.Code = RT.Service.Resolve<FixedAssetsAccountController>().GetCode();
            return data;
        }
    }
}
