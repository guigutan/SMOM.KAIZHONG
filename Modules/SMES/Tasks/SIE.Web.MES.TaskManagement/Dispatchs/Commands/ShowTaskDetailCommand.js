SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.ShowTaskDetailCommand', {
    meta: { text: "个人任务详情", group: "edit", iconCls: "icon-UpList icon-blue" },
    /**
     * 执行
     * @method execute
     * @param {ListLogicalView} view 
     * @param {source} 数据源
     */
    execute: function (view, source) {
        var me = view;
        var target = this;
        SIE.AutoUI.getMeta({
            model: "SIE.MES.TaskManagement.Dispatchs.ViewModels.TaskDetailViewModel",
            ignoreCommands: false,
            isDetail: false,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                token = mainBlock.token;
                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "个人任务详情".t(),
                    width: 900,
                    height: 500,
                    buttons: [
                        { xtype: "button", text: "确定".t(), hidden: true },
                        {
                            xtype: "button", text: "关闭".t(), hidden: false, handler: function () {
                                this.up('window').close()
                            }
                        }
                    ],
                    items: ui,
                    id: "TaskDetail001",
                });

                var filter = {
                    Method: 'GetReportDispatchTaskList',
                    Parameters: [source]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer',
                });
            }
        });
    },
});