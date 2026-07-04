Ext.define('SIE.Web.Inventory.Transactions.Editors.BillTemplateEditor', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.BillTemplateEditor',
    triggerCls: "x-form-arrow-trigger",
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        var me = this;
        if (record) {
            me._SelectItems = [];
            me.setValue(record);
            me._SelectItems.push(record);
            me._win.hide();
            var view = e.view.up().ownerCt.up().up().up().ownerCt.SIEView;
            var data = view.getCurrent();
            var id = data.getId();
            var tempId = record.data.Id;
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.Inventory.Transactions.DataQueryer.FunctionDataQuery",
                method: 'SaveBillTemplateId',
                token: view.token,
                params: [id, tempId],
                success: function (res) {
                    view.reloadData();
                }
            })
        }
    },
});
