SIE.defineCommand('SIE.Web.Dock.DockAppoints.Commands.CancelAppointCommand', {
    meta: { text: "取消预约", group: "edit", iconCls: "iconfont icon-Cancel icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var sel = view.getSelection();
        var dt = new Date();
        if (sel.any(function (p) { return p.getIsCancelAppoint() == true || p.getAppointEndDate() <= dt; }))
            return false;

        return true;
    },
    execute: function (view, source) {
        var me = this;
        var billIds = view.getSelectionIds();
        SIE.AutoUI.getMeta({
            model: 'SIE.Dock.ViewModels.CancelAppointViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                entity.setType(0);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "取消预约".t(),
                    width: 500,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.CancelReasonId == null || indata.CancelReasonId <= 0) {
                                SIE.Msg.showError("取消原因不能为空！".t());
                                return false;
                            }

                            var cancelReason = indata.ReasonName + "：" + indata.ReasonDesc;
                            view.execute({
                                data: { BillIds: billIds, ReasonDesc: cancelReason },
                                success: function (res) {
                                    view.reloadData();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });
                        }
                    }
                });
            },
        });
    }
});