using SIE.Items;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.Common;
using SIE.Web.MES.ProjectDesigns.ChildCommands.DocumentCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-文件上传视图配置
    /// </summary>
    public class DesignTreeDocumentViewConfig : WebViewConfig<DesignTreeDocument>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectDesign));
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DocumentAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, typeof(DocumentSaveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.DocCode).ShowInList(width: 120);
                View.Property(p => p.DocName).ShowInList(width: 120);
                View.Property(p => p.DocVer).ShowInList(width: 120);
                View.Property(p => p.DocType).UseCatalogEditor(p => { p.CatalogType = DesignTreeDocument.DesignTreeDocumentType; p.CatalogReloadData = true; }).ShowInList(width: 120);
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetProductItems(k, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValuePairs;
                }).HasLabel("产品编码").ShowInList(width: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 120);
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).ShowInList(width: 120);
                View.ChildrenProperty(p => p.AttachmentList);
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.DocCode).ShowInList(width: 120);
                View.Property(p => p.DocName).ShowInList(width: 120);
                View.Property(p => p.DocVer).ShowInList(width: 120);
                View.Property(p => p.DocType).ShowInList(width: 120);
                View.Property(p => p.Product).HasLabel("产品编码").ShowInList(width: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 120);
                View.Property(p => p.Process).ShowInList(width: 120);
                View.ChildrenProperty(p => p.AttachmentList).UseViewGroup(DesignTreeDocumentAttachmentViewConfig.LookUpViewGroup);
            }
        }
    }
}
