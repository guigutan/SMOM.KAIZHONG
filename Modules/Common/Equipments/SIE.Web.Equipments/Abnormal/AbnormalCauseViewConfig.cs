using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Web.Common;
using SIE.Web.Core.Common;
using SIE.Web.Equipments.Abnormal.Commands;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SIE.Web.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class AbnormalCauseViewConfig : WebViewConfig<AbnormalCause>
    {
        private const string SOURCE_TYPE_HELP_INFO = "异常停线来源等于安灯异常或者预警停线不可编辑";

        #region 根据来源类型设置是否只读 IsReadonly
        /// <summary>
        /// 根据来源类型设置是否只读
        /// </summary>
        private readonly Expression<Func<AbnormalCause, bool>> IsReadonlyExp = p => p.SourceType == ExceptionStopSourceType.AlertLight || p.SourceType == ExceptionStopSourceType.Alerter;
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.Equipments.Abnormal.Commands.AbnormalCauseAddCommand",
                WebCommandNames.Edit,
                WebCommandNames.Save,
                typeof(AbnormalCauseRestoreCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Equipments.Abnormal.Commands.AbnormalCauseAddCommand");
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.SourceType).Readonly()
              .UseListSetting(e => { e.HelpInfo = SOURCE_TYPE_HELP_INFO.L10N(); });
            View.Property(p => p.EquipAccountId).Readonly(p => p.SourceType == ExceptionStopSourceType.AlertLight
                || p.SourceType == ExceptionStopSourceType.Alerter
                || p.ResourceId != null)
                .UseDataSource((e, c, r) =>
                {
                    var eq = e as AbnormalCause;
                    if (eq == null)
                        return new EntityList<EquipAccountSelect>();                    
                    return AppRuntime.Service.Resolve<EquipAccountSelectController>().GetEquipAccounts(r, c);
                })
                .UsePagingLookUpEditor((m, e) =>
              {
                  Dictionary<string, string> dic = new Dictionary<string, string>();
                  dic.Add(nameof(e.EquipName), nameof(EquipAccount.Name));
                  m.DicLinkField = dic;
              });
            View.Property(p => p.EquipName).Readonly();
            View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
            {
                var eq = e as AbnormalCause;
                if (eq == null)
                    return new EntityList<WipResource>();
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                return AppRuntime.Service.Resolve<WipResourceController>().GetWipResources(stateList, null, c, r);
            }).UsePagingLookUpEditor((p, e) =>
            {
                p.DicLinkField = new Dictionary<string, string>()
                {
                    { nameof(e.LineName),nameof(WipResource.Name)},
                    { nameof(e.ShopId),nameof(WipResource.WorkShopId)},
                    { WebCommon.GetDisplayName(nameof(e.ShopId)),nameof(WipResource.WorkShopCode) }
                };
            }).Readonly(p => p.SourceType == ExceptionStopSourceType.AlertLight
                || p.SourceType == ExceptionStopSourceType.Alerter
                || p.EquipAccountId != null)
            .UseListSetting(e => { e.HelpInfo = "显示当前车间下不失效的生产资源,异常停线来源等于安灯异常或者预警停线不可编辑"; });
            View.Property(p => p.LineName).Readonly();
            View.Property(p => p.ExceptionStopType).Readonly(IsReadonlyExp)
               .UseListSetting(e => { e.HelpInfo = SOURCE_TYPE_HELP_INFO.L10N(); });
            View.Property(p => p.StateDescription).Readonly();
            View.Property(p => p.AbnormalType).UseCatalogEditor(e => { e.CatalogType = AbnormalCause.AbnormalTypeCatalog; e.CatalogReloadData = true; }).Readonly(IsReadonlyExp)
                .UseListSetting(e => { e.HelpInfo = "异常类型快码类型(ABNORMAL_TYPE),异常停线来源等于安灯异常或者预警停线不可编辑"; });
            View.Property(p => p.BeginDate).UseDateTimeEditor().ShowInList(160).Readonly(IsReadonlyExp)
                .UseListSetting(e => { e.HelpInfo = SOURCE_TYPE_HELP_INFO.L10N(); });
            View.Property(p => p.EndDate).UseDateTimeEditor().ShowInList(160).Readonly(IsReadonlyExp)
                .UseListSetting(e => { e.HelpInfo = SOURCE_TYPE_HELP_INFO.L10N(); });
            View.Property(p => p.ProcessId).Readonly();
            View.Property(p => p.ProcessSegmentId).Readonly();
            View.Property(p => p.AbnormalReason).Readonly();
            View.Property(p => p.RestoreReason).Readonly();
            View.Property(p => p.RestorerId).Readonly();
            View.Property(p => p.AlerterManageId).Readonly();
            View.Property(p => p.AlerterId).Readonly();
            View.Property(p => p.AlerterName).Readonly();
            View.Property(p => p.AlerterPlugName).Readonly();
            View.Property(p => p.ShopId).Readonly(IsReadonlyExp);
            View.Property(p => p.WorkOrderId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ProductCode), "ProductCode");
                m.DicLinkField = dic;
            }).UseDataSource((e, c, r) =>
            {
                var eq = e as AbnormalCause;
                if (eq == null || eq.Resource == null)
                    return new EntityList<WorkOrder>();
                return AppRuntime.Service.Resolve<AbnormalCauseController>().GetWorkOrders(c, r, eq.ResourceId.Value);
            }).Readonly(IsReadonlyExp).UseListSetting(e => { e.HelpInfo = "显示当前资源下的工单,异常停线来源等于安灯异常或者预警停线不可编辑"; });
            View.Property(p => p.ProductCode).Readonly();
        }

    }
}
