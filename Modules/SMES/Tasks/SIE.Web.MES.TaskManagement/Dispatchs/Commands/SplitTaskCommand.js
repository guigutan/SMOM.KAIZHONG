SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.SplitTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "拆分/转移", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1)
            return false;
        if (entity != null && entity.data) {
            //当前状态(TaskStatus)为“暂停”，原状态(OldTaskStatus)为执行中、派工中、待派工的派工单
            return (entity.data.TaskStatus == 40 && (entity.data.OldTaskStatus == 30 || entity.data.OldTaskStatus == 10 || entity.data.OldTaskStatus == 0));//((entity.data.TaskStatus == 0 || entity.data.TaskStatus == 10) && entity.data.MergedStatus == 0);
        }
        return false;
    },
    execute: function (view, source) {
        var me = view;
        var indata = view.getCurrent().getData();
        SIE.AutoUI.getMeta({
            model: "SIE.MES.TaskManagement.Dispatchs.ViewModels.SplitTaskViewModel",
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
                var entity = new SIE.MES.TaskManagement.Dispatchs.ViewModels.SplitTaskViewModel();
                entity.setDispatchQty(indata.DispatchQty);
                entity.setReportQty(indata.ReportQty);
                entity.setNgQty(indata.NgQty);
                entity.setDispatchTaskId(indata.Id);
                entity.setSuspectQty(indata.SuspectQty);
                detailView._view.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "填写拆分数量".t(),
                    width: 370,
                    height: 150,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var currentView = detailView._view;
                            var splitTask = currentView.getCurrent().getData();
                            if (splitTask.Qty <= 0) {
                                SIE.Msg.showError("拆分数量必须大于0！".t());
                                return false;
                            }

                            var maxQty = splitTask.DispatchQty - splitTask.ReportQty - splitTask.SuspectQty;
                            if (splitTask.Qty > maxQty) {
                                SIE.Msg.showError(Ext.String.format('拆分数量必须小于等于{0}（任务数量-已报工数量-可疑品数量）！'.L10N(), maxQty));
                                return false;
                            }
                            if (splitTask.WipResourceId == null || splitTask.WipResourceId == '' || splitTask.WipResourceId == 0) {
                                SIE.Msg.showError("资源不能为空！".t());
                                return false;
                            }

                            me.execute({
                                data: splitTask,
                                success: function (res) {
                                    var errMsg = res.Result;
                                    if (errMsg == '拆分成功'.t())
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