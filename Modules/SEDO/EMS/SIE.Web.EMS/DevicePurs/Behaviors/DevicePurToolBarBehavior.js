Ext.define('SIE.Web.EMS.DevicePurs.Behaviors.DevicePurToolBarBehavior', {
    _view: null,
    /**
    * view生命周期函数--view生成前
    * @param {*} meta 实体视图元数据
    * @param {*} curEntity 当前操作实体(可空)
    */
    beforeCreate: function (meta, curEntity) {
        me = this;
        _view = null;
        if (!meta)
            return;

        var gridConfig = meta.gridConfig;

        var toolBar = gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
        if (!Ext.isEmpty(toolBar)) {

            var txtKeyword = new Ext.form.TextField({
                width: 250,
                allowBlank: true,
                name: 'txtKeyword',
                labelAlign: 'right',
                fieldLabel: '关键字'.t(),
                blankText: '请输入设备编码或设备名称',
            });

            //控件插入工具栏
            toolBar.items.push(txtKeyword);
        }

    },
    onViewReady: function (view) {
        _view = view;
    },
});