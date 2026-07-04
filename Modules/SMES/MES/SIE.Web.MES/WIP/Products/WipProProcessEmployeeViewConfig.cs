using SIE.MES.WIP.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 条码工序指派员工信息
    /// </summary>
    public class WipProProcessEmployeeViewConfig : WebViewConfig<WipProProcessEmployee>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeCode).Readonly().ShowInList();
                View.Property(p => p.EmployeeName).Readonly().ShowInList();
                View.Property(p => p.CreateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Readonly().Show(ShowInWhere.Hide);
            }
        }
    }
}
