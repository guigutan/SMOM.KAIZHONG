SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.PrintDispatchTaskCommand', {
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var me = this;
        var taskIds = view.getSelectionIds();
        SIE.AutoUI.getMeta({
            model: 'SIE.MES.TaskManagement.Dispatchs.ViewModels.DispatchTaskPrintViewModel',
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
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "打印".t(),
                    width: 400,
                    height: 240,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.BillTemplateId == null) {
                                SIE.Msg.showError("模板不能为空!".t());
                                return false;
                            }
                            else {
                                view.execute({
                                    data: { TaskIdList: taskIds, TaskTemplateId: indata.BillTemplateId },
                                    success: function (res) { //回调
                                        var param = { content: res.Result };
                                        CRT.Workbench.showPageDialog({
                                            id: 'Label_rpt',
                                            text: "单据打印".t(),
                                            url: '/Modules/PrintTemplate/DevPrint',
                                            params: param,
                                            method: 'POST'
                                        });
                                        win.close();
                                    }
                                });
                            }
                        }
                    }
                });
            }
        });
    }
});