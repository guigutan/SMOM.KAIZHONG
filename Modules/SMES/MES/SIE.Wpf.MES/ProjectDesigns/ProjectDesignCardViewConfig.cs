using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MES.ProjectDesigns.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ProjectDesigns
{
    /// <summary>
    /// 工艺卡视图属性配置
    /// </summary>
    public class ProjectDesignCardViewConfig : WPFViewConfig<ProjectDesignCard>
    {
        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.WoNo).ShowInDetail();
                View.Property(p => p.ProductCode).ShowInDetail();
                View.Property(p => p.ProductName).ShowInDetail();
                View.Property(p => p.PlanQty).ShowInDetail();
                View.Property(p => p.WipResource).ShowInDetail();
                View.Property(p => p.Process).ShowInDetail();
                View.Property(p => p.ProjectNo).ShowInDetail();
                View.AttachChildrenProperty(typeof(ProjectDesignCardProperty), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var card = args.Parent.CastTo<ProjectDesignCard>();
                    if (card == null)
                    {
                        return new EntityList<ProjectDesignCardProperty>();
                    }
                    return RT.Service.Resolve<ProjectCardController>().GetCardPropertys(card.ProjectId, card.ProductId, args.SortInfo, args.PagingInfo);
                }).HasLabel("基本资料").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(ProjectDesignCardParamter), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var card = args.Parent.CastTo<ProjectDesignCard>();
                    if (card == null)
                    {
                        return new EntityList<ProjectDesignCardParamter>();
                    }
                    return RT.Service.Resolve<ProjectCardController>().GetCardParamters(card.ProjectId, card.ProductId, card.ProcessId, args.SortInfo, args.PagingInfo);
                }).HasLabel("工艺参数").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(DesignTreeDocument), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var card = args.Parent.CastTo<ProjectDesignCard>();
                    if (card == null)
                    {
                        return new EntityList<DesignTreeDocument>();
                    }
                    return RT.Service.Resolve<ProjectCardController>().GetDesignTreeDocuments(card.ProjectId, card.ProductId, args.SortInfo, args.PagingInfo);
                }, DesignTreeDocumentWPFViewConfig.LookUpViewGroup).HasLabel("附件信息").Show(ChildShowInWhere.All);
            }
        }
    }
}
