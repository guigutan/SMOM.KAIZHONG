SIE.defineCommand('SIE.Web.Fixtures.MaintainTasks.Commands.ShowTaskDetail', {
    meta: { text: "查看明细", group: "edit", iconCls: "icon-FileEye icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity && entity.data.State !== 5)
            return true;
        else
            return false;
    },
    /**
     * 执行
     * @method execute
     * @param {ListLogicalView} view 
     * @param {source} 数据源
     */
    execute: function (view, source) {
        var me = this;        
        var task = view.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.Fixtures.MaintainTasks.MaintainTaskDetail",
            module: "SIE.Fixtures.Fixtures.Accounts.FixtureAccountModel,SIE.Fixtures",
            viewGroup: 'ShowMaintainDetailView',
            ignoreCommands: false,
            isDetail: false,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                me.token = mainBlock.token;
                var listView = SIE.AutoUI.createListView(mainBlock);
                me.view = listView;
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "查看明细".t(),
                    width: 700,
                    height: 500,
                    buttons:[],
                    items: ui,
                    id: "maintainTask001",
                });

                var filter = {
                    Method: 'GetMaintainTaskDetails',
                    Parameters: [task.getId()]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: view.token,
                    type: 'SIE.Web.Fixtures.MaintainTasks.DataQuery.TaskDataQueryer',
                });
            }
        });
    },
});