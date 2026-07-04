SIE.defineCommand('SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfCancelCommand', {
    meta: { text: "作废", group: "business", iconCls: "icon-Cut icon-blue" },

    canExecute: function (view) {
        if (view && view.getSelection().length > 0 && view.getSelection().all(p => p.getIsCancel() == false || p.getIsCancel() == null)) {
            return true;
        }
        return false;
    },

    execute: function (view) {

        var id = view.getSelectionIds().join(',');
        var me = this;

        SIE.AutoUI.getMeta({
            model: "SIE.MES.TaskManagement.SchedulingInfs.ViewModels.SchedulingInfCancelViewModel",
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
                entity.setIds(id);
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "作废原因".t(),
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
    },

});