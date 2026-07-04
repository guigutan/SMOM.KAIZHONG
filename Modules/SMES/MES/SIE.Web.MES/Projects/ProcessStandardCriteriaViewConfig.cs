using SIE.Items;
using SIE.MES.Projects;
using SIE.Tech.Processs;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects
{
    /// <summary>
    /// 工序标准参数查询实体视图配置
    /// </summary>
    public class ProcessStandardCriteriaViewConfig : WebViewConfig<ProcessStandardCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Type).UseCatalogEditor(p => { p.CatalogType = ProcessStandardParam.ProcessStandardCata; p.CatalogReloadData = true; }).Show();
            View.Property(p => p.Product).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ItemController>().GetProductItems(k, p);
            }).Show();
            View.Property(p => p.Process).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
            }).Show();
            View.Property(p => p.ProcessStStatus).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
        }
    }
}
