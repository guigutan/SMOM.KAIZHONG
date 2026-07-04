Ext.define('SIE.Web.MES.TeamManagement.Editors.ClearWorkGroupEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.clearWorkGroupEditor',
    layout: {
        type: 'vbox',
    },
    style: 'padding: 0px 0px 0px 5px',
    items: [{
        xtype: 'button',
        text: '清除'.t(),
        handler: function () {
            var p = this.findParentByType('clearWorkGroupEditor');
            var form = this.up('form');
            if (form) {
                var entity = form.SIEView.getData();
                //清空班组
                entity.setWorkGroupId(0);
                entity.setWorkGroupId_Display('');
            }
        },
        style: 'border-radius:4px;border-width:0px;',
    }],
});