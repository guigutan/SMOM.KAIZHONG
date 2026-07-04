SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.DispatchTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "派工", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.TaskStatus != 0 && selecteditems[i].data.TaskStatus != 10) {
                    return false;
                }
            }
            //当资源来源类型只有一种，且是同一个工作中心，才能多选一起派工
            if (selecteditems.groupBy(p => p.getResourceSourceType()).length == 1 && selecteditems.groupBy(p => p.getResourceId()).length == 1)
                return true;
        }
        return false;
    },
    execute: function (view) {
        var _view = view;
        var me = this;
        SIE.Msg.askQuestion(Ext.String.format('是否派工选中任务单？'.t()),
            function () {

                //var IsDispatchTaskWipResource = false;
                var TaskWipResourceList = [];
                //当选择的为工作中心、且验证为需要派工到产线的时候，弹窗框，选择对应的产线
                if (view.getCurrent().getResourceSourceType() == 2) {
                    SIE.invokeDataQuery({
                        type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
                        method: "IsDispatchTaskWipResource",
                        params: [_view.getSelectionIds()],
                        async: false,
                        token: _view.token,
                        callback: function (res) {
                            TaskWipResourceList = res.Result;
                        }
                    });
                }

                if (TaskWipResourceList.length > 0) {

                    var id = TaskWipResourceList.join(',');//view.getSelectionIds().join(',');
                    SIE.AutoUI.getMeta({
                        model: "SIE.MES.TaskManagement.Dispatchs.ViewModels.DispatchTaskViewModel",
                        ingoreCommands: true,
                        isDetail: true,
                        ignoreQuery: true,
                        callback: function (res) {
                            var mainBlock;
                            if (res.mainBlock)
                                mainBlock = res.mainBlock;
                            else
                                mainBlock = res;
                            var detailView = SIE.AutoUI.createDetailView(res);
                            var entity = new detailView._model();
                            entity.setTaskId(id);
                            entity.setWorkCenterCode(view.getCurrent().getResourceCode());
                            detailView._setDefaultValue(entity);
                            detailView.setData(entity);
                            var ui = detailView.getControl();
                            var win = SIE.Window.show({
                                title: "派工选择资源".t(),
                                width: 400,
                                height: 200,
                                items: ui,
                                callback: function (btn) {
                                    if (btn == "确定".t()) {
                                        var currentView = detailView;
                                        var success = true;
                                        SIE.invokeDataQuery({
                                            type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
                                            method: "DispatchTaskWipResource",
                                            params: [currentView.getCurrent().getData()],
                                            async: false,
                                            token: _view.token,
                                            callback: function (res) {
                                                success = res.Success;
                                                if (res.Success == true) {
                                                    SIE.Msg.wait("正在派工......".t());
                                                    _view.execute({
                                                        withIds: true,
                                                        selectIds: _view.getSelectionIds(),
                                                        success: function (res) { //回调
                                                            var errMsg = res.Result;
                                                            if (errMsg == '派工成功'.t())
                                                                _view.reloadData();
                                                            SIE.Msg.showMessage(errMsg);
                                                        }
                                                    });
                                                }
                                            }
                                        });

                                        return success;
                                    }
                                }
                            });
                        }
                    });
                }
                else {
                    SIE.Msg.wait("正在派工......".t());
                    view.execute({
                        withIds: true,
                        selectIds: view.getSelectionIds(),
                        success: function (res) { //回调
                            var errMsg = res.Result;
                            if (errMsg == '派工成功'.t())
                                view.reloadData();
                            SIE.Msg.showMessage(errMsg);
                        }
                    });
                }
                
            });
    }
})