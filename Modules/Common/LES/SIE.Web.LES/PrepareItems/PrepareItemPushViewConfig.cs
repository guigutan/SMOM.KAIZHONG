using SIE.Items;
using SIE.Items.Items;
using SIE.LES;
using SIE.LES.Commons;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.PrepareItems.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES
{
    /// <summary>
    /// 备料模式推式视图配置
    /// </summary>
    internal class PrepareItemPushViewConfig : WebViewConfig<PrepareItemPush>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddPrepareItemPushCommand).FullName, WebCommandNames.Copy, typeof(EditPrepareItemPushCommand).FullName, WebCommandNames.Delete, typeof(SavePrepareItemPushCommand).FullName,
                 typeof(ImportPrepareItemPushCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResourceId).UsePagingLookUpEditor((p, m) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(m.WipResourceNameView), nameof(m.WipResource.Name));
                    p.DicLinkField = keyValues;
                }).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(null, c, r);
                }).HasLabel("资源编号");
                View.Property(p => p.WipResourceNameView).Readonly().HasLabel("资源名称");
                View.Property(p => p.ItemCategoryId).UseDataSource((e, c, r) =>
                {
                    var list = RT.Service.Resolve<ItemController>().GetItemSmallCategory(CategoryType.Item, null, r, c);
                    list.ForEach(p => p.TreePId = null);
                    return list;
                });
                View.Property(p => p.ItemId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<ItemController>().GetAllEnableItems(c, r, ConsumeMode.Push);
                }).UsePagingLookUpEditor((p, m) =>
                {
                    var keyValues = new Dictionary<string, string>
                    {
                        { nameof(m.ItemName), nameof(m.Item.Name) }
                    };
                    keyValues.Add(nameof(m.IsAllowEdit), nameof(m.Item.EnableExtendProperty));
                    p.DicLinkField = keyValues;
                }).Cascade(p => p.ItemExtProp, null).Cascade(p => p.ItemExtPropName, null);
                View.Property(p => p.ItemName).Readonly();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.DbField = "ItemExtProp";
                }).Readonly(p=>!p.IsAllowEdit);
                View.Property(p => p.TriggerType).UseSelectEnumBoxEditor(p =>
                {
                    p.ValuesList.Add((int)TriggerMode.XHoursBefore);
                    p.ValuesList.Add((int)TriggerMode.InvBelowSafeLevelToBeat);
                }).UseListSetting(p => p.HelpInfo = "警戒水位=产品机型节拍*最短满足时间;警戒水位,需同时满足提前小时和警戒水位才触发"); 
                View.Property(p => p.AdvanceHours).HasLabel("提前小时".L10N()+"*");
                View.Property(p => p.SatisfactionTime).Readonly(p => p.TriggerType == TriggerMode.XHoursBefore);
                View.Property(p => p.DemandType).UseSelectEnumBoxEditor(p =>
                {
                    p.ValuesList.Add((int)DemandMode.WoSurplusQty);
                    p.ValuesList.Add((int)DemandMode.StockToSafeLevelForBeat);
                    p.ValuesList.Add((int)DemandMode.StockIsSafeLevelForBeat);
                    p.ValuesList.Add((int)DemandMode.FixedQuantity);
                }).UseListSetting(p => p.HelpInfo = "安全水位=产品机型节拍*最小备料时间") ;
                View.Property(p => p.FixedQuantity).UseItemUnitEditor().Readonly(p => p.DemandType != DemandMode.FixedQuantity);
                View.Property(p => p.PreparationTime).Readonly(p => p.DemandType != DemandMode.StockToSafeLevelForBeat && p.DemandType != DemandMode.StockIsSafeLevelForBeat);
            }
        }
    }
}
