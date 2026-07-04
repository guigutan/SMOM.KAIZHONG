using SIE.ERPInterface.Smom.InventoryControl;
using SIE.MetaModel.View;
using SIE.Warehouses;

namespace SIE.Web.ERPInterface.InventoryControl
{
    /// <summary>
    /// 库存对照表查询视图
    /// </summary>
    public class InventoryControlCriteriaViewConfig : WebViewConfig<InventoryControlViewModelCriteria>
    {
        protected override void ConfigView()
        {
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.ERPInterface.InventoryControl.Commands.InventoryControlQueryCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ErpLotCode).Show();
                View.Property(p => p.WarehouseCode).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Warehouse).FullName;
                    p.XType = "InvMultiWhComboPopup";
                    p.LinkField = Warehouse.CodeProperty.Name;
                    p.ValueField = Warehouse.CodeProperty.Name;
                    p.DisplayField = Warehouse.CodeProperty.Name;
                    p.Editable = true;
                    p.Separator = ",";
                }).Show()
               .UseListSetting(f => f.HelpInfo = "查多个用英文逗号分隔");
                View.Property(p => p.ErpWarehouseCode).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(ErpWarehouse).FullName;
                    p.XType = "InvMultiWhComboPopup";
                    p.LinkField = ErpWarehouse.CodeProperty.Name;
                    p.ValueField = ErpWarehouse.CodeProperty.Name;
                    p.DisplayField = ErpWarehouse.CodeProperty.Name;
                    p.Editable = true;
                    p.Separator = ",";
                }).Show().UseListSetting(f => f.HelpInfo = "查多个用英文逗号分隔");
                View.Property(p => p.IsShowDifferent).Show();
                View.Property(p => p.IsShowZero).Show();
            }
        }
    }
}
