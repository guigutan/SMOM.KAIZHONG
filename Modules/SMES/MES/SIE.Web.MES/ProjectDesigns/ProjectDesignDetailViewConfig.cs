using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.Web.MES.ProjectDesigns.ChildInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计详情视图配置
    /// </summary>
    public class ProjectDesignDetailViewConfig : WebViewConfig<ProjectDesignDetail>
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
            View.FormEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.MES.ProjectDesigns.Behaviors.ProjectDesignDetailBehavior");
            View.ClearCommands();
            View.DisableEditing();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectMaintain).ShowInDetail();
                View.Property(p => p.Product).ShowInDetail();
                View.Property(p => p.ProductName).ShowInDetail();
                View.Property(p => p.SaleOrderNo).ShowInDetail();
                View.AttachChildrenProperty(typeof(DesignBasicProperty), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignBasicProperty>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignBasicProperties(design.Id, args.SortInfo, args.PagingInfo);
                }).HasLabel("基本属性").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignProductTree), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignProductTree>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignProductTrees(design.Id, args.PagingInfo);
                }).HasLabel("产品设计").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeBom), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignTreeBom>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeBomDetails(design.Id, args.SortInfo, args.PagingInfo);
                }).HasLabel("产品Bom").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeRouting), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignTreeRouting>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeRoutings(design.Id, args.SortInfo, args.PagingInfo);
                }).HasLabel("产品工艺路线").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeDocument), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignTreeDocument>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeDocuments(design.Id, args.SortInfo, args.PagingInfo);
                }).HasLabel("文件上传").Show(ChildShowInWhere.All);


            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.AddBehavior("SIE.Web.MES.ProjectDesigns.Behaviors.ProjectDesignDetailBehavior");
            View.ClearCommands();
            View.DisableEditing();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectMaintain).ShowInDetail();
                View.Property(p => p.Product).ShowInDetail();
                View.Property(p => p.ProductName).ShowInDetail();
                View.Property(p => p.SaleOrderNo).ShowInDetail();
                View.AttachChildrenProperty(typeof(DesignBasicProperty), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignBasicProperty>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignBasicProperties(design.Id, args.SortInfo, args.PagingInfo);
                }, DesignBasicPropertyViewConfig.LookUpViewGroup).HasLabel("基本属性").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignProductTree), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignProductTree>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignProductTrees(design.Id, args.PagingInfo);
                }, DesignProductTreeViewConfig.LookUpViewGroup).HasLabel("产品设计").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeBom), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignTreeBom>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeBomDetails(design.Id, args.SortInfo, args.PagingInfo);
                }, DesignTreeBomViewConfig.LookUpViewGroup).HasLabel("产品Bom").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeRouting), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignTreeRouting>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeRoutings(design.Id, args.SortInfo, args.PagingInfo);
                }, DesignTreeRoutingViewConfig.LookUpViewGroup).HasLabel("产品工艺路线").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeDocument), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var design = args.Parent.CastTo<ProjectDesignDetail>();
                    if (design == null)
                    {
                        return new EntityList<DesignTreeDocument>();
                    }
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeDocuments(design.Id, args.SortInfo, args.PagingInfo);
                }, DesignTreeDocumentViewConfig.LookUpViewGroup).HasLabel("文件上传").Show(ChildShowInWhere.All);
            }
        }
    }
}
