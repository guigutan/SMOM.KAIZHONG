using SIE.Items;
using SIE.Items.ProductModels;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.StandardWorkHours
{
    /// <summary>
    /// 产品标准工时维护查询实体视图配置
    /// </summary>
    public class StandardHourSetCriteriaViewConfig : WebViewConfig<StandardHourSetCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourceList(p, k);
                }).Show();
                View.Property(p => p.ProductModel).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProductModelController>().GetProductModels(k, p);
                }).Show();
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetEnableItemList(k, p);
                }).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}
