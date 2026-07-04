using SIE.Tech.Routings;
using SIE.Web.Data;

namespace SIE.Web.MES.RoutingSettings
{
    /// <summary>
    /// 工艺路线设置数据提供者
    /// </summary>
    public class RoutingSettingDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取工艺路线默认版本布局
        /// SIE.Web.MES.RoutingSettings.RoutingSettingsLayout.js调用
        /// </summary>
        /// <param name="routingId">工艺路线id</param>
        /// <param name="versionId">版本Id</param>
        /// <returns>布局xml</returns>
        public string GetDefaultVersionLayout(double routingId, double versionId)
        {
            return RT.Service.Resolve<RoutingController>().GetRoutingVersion(routingId, versionId)?.Layout?.Layout;
        }
    }
}