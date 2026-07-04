using SIE.Domain;
using SIE.Items;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Web.MES.ProjectDesigns.ChildCommands;
using SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 项目号需求设计-工艺资料视图配置
    /// </summary>
    public class DesignProductTreeViewConfig : WebViewConfig<DesignProductTree>
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
            View.DraggableForTree();
            View.UseCommands(typeof(ProductTreeInitBomCommand).FullName, typeof(ProductTreeAddCommand).FullName, WebCommandNames.Edit, typeof(ProductTreeDeleteCommand).FullName, typeof(ProductTreeSaveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.TreeLevel).Readonly().ShowInList(width: 150);
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
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.DraggableForTree();
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.TreeLevel).Readonly().ShowInList();
                View.Property(p => p.Product).Readonly().HasLabel("产品编码").ShowInList(width: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 120);
            }
        }
    }
}
