Ext.define('SIE.Web.Andon.AndonAverageTime.Scripts.AndonAverageTimeLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'AndonAverageTimeLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,
    requestData: null,
    _listView: null,

    /**
     * 初始化界面布局
     * @param regions 聚合块
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        // 建立关联，使查询命令能获取到 layout 实例
        regions.main._view._relations[0]._target.mainLayout = me;

        // 获取原有的工具栏（包含查询、导出等命令按钮）
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });

        // 创建列表视图
        var listView = me.createListView();
        this._listView = listView;

        // 返回容器布局：上方工具栏，中间列表视图
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false
            }, {
                region: 'center',
                layout: 'fit',
                xtype: 'panel',
                id: "andonAverageTimeMainPanel",
                border: false,
                items: [listView.getControl()]
            }]
        });
    },

    /**
     * 创建列表视图（基于 AndonAverageTimeViewModel）
     */
    createListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.Andon.AndonAverageTime.AndonAverageTimeViewModel',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });
        if (meta && meta.gridConfig) {
            meta.gridConfig.manageHeight = true;
        }
        return SIE.AutoUI.createListView(meta);
    },

    /**
     * 查询数据（通过 DataQueryer 调用后端 Controller）
     * @param {Object} criteria 查询条件（AndonAverageTimeViewModelCriteria）
     * @param {String} token 视图令牌
     */
    loadReportData: function (criteria, token) {
        var me = this;
        if (Ext.isEmpty(token)) return;
        this._token = token;
        this._criteria = criteria;

        if (me._isRunning) return;
        me._isRunning = true;

        // 调用后端 Queryer（需要您创建 AndonAverageTimeDataQueryer，并实现 GetReportData 方法）
        SIE.invokeDataQuery({
            method: 'GetReportData',
            params: [criteria],
            action: 'queryer',
            async: true,
            type: 'SIE.Web.Andon.AndonAverageTime.AndonAverageTimeDataQueryer',
            token: token,
            success: function (res) {
                if (res.Success) {
                    // 后端返回的 res.Result 就是 EntityList<AndonAverageTimeViewModel>
                    var resultData = res.Result;
                    me.bindReportInfos(resultData);
                    me.requestData = resultData;
                }
                Ext.getBody().unmask();
                me._isRunning = false;
            },
            error: function () {
                Ext.getBody().unmask();
                me._isRunning = false;
            }
        });
    },

    /**
     * 绑定数据到列表
     * @param {EntityList} data 后端返回的列表数据（直接是 AndonAverageTimeViewModel 集合）
     */
    bindReportInfos: function (data) {
        // data 即为 EntityList<AndonAverageTimeViewModel>
        this._listView.getData().loadData(data);

        var records = this._listView.getData().getData().items;
        records.forEach(function (record) {
            if (record) record.markSaved();
        });

        var mainPanel = Ext.getCmp("andonAverageTimeMainPanel");
        if (mainPanel) {
            mainPanel._dataLoaded = true;
        }
    }
});