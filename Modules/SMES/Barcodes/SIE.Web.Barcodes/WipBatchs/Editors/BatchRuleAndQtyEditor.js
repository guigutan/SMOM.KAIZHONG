/*
 ** 批次生成的自定义下拉编辑器（获取物料的批次规则以及控制是否可选和编辑）
 * @class SIE.Web.Barcodes.WipBatchs.Editors.BatchRuleAndQtyEditor
 */

Ext.define('SIE.Web.Barcodes.WipBatchs.Editors.BatchRuleAndQtyEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.BatchRuleAndQtyEditor',
    layout: {
        type: 'hbox',
    },
    style: 'padding: 0px 0px 0px 5px',
    items: [{
        xtype: 'combobox',
        name: 'ccBatchRule',
        displayField: 'Name',
        valueField: 'Value',
        queryMode: 'local',
        emptyText: '-请选择-'.t(),
        hideLabel: true,
        margin: '0 6 0 0',
        width: 100,
        allowBlank: false,
        forceSelection: true,
        store: {
            fields: ["Name", "Value"],
            data: [
                { Name: "按产品分批".t(), Value: 0 },
                { Name: "按载具分批".t(), Value: 1 },
                { Name: "按工单分批".t(), Value: 2 },
                { Name: "按固定值".t(), Value: 3 }
            ]
        },
        listeners: {
            afterRender: function (combo) {
                var form = this.up('form');
                if (form) {
                    var entity = form.SIEView.getData();
                    SIE.invokeDataQuery({
                        method: 'GetBatchRule',
                        params: [entity.getProductId(), entity.getBatchWoId()],
                        action: 'queryer',
                        type: 'SIE.Web.Barcodes.WipBatchs.WipBatchsDataQueryer',
                        token: form.SIEView.getToken(),
                        success: function (res) {
                            var rtnData = res.Result;
                            entity.setBatchQty(rtnData.Qty);
                            combo.setValue(rtnData.BatchRule);//同时下拉框会将与name为firstValue值对应的 text显示
                            if (rtnData.Warning !== "" && rtnData.Warning !== null) {
                                SIE.Msg.showMessage(rtnData.Warning);
                            }
                        }
                    });
                }
            },
            "change": function (filed, newValue, oldValue) {
                var form = this.up('form');
                if (form) {
                    var entity = form.SIEView.getData();
                    var cb = form.SIEView.getControl().query('*[fieldLabel=生成子批次]')[0];
                    var qtyControl = Ext.getCmp('BatchRuleAndQtyEditorQty');
                    if (cb == null) {
                        return;
                    }
                    if (newValue == 3) {
                        qtyControl.setDisabled(false);
                    }
                    else {
                        qtyControl.setDisabled(true);
                    }
                    cb.setValue(false);
                    if (newValue == 1) {
                        document.getElementById(cb.inputId).disabled = 'disabled';
                    }
                    else {
                        document.getElementById(cb.inputId).disabled = '';
                    }
                    SIE.invokeDataQuery({
                        method: 'GetBatchQty',
                        params: [newValue, entity.getProductId(), entity.getBatchWoId()],
                        action: 'queryer',
                        type: 'SIE.Web.Barcodes.WipBatchs.WipBatchsDataQueryer',
                        token: form.SIEView.getToken(),
                        success: function (res) {
                            var rtnData = res.Result;
                            entity.setBatchQty(rtnData);
                        }
                    });
                }
            }
        }
    }, {
        xtype: 'numberfield',
        name: 'SetBatchQty',
        id: 'BatchRuleAndQtyEditorQty',
        bind: '{p.BatchQty}',
        width: 70,
        hideLabel: true,
        value: 1,
        minValue: 1,
        baiallowNegative: false,
        allowDecimals: false,
        allowBlank: false
    }],
});