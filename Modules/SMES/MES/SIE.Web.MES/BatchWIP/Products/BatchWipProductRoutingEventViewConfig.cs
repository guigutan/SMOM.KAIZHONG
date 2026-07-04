using SIE.MES.BatchWIP.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BatchWIP.Products
{
    internal class BatchWipProductRoutingEventViewConfig : WebViewConfig<BatchWipProductRoutingEvent>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductRouting));
            View.Property(p => p.ChangeUserName).HasLabel("修改人");
            View.Property(p => p.ChangeDate).HasLabel("修改时间");
            View.Property(p => p.Remark).Show(ShowInWhere.All);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
