SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.ExportSaleOrderCommand', {
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    execute: function (view) {
        var me = this;
        var record = view._relations[0]._target.getCurrent();
        var criteria = record.data;
        SIE.invokeDataQuery({
            method: 'ExportSaleOrder',
            params: [criteria],
            action: 'queryer',
            type: 'SIE.Web.SO.SaleOrders.DataQuery.SaleOrderDataQueryer',
            token: view.getToken(),
            success: function (res) {
                if (res.Success) {
                    var exportData = res.Result;
                    if (exportData && exportData.Tables && exportData.Tables.length === 0) {
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                        }, 1000);
                        SIE.Msg.showMessage("没有可导出的数据".L10N());
                    }
                    else {
                        me.generateExcel(exportData);
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                        }, 1000);
                    }
                }
            }
        });
    },

    generateExcel: function (exportData) {
        SIE.Web.MES.Common.Scripts.Helpers.ExportExcelHelper.tablesToMultiSheetExcel(exportData, '销售订单表'.L10N() + Ext.util.Format.date(new Date(), 'Ymdhis'), false);
    }
});

