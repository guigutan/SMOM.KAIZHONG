SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.SchedulingInfReturnCommand', {
    meta: { text: "排程退回", group: "edit", iconCls: "icon-Reload icon-blue" },
    canExecute: function (view) {
        //if (view && view.getCurrent() && view.getCurrent().getSourceType() == 2 && view.getCurrent().getTaskStatus() != 50)
        if (view && view.getSelection().length > 0 && view.getSelection().all(p => p.getSourceType() == 2 && p.getTaskStatus() != 50 && (p.getIsSchedulingInfReturn() == 0 || p.getIsSchedulingInfReturn() == null || p.getIsSchedulingInfReturn() == '')))
            return true;
        return false;
        //var selecteditems = view.getSelection();
        //if (selecteditems != null && selecteditems.length > 0) {
        //    for (var i = 0; i < selecteditems.length; i++) {
        //        if (selecteditems[i].data.TaskStatus !== 40) {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        //return false;
    },
    execute: function (view, source) {

        var me = this;
        var result = "";
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
            method: "SchedulingInfReturnValid",
            params: [view.getSelectionIds()],
            async: false,
            token: view.token,
            callback: function (res) {
                result = res.Result;
            }
        });

        if (result != null && result != "") {
            SIE.Msg.showError(result);
            return;
        }

        var id = view.getSelectionIds().join(',');

        SIE.AutoUI.getMeta({
            model: "SIE.MES.TaskManagement.Dispatchs.ViewModels.SchedulingInfReturnValidViewModel",
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
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "排程退回原因".t(),
                    width: 400,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var currentView = detailView;

                            me.view.execute({
                                data: currentView.getCurrent().getData(),
                                success: function (res) { //回调
                                    view.reloadData();
                                }
                            });
                        }
                    }
                });
            }
        });

    }
});