using SIE.Common.Menus;
using SIE.Traces.ForwardTraces;
using SIE.Traces.ReverseTraces;

namespace SIE.Web.Traces.Menus
{
    /// <summary>
    /// PQC菜单初始化
    /// </summary>
    public class TraceMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            const string strTraceInfo = "SNest.追溯管理";
            var res = new List<MenuDto>() {
                new MenuDto()
                {
                    TreeKey = "SNest",
                    IsLeafNode = false,
                    Label = "追溯管理",
                },
                new MenuDto()
                {
                    TreeKey =strTraceInfo,
                    EntityType = typeof(ForwardTraceViewModel),
                    Label = "正向追溯",
                },
                new MenuDto()
                {
                    TreeKey =strTraceInfo,
                    EntityType = typeof(ReverseTraceViewModel),
                    Label = "反向追溯",
                }
            };
            return res;
        }
    }
}
