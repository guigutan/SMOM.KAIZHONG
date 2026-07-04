Ext.define('SIE.Web.EMS.InventoryTasks.InventoryTaskSparePartBehavior', {
    /*
     * view生命周期函数--view生成前
     * @param {*} meta 实体视图元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {
        if (!meta) {
            return;
        }

        var gridConfig = meta.gridConfig;

        var toolBar = gridConfig.dockedItems.first(function (p) {
            return p.xtype == "toolbar"
        });

        if (!Ext.isEmpty(toolBar)) {
            var txtSparePartCode = new Ext.form.TextField({
                width: 250,
                allowBlank: true,
                name: 'txtSparePartCode',
                labelAlign: 'right',
                fieldLabel: '',
                emptyText: '请输入备件编码'.t(),
            });

            //控件插入工具栏
            toolBar.items.splice(0, 0, txtSparePartCode);
        }
    },
});