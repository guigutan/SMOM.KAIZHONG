using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MES.DashBoard.WorkOrderReachs;

namespace SIE.Web.MES.DashBoard
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class DashBoardMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "MES.员工考勤绩效",
                Label = "评分统计表",
                EntityType = typeof(ScoreRecordViewModel)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "生产报表",
                IsLeafNode = false,
            });

            const string mesReport = "MES.生产报表";

            res.Add(new MenuDto()
            {
                TreeKey = mesReport,
                Label = "产线直通率报表",
                EntityType = typeof(LineReportViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesReport,
                Label = "工单准时达成率报表",
                EntityType = typeof(WoReachReportViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesReport,
                Label = "产品直通率报表",
                EntityType = typeof(ProductReportViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesReport,
                Label = "车间直通率报表",
                EntityType = typeof(ShopReportViewModel)
            });

            return res;
        }

    }
}
