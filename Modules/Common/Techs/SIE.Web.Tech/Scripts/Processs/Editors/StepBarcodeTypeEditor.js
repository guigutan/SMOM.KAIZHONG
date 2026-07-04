/*
 ** 批次生成的自定义下拉编辑器（获取物料的批次规则以及控制是否可选和编辑）
 * @class SIE.Web.Barcodes.WipBatchs.Editors.BatchRuleAndQtyEditor
 */
Ext.define('SIE.Web.Tech.Processs.Editors.StepBarcodeTypeEditor', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.StepBarcodeTypeEditor',
});

Ext.define('SIE.Web.Tech.Processs.Editors.StepBarcodeTypeEditorRouting', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.StepBarcodeTypeEditorRouting',
});

Ext.define('SIE.Web.Tech.Processs.Editors.ProcessTypeEditor', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.ProcessTypeEditor',
    listeners: {
        "afterrender": function (comp) {
            var items = comp.getStore().data.items.where(function (p) { return p.data.value != 5 });
            var newStore = new Ext.data.Store();
            newStore.data.add(items);
            comp.setStore(newStore);
        }
    }
});