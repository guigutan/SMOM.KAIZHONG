SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.TaskPerformerCommand', {
    meta: { text: "可执行对象保存", group: "edit" },
    execute: function (view, source) {
        var me = view;
        var dispatchTaskId = source.data.DispatchTaskId;
        var selectedIds = view.getSelectionIds();
        me.execute({
            command: Ext.getClassName(this),
            data: source.data,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg !== '保存成功')
                    SIE.Msg.showMessage(errMsg);
               // else
              //      view.reloadData();
                if (source.data.Status == 0)
                    return;
                var adoTypeBoxControl = Ext.getCmp("adoTypeBoxId");
                var adoNameBoxControl = Ext.getCmp("adoNameBoxId");
                var adoType = adoTypeBoxControl.value;
                var adoName = adoNameBoxControl.value;
                if (adoName == null)
                    adoName = "";
                SIE.invokeDataQuery({
                    type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
                    method: "GetTaskPerformerInfo",
                    params: [selectedIds, dispatchTaskId, adoType, adoName],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        if (res.Success) {
                            var taskPerformerInfo = res.Result;
                            view.isSelectedTaskPerformer = taskPerformerInfo.IsSelectedTaskPerformer;
                            var dragControl = Ext.getCmp('taskPerfomerId');
                            var grid1Id = dragControl.items.items[0].id;
                            var grid1Control = Ext.getCmp(grid1Id);
                            var store = grid1Control.getStore();
                            store.setData(taskPerformerInfo.AdoInfos);
                            grid1Control.setStore(store);
                        }
                    }
                });
            }
        });
    }
});