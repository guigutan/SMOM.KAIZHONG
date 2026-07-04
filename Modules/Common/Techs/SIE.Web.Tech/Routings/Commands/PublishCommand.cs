using SIE.Security;
using SIE.Tech.Routings;
using SIE.Web.Command;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 工艺路线发布命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.PublishCommand")]
    [AllowAnonymous]
    public class PublishCommand : ViewCommand
    {
        /// <summary>
        /// 工艺路线发布命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var obj = args.Data.ToJsonObject<RoutingLayout>();
            var version = RT.Service.Resolve<RoutingController>().ReleaseRoutingVersion(obj.Id, obj.Layout);
            string name = (version.IsDefault == YesNo.Yes ? "*" : string.Empty) + version.Name + "(" + version.ReferenceQty + ")";
            int.TryParse(version.Name.Substring(1), out int number);
            return new
            {
                Id = version.Id,
                text = name,
                leaf = true,
                nodetype = "VersionNode",
                routingId = version.RoutingId,
                layoutId = version.LayoutId,
                state = version.State,
                isDefault = version.IsDefault,
                referenceTime = version.ReferenceQty,
                versionName = version.Name,
                versionNum = number
            };
        }
    }
}