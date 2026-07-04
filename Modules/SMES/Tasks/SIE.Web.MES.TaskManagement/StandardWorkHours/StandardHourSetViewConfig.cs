using SIE.Items;
using SIE.Items.ProductModels;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Web.MES.TaskManagement.StandardWorkHours.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.StandardWorkHours
{
    /// <summary>
    /// 产品标准工时维护视图配置
    /// </summary>
    public class StandardHourSetViewConfig : WebViewConfig<StandardHourSet>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Copy, WebCommandNames.Edit, WebCommandNames.Delete, typeof(StandardHourSetSaveCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(StandardHourSetImportCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourceList(p, k);
                }).ShowInList(width: 150);
                View.Property(p => p.ProductModel).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProductModelController>().GetProductModels(k, p);
                }).ShowInList(width: 150);
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetEnableItemList(k, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    keyValuePairs.Add(nameof(e.SpecificationModel), nameof(e.Product.SpecificationModel));
                    m.DicLinkField = keyValuePairs;
                }).HasLabel("产品编码").ShowInList(width: 150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 150);
                View.Property(p => p.SpecificationModel).Readonly().ShowInList(width: 150);
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).ShowInList(width: 150);
                View.Property(p => p.StandardMin).UseSpinEditor(p => { p.AllowNegative = false; p.DecimalPrecision = 3; }).ShowInList(width: 150);
                View.Property(p => p.AttachMin).UseSpinEditor(p => { p.AllowNegative = false; p.DecimalPrecision = 3; }).ShowInList(width: 150);
                View.Property(p => p.Remark).ShowInList(width: 200);
            }
        }
    }
}
