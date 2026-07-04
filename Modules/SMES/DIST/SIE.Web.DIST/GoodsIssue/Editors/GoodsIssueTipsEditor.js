Ext.define('SIE.Web.DIST.Editors.GoodsIssueTipsEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.GoodsIssueTipsEditor',
    items: [{
        xtype: 'textfield',
        name: 'GoodsIssueTips',
        bind: '{p.Tips}',
        hideLabel: true,
        style: "width:100%;opacity:1;",
        allowBlank: true,
        forceSelection: false,
        disabled: true,
        fieldStyle: "background-color:#D3D3D3; font-size:25px;color:#008000;height:30px;",       
    }],
});