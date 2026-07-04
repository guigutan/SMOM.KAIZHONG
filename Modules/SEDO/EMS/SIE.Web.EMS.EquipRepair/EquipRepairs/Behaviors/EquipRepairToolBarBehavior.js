Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairToolBarBehavior', {
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

            var cbbState = new Ext.form.ComboBox({
                name: 'cbbState',
                xtype: 'combobox',
                displayField: 'name',
                valueField: 'value',
                store: [
                    { value: null, name: '全部'.t() },//查询时，全部选项会过滤已完成状态
                    { value: '0', name: '报修'.t() },
                    { value: '1', name: '待维修'.t() },
                    { value: '2', name: '维修中'.t() },
                    { value: '3', name: '待确认'.t() },
                    { value: '4', name: '待评分'.t() },
                    //{ value: '5', name: '已完成' },//需求说不需要这个状态，暂时屏蔽
                    { value: '6', name: '暂停中'.t() },
                    { value: '7', name: '取消'.t() },
                    { value: '8', name: '关闭'.t() },
                ],
                queryMode: 'local',// 数据模式，local代表本地数据
                emptyText: '请选择'.t(),
                editable: false,// 是否允许输入
                forceSelection: true,// 必须选择一个选项
                fieldLabel: '维修状态'.t(),
                labelWidth: 68,
            });
            cbbState.setValue('null');

            //控件插入工具栏
            toolBar.items.push(cbbState);
        }

    },
    onViewReady: function (view) {
        _view = view;
    },
});