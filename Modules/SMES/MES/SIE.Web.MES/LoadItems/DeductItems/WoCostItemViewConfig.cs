using SIE.Domain;
using SIE.Domain.Caching;
using SIE.Items;
using SIE.LES.Commons;
using SIE.MES.LoadItems;
using SIE.MES.LoadItems.Enum;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Web.Items._Extentions_;
using SIE.Web.MES.LoadItems.DeductItems.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.LoadItems.DeductItems
{
    /// <summary>
    /// 扣料记录视图配置
    /// </summary>
    public class WoCostItemViewConfig : WebViewConfig<WoCostItem>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(WoCostItemAddCommand).FullName,
                "SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemEditCommand",
                "SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemDeleteCommand",
                typeof(WoCostItemSaveCommand).FullName,
                typeof(WoCostItemSubmitCommand).FullName,
                typeof(WoCostItemSupplementCommand).FullName,
                typeof(WoCostItemCloseCommand).FullName,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            using (View.OrderProperties())
            {
                View.Property(p => p.CostNo).Readonly().ShowInList(width: 150);
                View.Property(p => p.RecordType).UseEnumEditor(p => p.XType = "woCostItemEnumEditor").Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).UseListSetting(p => p.HelpInfo = "手动创建耗用单的单据类型不能为【物料倒扣】").ShowInList(width: 150);
                View.Property(p => p.State).Readonly().ShowInList(width: 150);
                View.Property(p => p.WorkOrder).UseDataSource((s, p, k) =>
                {
                    var source = s as WoCostItem;
                    if (source == null)
                    {
                        return new EntityList<WorkOrder>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WoCostItemController>().GetWorkOrders(source, p, k);
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.WorkOrder.ProductName));
                    keyValues.Add(nameof(e.ProjectNo), nameof(e.WorkOrder.ProjectMaintainCode));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem || p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit).Cascade(p => p.ItemId, null).ShowInList(width: 150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 150);
                View.Property(p => p.Item).UseDataSource((s, p, k) =>
                {
                    var source = s as WoCostItem;
                    if (source == null)
                    {
                        return new EntityList<Item>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WoCostItemController>().GetItemsByType(source, p, k);
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.CostItemName), nameof(e.Item.Name));
                    keyValues.Add(nameof(e.ConsumeType), nameof(e.Item.ConsumeMode));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem || p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit).HasLabel("耗用物料编码").Cascade(p => p.CostItemLabelId, null).Cascade(p => p.ItemExtProp, null).Cascade(p => p.ItemExtPropName, null).ShowInList(width: 150);
                View.Property(p => p.CostItemName).Readonly().ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.DbField = "ItemExtProp";
                }).Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).HasLabel("物料拓展属性").ShowInList(width: 150);
                View.Property(p => p.ProjectNo).Readonly().ShowInList(width: 150);
                View.Property(p => p.ConsumeType).Readonly().ShowInList(width: 150);
                View.Property(p => p.CostItemLabel).UseDataSource((s, p, k) =>
                {
                    var source = s as WoCostItem;
                    if (source == null)
                    {
                        return new EntityList<ItemLabel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WoCostItemController>().GetItemLabels(source, p, k);
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Lot), nameof(e.CostItemLabel.Lot));
                    keyValues.Add(nameof(e.Warehouse), nameof(e.CostItemLabel.WarehouseCode));
                    keyValues.Add(nameof(e.Storage), nameof(e.CostItemLabel.StorageLocationCode));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).ShowInList(width: 150).HasLabel("物料标签".L10N() + "*");
                GetPartView();
            }
        }

        /// <summary>
        /// 获取部分视图
        /// </summary>
        private void GetPartView()
        {
            View.Property(p => p.Lot).Readonly().ShowInList(width: 150);
            View.Property(p => p.Qty).UseItemUnitEditor().Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).ShowInList(width: 150);
            View.Property(p => p.Warehouse).Readonly().ShowInList(width: 150);
            View.Property(p => p.Storage).Readonly().ShowInList(width: 150);
            View.Property(p => p.Process).UseDataSource((s, p, k) =>
            {
                var source = s as WoCostItem;
                if (source == null)
                {
                    return new EntityList<Process>();
                }
                else
                {
                    return RT.Service.Resolve<WoCostItemController>().GetProcesses(source, p, k);
                }
            }).Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).ShowInList(width: 150);
            View.Property(p => p.Station).Readonly().ShowInList(width: 150);
            View.Property(p => p.FailMsg).Readonly().ShowInList(width: 150);
            View.Property(p => p.BarCode).Readonly().ShowInList(width: 150);
            View.Property(p => p.BatchNo).Readonly().ShowInList(width: 150);
            View.Property(p => p.Factory).UseFactoryEditor()
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.FactoryName), nameof(e.Factory.Name));
                    m.DicLinkField = keyValues;
                })
                .Cascade(p => p.WipResourceId, null).Cascade(p => p.WorkOrderId, null).Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).ShowInList(width: 150);
            View.Property(p => p.WipResource).UseDataSource((s, p, k) =>
            {
                var source = s as WoCostItem;
                if (source == null)
                {
                    return new EntityList<WipResource>();
                }
                else
                {
                    return RT.Service.Resolve<WoCostItemController>().GetWipResources(source, p, k);
                }
            })
            .UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.WipResourceName), nameof(e.WipResource.Name));
                m.DicLinkField = keyValues;
            })
            .Cascade(p => p.WorkOrderId, null).Cascade(p => p.ProductName, null).Readonly(p => p.State != SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit || p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem).ShowInList(width: 150);
            View.Property(p => p.Submiter).Readonly().ShowInList(width: 150);
            View.Property(p => p.SubmitTime).Readonly().ShowInList(width: 150);
        }
    }
}