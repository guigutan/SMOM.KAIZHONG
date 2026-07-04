using SIE.Domain;
using SIE.MES.Abnormal;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Common;
using SIE.Wpf.Resources;
using System.Collections.Generic;

namespace SIE.WPF.MES.Abnormal
{
    /// <summary>
    /// 异常停线视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class AbnormalCauseViewConfig : WPFViewConfig<AbnormalCause>
    {
        #region 根据来源类型设置是否只读 IsReadonly
        /// <summary>
        /// 根据来源类型设置是否只读
        /// </summary>
        [Label("是否只读")]
        public static readonly Property<bool> IsReadonlyProperty = P<AbnormalCause>.RegisterExtensionReadOnly("IsReadonly", typeof(AbnormalCauseViewConfig),
            GetIsReadonly, AbnormalCause.SourceTypeProperty);

        /// <summary>
        /// 根据来源类型设置是否只读
        /// </summary>
        /// <param name="me">异常停线实体</param>
        /// <returns>true:只读; false:读写</returns>
        public static bool GetIsReadonly(AbnormalCause me)
        {
            if (me.SourceType == ExceptionStopSourceType.AlertLight)
                return true;
            else
                return false;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Shop).UseResourceWorkShopEditor(p => p.DisplayMember = nameof(Enterprise.Name)).Readonly(IsReadonlyProperty);
            View.Property(p => p.Resource).UseDataSource((e, c, r) =>
            {
                var eq = e as AbnormalCause;
                if (eq == null || eq.Shop == null)
                    return new EntityList<WipResource>();

                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, eq.ShopId, c, r);
            }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; p.DisplayMember = nameof(WipResource.Name); }).Readonly(IsReadonlyProperty);
            View.Property(p => p.WorkOrder).UseDataSource((e, c, r) =>
            {
                var eq = e as AbnormalCause;
                if (eq == null || eq.Resource == null)
                    return new EntityList<WorkOrder>();
                return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(c, r, eq.ResourceId);
            }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).Readonly(IsReadonlyProperty);
            View.Property(p => p.ProductCode).Readonly(IsReadonlyProperty);
            View.Property(p => p.AbnormalType).UseCatalogEditor(e => e.CatalogType = AbnormalCause.AbnormalTypeCatalog).Readonly(IsReadonlyProperty);
            View.Property(p => p.AbnormalReason).Readonly(IsReadonlyProperty);
            View.Property(p => p.BeginDate).UseDateTimeEditor().ShowInList(gridWidth: 160).Readonly(IsReadonlyProperty);
            View.Property(p => p.EndDate).UseDateTimeEditor().ShowInList(gridWidth: 160).Readonly(IsReadonlyProperty);
            View.Property(p => p.SourceType).Readonly(IsReadonlyProperty);
            View.Property(p => p.ExceptionStopType).Readonly(IsReadonlyProperty);
        }
    }
}
