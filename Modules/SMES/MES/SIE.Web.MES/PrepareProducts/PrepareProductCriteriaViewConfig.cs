using SIE.Items.ProductFamilys;
using SIE.MES.PrepareProducts;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产品产前准备配置查询视图
    /// </summary>
    public class PrepareProductCriteriaViewConfig : WebViewConfig<PrepareProductCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProductCode).ShowInList(width: 150);
                View.Property(p => p.ProductName).ShowInList(width: 150);
                View.Property(p => p.ProductFamily).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ProductFamilyController>().GetProductFamily(keyword, pagingInfo);
                }).ShowInList(width: 150);
                View.Property(p => p.Process).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(pagingInfo, keyword);
                }).ShowInList(width: 150);
                View.Property(p => p.ProjectCode).ShowInList(width: 150);
                View.Property(p => p.ProjectName).ShowInList(width: 150);
                View.Property(p => p.ProjectType).ShowInList(width: 150);
            }
        }
    }
}
