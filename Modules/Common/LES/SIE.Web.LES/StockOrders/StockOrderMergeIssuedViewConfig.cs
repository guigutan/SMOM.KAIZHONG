using SIE.LES.LinesideWarehouses;
using SIE.LES.StockOrders;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.LES.StockOrders.Commands;
using System.Collections.Generic;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
	/// 备料单合并下发规则视图配置
	/// </summary>
	internal class StockOrderMergeIssuedViewConfig : WebViewConfig<StockOrderMergeIssued>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.AddBehavior("SIE.Web.LES.StockOrders.Scripts.StockOrderMergeIssuedBehavior");
            View.ReplaceCommands(WebCommandNames.Add, typeof(StockOrderMergeIssuedAddCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(StockOrderMergeIssuedEditCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(StockOrderMergeIssuedDeleteCommand).FullName);
            View.UseCommands(WebCommandNames.Save);
            View.UseCommands(typeof(StockOrderMergeIssuedEnableCommand).FullName);
            View.UseCommands(DisableCommand.CommandName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.LinesideWarehouseId).UseDataSource((e, c, r) =>
            {
                return RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouses(c, r);
            }).UsePagingLookUpEditor((m, e) =>
            {
                m.DisplayField = LinesideWarehouse.WarehouseCodeProperty.Name;
                var keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.WipResourceId), nameof(e.LinesideWarehouse.WipResouceId));
                keyValues.Add(nameof(e.WipResourceName), nameof(e.LinesideWarehouse.WipResouceName));
                keyValues.Add(nameof(e.WarehouseId), nameof(e.LinesideWarehouse.WarehouseId));
                keyValues.Add(nameof(e.WarehouseName), nameof(e.LinesideWarehouse.WarehouseName));
                keyValues.Add(nameof(e.WarehouseCode), nameof(e.LinesideWarehouse.WarehouseCode));
                m.DicLinkField = keyValues;
                m.ReloadDataOnPopping = true;
                m.BindDisplayField = StockOrderMergeIssued.WarehouseCodeProperty.Name;

            }).Readonly(p => p.State == Domain.State.Enable).HasLabel("线边仓");
            View.Property(p => p.WipResourceName).Readonly();
            View.Property(p => p.StockModel).Readonly(p => p.State == Domain.State.Enable);
            View.Property(p => p.State).Readonly();
            View.ChildrenProperty(p => p.StockOrderMergeTimesList).Show(ChildShowInWhere.All);
        }
    }
}
