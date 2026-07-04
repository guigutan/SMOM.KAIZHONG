using SIE.Domain;
using SIE.Security;
using SIE.Tech.Routings;
using SIE.Tech.Routings.ViewModels;
using SIE.Web.Command;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 保存工艺路线
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.SaveRoutingCommand")]
    [AllowAnonymous]
    public class SaveRoutingCommand : ViewCommand
    {
        /// <summary>
        /// 保存工艺路线执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var obj = args.Data.ToJsonObject<RoutingLayoutMsg>();
            if (obj.RoutingId > 0)
            {
                if (obj.RoutingVersionId > 0)
                {
                    var version = RF.Find<RoutingVersion>().GetById(obj.RoutingVersionId) as RoutingVersion;
                    if (version != null)
                    {
                        version.Layout.Layout = obj.Layout;
                        RF.Save(version.Layout);
                    }
                }
                else
                {
                    var version = RT.Service.Resolve<RoutingController>().CreateRoutingVersion(obj);
                    string name = (version.IsDefault == YesNo.Yes ? "*" : string.Empty) + version.Name + "(" + version.ReferenceQty + ")";
                    int.TryParse(version.Name.Substring(1), out int number);
                    //新增工艺路线版本返回新增的节点，让前端展开显示（前端判断非true则增加节点并展开显示）
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
            //其他return true
            return true;
        }
    }
}