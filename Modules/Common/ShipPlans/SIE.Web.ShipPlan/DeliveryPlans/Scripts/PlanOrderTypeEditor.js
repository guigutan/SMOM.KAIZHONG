
/*序列号管理*/
Ext.define('SIE.Web.ShipPlan.PlanOrderTypeEditor', {
    extend: 'SIE.grid.column.ComboBox',
    alias: 'widget.planOrderTypeEditor', 
    renderer: function (value, meta) {
        if (value == SIE.Inventory.Commom.OrderType.WorkFeed.value) {
            return "工单发料".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.SaleOut.value) {
            return "销售出库".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.OutWorkFeed.value) {
            return "委外工单发料".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.OutWorkFeedUse.value) {
            return "委外工单耗料".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.OutAllotReturn.value) {
            return "委外调拨退料".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.OtherOut.value) {
            return "其他出库".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.SupplierReturn.value) {
            return "供应商退货".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.DirectAllocate.value) {
            return "直接调拨".t();
        }
        else if (value == SIE.Inventory.Commom.OrderType.TwoAllocate.value) {
            return "两步调拨".t();
        }        
    }
});