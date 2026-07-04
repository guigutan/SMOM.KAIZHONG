Ext.define('SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountResumeBehavior', {
    _view: null,
    /**
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
                    { value: '1', name: '维修'.t() },
                    { value: '2', name: '保养'.t() },
                    { value: '3', name: '变更'.t() },
                    { value: '4', name: '点检'.t() },
                    { value: '10', name: '润滑'.t() },
                    { value: '11', name: '资产调拨'.t() },
                    { value: '12', name: '特种设备定检'.t() },
                    { value: '13', name: '计量设备定检'.t() },

                    { value: '14', name: '闲置'.t() },
                    { value: '15', name: '封存'.t() },
                    { value: '16', name: '闲置启用'.t() },
                    { value: '17', name: '封存启用'.t() },
                    { value: '18', name: '计划维修'.t() },
                    { value: '19', name: '领用发放'.t() },
                    { value: '20', name: '借用发放'.t() },
                    { value: '21', name: '资产归还'.t() },
                    { value: '22', name: '报废'.t() },
                    { value: '23', name: '设备立卡'.t() },
                    { value: '24', name: '处置'.t() },
                    { value: '25', name: '设备借还' },
                ],
                queryMode: 'local',// 数据模式，local代表本地数据
                emptyText: '请选择'.t(),
                editable: false,// 是否允许输入
                forceSelection: true,// 必须选择一个选项
                fieldLabel: '类型'.t(),
                labelWidth: 68,
            });
            cbResumeState.setValue('null');

            //控件插入工具栏
            toolBar.items.push(cbResumeState);
        }

    },
    onViewReady: function (view) {
        _view = view;
    },
});