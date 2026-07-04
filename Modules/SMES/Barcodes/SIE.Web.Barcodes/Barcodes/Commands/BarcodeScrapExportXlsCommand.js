SIE.defineCommand('SIE.Web.Barcodes.BarcodeScrapExportXlsCommand', {
    extend: 'SIE.cmd.ExportXls',
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var dataCount = view.getData().data.items.length;
        //数据存在时允许导出
        if (dataCount === 0) {
            SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
            return false;
        }
        SIE.Signature.otherCheckIsNeedToSign("导出", view, function () {
            SIE.ExportExcelHelper.exportXls(view.gridConfig, view.getData().getData().items, "条码报废".t());//此处写死了文件名
        });

    },
});