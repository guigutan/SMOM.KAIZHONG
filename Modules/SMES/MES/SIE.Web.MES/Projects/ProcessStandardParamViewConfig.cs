using SIE.Items;
using SIE.MES.Projects;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.Common;
using SIE.Web.MES.Projects.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects
{
    /// <summary>
    /// 工序标准参数视图配置
    /// </summary>
    public class ProcessStandardParamViewConfig : WebViewConfig<ProcessStandardParam>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(WebCommandNames.Add, "SIE.Web.MES.Projects.Commands.ProcessStandardEditCommand", "SIE.Web.MES.Projects.Commands.ProcessStandardDeleteCommand", typeof(ProcessStandardSaveCommand).FullName, typeof(ProcessStandExportCommand).FullName);
            View.UseCommands(typeof(ProcessStandardExamineCommand).FullName, typeof(ProcessStandardRevokeExamineCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Type).UseCatalogEditor(p => { p.CatalogType = ProcessStandardParam.ProcessStandardCata; p.CatalogReloadData = true; }).Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetProductItems(k, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("产品编码").Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.ProductName).Readonly().ShowInList();
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList();
                View.Property(p => p.ProcessStStatus).Readonly().ShowInList();
                View.Property(p => p.Description).Readonly(p => p.ProcessStStatus == SIE.MES.Projects.Enums.ProcessStStatus.Examined).ShowInList(width: 150);
                View.ChildrenProperty(p => p.ProcessDtlList).HasLabel("工序需求属性组");
            }
        }
    }
}
