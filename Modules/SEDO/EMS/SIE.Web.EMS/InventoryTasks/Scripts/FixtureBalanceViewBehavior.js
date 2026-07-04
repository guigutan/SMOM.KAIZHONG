Ext.define('SIE.Web.EMS.InventoryTasks.FixtureBlanceViewBehavior', {
    _view: null,
    /**  工治具盘点平账视图行为
          * view生命周期函数--view生成前
          * @param {*} meta 实体视图元数据
          * @param {*} curEntity 当前操作实体(可空)
          */
    beforeCreate: function (meta, curEntity) {
        me = this;
        _view = null;
        if (!meta) {
            return;
        }
        var gridConfig = meta.gridConfig;

        var toolBar = gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
        if (!Ext.isEmpty(toolBar)) {

            var cbResumeState = new Ext.form.ComboBox({
                name: 'cbResumeState',
                xtype: 'combobox',
                displayField: 'name',
                valueField: 'value',
                store: [
                    { value: null, name: '全部'.t() },//查询时，全部选项会过滤已完成状态  { value: '0', name: '报修' },
                    { value: '10', name: '正常'.t() },
                    { value: '20', name: '信息变动'.t() },
                    { value: '30', name: '盘盈'.t() },
                    { value: '40', name: '盘亏'.t() },
                    { value: '50', name: '盘盈+盘亏'.t() }

                ],
                queryMode: 'local',// 数据模式，local代表本地数据
                emptyText: '请选择'.t(),
                editable: false,// 是否允许输入
                forceSelection: true,// 必须选择一个选项
                fieldLabel: '盘点结果'.t(),
                labelWidth: 68,
            });
            cbResumeState.setValue('null');

            //控件插入工具栏
            toolBar.items.splice(1, 0, cbResumeState)
        }

    },
    onViewReady: function (view) {
        _view = view;
    },
});