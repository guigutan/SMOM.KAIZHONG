SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.ImportSaleOrderCommand', {
    extend: 'SIE.Web.Pcb.SO.Common.Commands.ImportDataCommonCommand',
    meta: { text: "数据导入", group: "business", iconCls: "icon-Download icon-blue", model: "SIE.Web.SO.SaleOrders.ViewModels.SaleOrderCheckDataViewModel" },

});