Ext.define('SIE.Web.EMS.FixedAssets.Accounts.Scripts.FixedAssetResumeBehavior', {
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

            var cbResumeState = new Ext.form.ComboBox({
                name: 'cbResumeState', 
                xtype: 'combobox',
                displayField: 'name',
                valueField: 'value',
                store: [
                    { value: null, name: '全部'.t() },//查询时，全部选项会过滤已完成状态
                    { value: '0', name: '报修'.t() },
                    { value: '1', name: '维修'.t() },
                    { value: '2', name: '保养'.t() },
                    { value: '3', name: '变更'.t() },
                    { value: '4', name: '点检'.t() }
                ],
                queryMode: 'local',// 数据模式，local代表本地数据
                emptyText: '请选择'.t(),
                editable: false,// 是否允许输入
                forceSelection: true,// 必须选择一个选项
                fieldLabel: '类型'.t(),
                labelWidth: 40,
            });
            cbResumeState.setValue('null');

            //控件插入工具栏
            toolBar.items.unshift(cbResumeState);
        }

    },
    onViewReady: function (view) {
        _view = view;
    },
});