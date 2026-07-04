using SIE.Common.Configs;
using SIE.Core.Configs;

namespace SIE.Core.Common
{
    /// <summary>
    /// 看板数据处理类
    /// </summary>
    public class DashboardDataUtils
    {
        /// <summary>
        /// 
        /// </summary>
        protected DashboardDataUtils() { }
        /// <summary>
        /// 判断看板是否使用Demo数据源
        /// 项目上可以在这个地方直接返回false，以减少对数据库的读取次数
        /// </summary>
        /// <returns></returns>
        public static bool IsDemoDashboard()
        {
            // 从全局配置项中取数据源配置
            var config = ConfigService.GetConfig(new DashboardDataSourceConfig());
            if (config == null)
                return false;
            return config.DashboardDataSourceType == DashboardDataSourceType.FromDemo;
        }
    }
}
