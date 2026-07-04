SIE.defineCommand('SIE.Web.Dock.DockQueues.Commands.AssignDockCommand', {
    meta: { text: "分配月台", group: "edit", iconCls: "icon-CalendarText icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }

        var cur = view.getCurrent();
        if (cur == null) {
            return false;
        }

        if (cur.getQueueState() != 0 && cur.getQueueState() != 1) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var me = this;
        var billIds = view.getSelectionIds();
        var bill = view.getCurrent();
        SIE.AutoUI.getMeta({
            model: 'SIE.Dock.ViewModels.AssignDockViewModel',
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
                entity.setYardId(bill.getYardZoneId());
                entity.setAppointType(bill.getAppointType());
                entity.setDockQueueId(bill.getId());
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "分配月台".t(),
                    width: 500,
                    height: 200,
                    items: ui,
                    buttons: ['确定并立即分配'.t(), '确定'.t(), '取消'.t()],
                    callback: function (btn) {
                        if (btn == "确定".t() || btn == "确定并立即分配".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.DockMaintainId == null || indata.DockMaintainId <= 0) {
                                SIE.Msg.showError("月台不能为空！".t());
                                return false;
                            }

                            if (btn == "确定并立即分配".t()) {
                                indata.IsAtOnceAssign = true;
                            }

                            view.execute({
                                data: indata,
                                success: function (res) {
                                    win.close();
                                    view.reloadData();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });

                            return false;
                        }
                    }
                });
            },
        });
    }
});