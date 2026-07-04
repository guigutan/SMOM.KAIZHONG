using SIE.Domain;
using SIE.MES.Abnormal;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Common;
using SIE.Wpf.Resources;
using System.Collections.Generic;

namespace SIE.Wpf.MES.Abnormal
{
    /// <summary>
    /// 异常停线查询视图类
    /// </summary>
    internal class AbnormalCauseCriteriaViewConfig : WPFViewConfig<AbnormalCauseCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Shop).UseShopEditor().Show(ShowInWhere.All);
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var eq = e as AbnormalCauseCriteria;
                    if (eq == null || eq.Shop == null)
                        return new EntityList<Resource>();
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(new List<ResourceState>() { ResourceState.Actived }, new List<SyncSourceType>() { SyncSourceType.Enterprise }, null, string.Empty);
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; }).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrder).UseDataSource((e, c, r) =>
                {
                    var eq = e as AbnormalCauseCriteria;
                    if (eq == null || eq.Resource == null)
                        return new EntityList<WorkOrder>();
                    return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(c, r, eq.ResourceId.Value);
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; }).Show(ShowInWhere.All);
                View.Property(p => p.Product).UsePagingLookUpEditor(p => { p.DisplayMember = nameof(Core.Items.Item.Code); }).Show(ShowInWhere.All);
                View.Property(p => p.AbnormalType).UseCatalogEditor(e => e.CatalogType = AbnormalCause.AbnormalTypeCatalog).Show(ShowInWhere.All);
                View.Property(p => p.SourceType).Show(ShowInWhere.All);
                View.Property(p => p.ExceptionStopType).Show(ShowInWhere.All);
                View.Property(p => p.BeginDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p => { p.DateTimePart = ObjectModel.DateTimePart.Date; p.DateRangeType = ObjectModel.DateRangeType.Week; });
            }
        }
    }
}
