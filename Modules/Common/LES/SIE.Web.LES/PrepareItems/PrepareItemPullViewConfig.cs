using SIE.Items;
using SIE.Items.Items;
using SIE.LES;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.PrepareItems.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES
{
    /// <summary>
    /// 备料模式拉式视图配置
    /// </summary>
    internal class PrepareItemPullViewConfig : WebViewConfig<PrepareItemPull>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddPrepareItemPullCommand).FullName, WebCommandNames.Copy, WebCommandNames.Edit, WebCommandNames.Delete, typeof(SavePrepareItemPullCommand).FullName,
                 typeof(ImportPrepareItemPullCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<LinesideWarehouseController>().GetAvailableLinesideWarehouses(c, r);
                });
                View.Property(p => p.ItemCategoryId).UseDataSource((e, c, r) =>
                {
                    var list = RT.Service.Resolve<ItemController>().GetItemSmallCategory(CategoryType.Item, null, r, c);
                    list.ForEach(p => p.TreePId = null);
                    return list;
                });
                View.Property(p => p.ItemId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<ItemController>().GetAllEnableItems(c, r, ConsumeMode.Pull);
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
                }).Readonly(p => !p.IsAllowEdit);
                View.Property(p => p.TriggerType).UseSelectEnumBoxEditor(p =>
                {
                    p.ValuesList.Add((int)TriggerMode.BelowSafe);
                }); 
                View.Property(p => p.LowestStage).HasLabel("最低安全水位");
                View.Property(p => p.DemandType).UseSelectEnumBoxEditor(p =>
                {
                    p.AllowBlank = false;
                    p.ValuesList.Add((int)DemandMode.MaxStock);
                    p.ValuesList.Add((int)DemandMode.FixedQuantity);
                });
                View.Property(p => p.MaxStock).UseItemUnitEditor();
                View.Property(p => p.FixedQuantity).UseItemUnitEditor();
            }
        }
    }
}
