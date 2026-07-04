/*
* 实现列配置了编辑器的显示
* */
Ext.define('SIE.Web.Items.Items.ViewModels.DefinitionComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.DefinitionComboList',
    //行双击事件
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        this.callParent(arguments);
        var me = this;
        var current = me._getSIEView().getCurrent();
        current.setPropertyType(record.data.PropertyType);
        current.setCatalogType(record.data.CatalogTypeId_Display);
        
    },
});


