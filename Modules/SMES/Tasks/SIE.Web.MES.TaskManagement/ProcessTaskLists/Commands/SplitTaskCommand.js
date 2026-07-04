SIE.defineCommand('SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands.SplitTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "生成任务单", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length != 1)
            return false;
        if (entity != null && entity.data) {
            return (entity.data.TasksGeneratedQty < entity.data.WorkOrderPlanQty);
        }
        return false;
    },
    execute: function (view, source) {
        var me = view;
        var indata = view.getCurrent().getData();
        SIE.AutoUI.getMeta({
            model: "SIE.MES.TaskManagement.ProcessTaskLists.SplitTaskViewModel",
            ingoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var entity = new SIE.MES.TaskManagement.ProcessTaskLists.SplitTaskViewModel();
                entity.setProcessName(indata.ProcessName);
                entity.setDispatchQty(indata.WorkOrderPlanQty);
                entity.setDispatchedTaskQty(indata.TasksGeneratedQty);
                entity.setCopies(1);
                entity.setRemainQty(indata.WorkOrderPlanQty - indata.TasksGeneratedQty);
                entity.setWoNo(indata.No);
                entity.setProcessId(indata.ProcessId);
                entity.setRountingPrcossId(parseFloat(indata.Id));
                
                detailView._view.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "生成任务单".t(),
                    width: 780,
                    height: 300,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var currentView = detailView._view;
                            var splitTask = currentView.getCurrent().getData();
                            if (splitTask.Qty <= 0) {
                                SIE.Msg.showError("本次生成数量必须大于0！".t());
                                return false;
                            }
                            if (splitTask.Copies <= 0) {
                                SIE.Msg.showError("份数必须大于0！".t());
                                return false;
                            }

                            var maxQty = splitTask.DispatchQty - splitTask.DispatchedTaskQty;
                            if (splitTask.Qty * splitTask.Copies > maxQty) {
                                SIE.Msg.showError(Ext.String.format('(本次生成数*份数)必须小于或等于{0}（工单工序数量-已生成任务数）！'.L10N(), maxQty));
                                return false;
                            }

                            me.execute({
                                data: splitTask,
                                success: function (res) {
                                    var errMsg = res.Result;
                                    if (errMsg == '生成成功'.t())
                                        view.reloadData();
                                    SIE.Msg.showMessage(errMsg);
                                }
                            });
                        }
                    }
                });
            }
        });
    }
})