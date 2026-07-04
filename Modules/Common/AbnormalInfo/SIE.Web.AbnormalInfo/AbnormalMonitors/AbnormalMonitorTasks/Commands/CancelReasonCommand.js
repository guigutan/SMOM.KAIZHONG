SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.CancelReasonCommand', {
    meta: { text: "取消", group: "edit", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (view) {
        if (view.getSelection().length != 1) return false;
        if (view.getCurrent().getTaskState() == SIE.AbnormalInfo.Common.TaskStateEnum.Done || view.getCurrent().getTaskState() == SIE.AbnormalInfo.Common.TaskStateEnum.Cancel) return false;
        return true;
    },
    execute: function (view, source) {
        var current = view.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.Web.AbnormalInfo.AbnormalMonitors.ViewModels.CancelReasonViewModel",
            ingoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                entity.setAbnormalMonitorTaskId(current.data.Id);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "取消异常任务".t(),
                    width: 505,
                    height: 205,
                    items: ui,
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.Reason === null) {
                                SIE.Msg.showError("取消异常任务的原因不能为空!".t());
                                return false;
                            } else if (indata.Reason.length >= 1000) {
                                SIE.Msg.showError("取消异常任务的原因不能大于或等于1000个字符!".t());
                                return false;
                            }
                            view.execute({
                                data: indata,
                                success: function (res) {
                                    if (res.Result) {
                                        win.close();
                                        view.reloadData();
                                        SIE.Msg.showInstantMessage("取消成功".t());
                                    }
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});