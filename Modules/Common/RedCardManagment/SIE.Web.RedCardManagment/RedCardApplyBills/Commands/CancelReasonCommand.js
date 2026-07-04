SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.CancelReasonCommand', {
    meta: { text: "取消", group: "edit", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (view) {
        if (view.getSelection().length != 1) return false;
        var billStatus = view.getCurrent().getBillStatus();
        if (billStatus == SIE.RedCardManagment.RedCardApplyBills.BillStatus.ToDo || billStatus == SIE.RedCardManagment.RedCardApplyBills.BillStatus.Reject) return true;
        return false;
    },
    execute: function (view, source) {
        var current = view.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.Web.RedCardManagment.RedCardApplyBills.ViewModels.CancelReasonViewModel",
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
                entity.setRedCardApplyBillId(current.data.Id);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "取消红牌申请单".t(),
                    width: 505,
                    height: 205,
                    items: ui,
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.Reason === null) {
                                SIE.Msg.showError("取消红牌申请单的原因不能为空!".t());
                                return false;
                            } else if (indata.Reason.length >= 1000) {
                                SIE.Msg.showError("取消红牌申请单的原因不能大于或等于1000个字符!".t());
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