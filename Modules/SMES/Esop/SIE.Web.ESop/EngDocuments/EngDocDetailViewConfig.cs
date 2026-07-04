using SIE.Domain;
using SIE.ESop.EngDocuments;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.Common;
using SIE.Web.ESop.EngDocuments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件子表视图配置
    /// </summary>
    public class EngDocDetailViewConfig : WebViewConfig<EngDocumentDetail>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EngDocument));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.UseCommands("SIE.Web.ESop.EngDocuments.Commands.DownLoadCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.DocCode).ShowInList(width:120);
                View.Property(p => p.DocName).ShowInList(width: 120);
                View.Property(p => p.UseType).UseListSetting(p => p.HelpInfo = "来源于快码DOC_TYPE").UseCatalogEditor(p => { p.CatalogType = EngDocumentDetail.DocTypeCatalogType; p.CatalogReloadData = true; }).ShowInList(width: 120);
                View.Property(p => p.SavePathDisplay).HasLabel("存储路径").UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.ESop.EngDocuments.Scripts.EngDocDetailEditor"; p.Editable = false; }).ShowInList(width: 300);
                View.Property(p => p.Process).UseDataSource((s,p,k) =>
                {
                    var list = RT.Service.Resolve<ProcessController>().GetEmployeeProcessList(p, k);
                    return list ?? new EntityList<Process>();
                }).ShowInList(width: 120);
                View.Property(p => p.SheetPage).ShowInList(width: 120);
                View.Property(p => p.DocumentType).Readonly().ShowInList(width: 120);
            }
        }
    }
}
