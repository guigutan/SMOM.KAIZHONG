
using SIE.MES.DashBoard.DashBoards.WorkShop;
using SIE.MES.ItemEquipAccount;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DashBoard.DashBoards.WorkShop
{
    /// <summary>
    /// 安全生产日期
    /// </summary>
    public class WorkSafetyViewConfig : WebViewConfig<WorkSafety>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.Factory).UseDataSource((o, e, r) =>
                {
                    return RT.Service.Resolve<SIE.MES.DashBoard.DashBoards.ProductionLine.WorkShopController >().GetInvOrgs(e, r);
                });
                View.Property(p => p.SafetyDate);
            }
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Factory);
            View.Property(p => p.SafetyDate);
        }
    }
}
