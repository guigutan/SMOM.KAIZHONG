Ext.define('SIE.Web.LES.MaterialReceptions.Behaviors.AddPageBehavior', {
    beforeCreate: function (meta, curEntity) {
        if (!meta)
            return;
        var me = this;
        var gridConfig = meta.gridConfig;

        var toolBar = gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
        if (!Ext.isEmpty(toolBar)) {
            if (meta.viewGroup === 'AddDetailPageView') {
                var txtKeyword = new Ext.form.TextField({
                    id:'MaterialReceptions_addDetail',
                    width: 400,
                    allowBlank: true,
                    name: 'txtKeyword',
                    labelAlign: 'right',
                    fieldLabel: '标签/批次号/物料号'.t(),
                    labelWidth: 120,
                    emptyText: '请扫描标签'.t(),
                    listeners: {
                        specialkey: function (field, e) {
                            if (e.getKey() === Ext.EventObject.ENTER) {
                                document.getElementById("SIE_LES_MaterialReceptions_ViewModels_MaterialReceptionAddViewModel-SIE_Web_LES_MaterialReceptions_Commands_AddByDetailOrderCommand_2-btnInnerEl").click();
                            }
                        }
                    }
                });

                //控件插入工具栏
                toolBar.items.unshift(txtKeyword);
            }
            else {
                var txtKeyword = new Ext.form.TextField({
                    id:'MaterialReceptions_addOrder',
                    width: 300,
                    allowBlank: true,
                    name: 'txtKeyword',
                    labelAlign: 'right',
                    fieldLabel: '备料单号'.t(),
                    labelWidth:60,
                    emptyText: '请输入备料单号'.t(),
                    listeners: {
                        specialkey: function (field, e) {
                            if (e.getKey() === Ext.EventObject.ENTER) {
                                document.getElementById("SIE_LES_MaterialReceptions_ViewModels_MaterialReceptionAddViewModel-SIE_Web_LES_MaterialReceptions_Commands_AddByDetailOrderCommand_3-btnInnerEl").click();
                            }
                        }
                    }
                });

                //控件插入工具栏
                toolBar.items.unshift(txtKeyword);
            }
        }

    },
});