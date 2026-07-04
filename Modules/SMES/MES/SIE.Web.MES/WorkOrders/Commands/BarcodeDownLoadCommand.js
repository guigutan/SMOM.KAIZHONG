SIE.defineCommand('SIE.Web.MES.WorkOrders.BarcodeDownLoadCommand', {
    extend: "SIE.Web.MES.Common.Commands.ImportDataCommonCommand",
    meta: { text: "条码导入", group: "edit", hierarchy: "条码", iconCls: "icon-Download icon-blue", model: "SIE.Barcodes.Barcode" },
    canExecute: function (listView) {
        return listView.getCurrent() != null;
    },
});