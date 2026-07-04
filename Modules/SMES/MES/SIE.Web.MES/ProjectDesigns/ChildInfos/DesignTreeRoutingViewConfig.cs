using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Tech.Routings;
using SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线视图配置
    /// </summary>
    public class DesignTreeRoutingViewConfig : WebViewConfig<DesignTreeRouting>
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
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(TreeRoutingInitCommand).FullName, WebCommandNames.Edit, typeof(TreeRoutingSaveCommand).FullName);
            View.UseCommands(typeof(TreeRoutingBomInitCommand).FullName, typeof(TreeRoutingUpDateCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.TreeLevel).Readonly().ShowInList();
                View.Property(p => p.OrderType).ShowInList();
                View.Property(p => p.ProductCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 150);
                View.Property(p => p.Routing).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<RoutingController>().GetRoutings(p, k);
                }).Cascade(p => p.RoutingVersionId, null).ShowInList();
                View.Property(p => p.RoutingVersion).Readonly().ShowInList(width: 150);
                View.Property(p => p.StartDate).UseDateEditor().ShowInList(width: 120);
                View.Property(p => p.EndDate).UseDateEditor().ShowInList(width: 120);
                View.Property(p => p.HasUp).Readonly().ShowInList();
                View.ChildrenProperty(p => p.RoutingDetailList).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.RoutingProBomList).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.RoutingParamList).Show(ChildShowInWhere.All);
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
                View.Property(p => p.TreeLevel).Readonly().ShowInList();
                View.Property(p => p.OrderType).ShowInList();
                View.Property(p => p.ProductCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 150);
                View.Property(p => p.Routing).ShowInList();
                View.Property(p => p.RoutingVersion).Readonly().ShowInList(width: 150);
                View.Property(p => p.StartDate).UseDateEditor().ShowInList(width: 120);
                View.Property(p => p.EndDate).UseDateEditor().ShowInList(width: 120);
                View.Property(p => p.HasUp).Readonly().ShowInList();
                View.ChildrenProperty(p => p.RoutingDetailList).UseViewGroup(DesignTreeRoutingDetailViewConfig.LookUpViewGroup).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.RoutingProBomList).UseViewGroup(DesignTreeRoutingProBomViewConfig.LookUpViewGroup).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.RoutingParamList).UseViewGroup(DesignTreeRoutingParamerViewConfig.LookUpViewGroup).Show(ChildShowInWhere.All);
            }
        }
    }
}
