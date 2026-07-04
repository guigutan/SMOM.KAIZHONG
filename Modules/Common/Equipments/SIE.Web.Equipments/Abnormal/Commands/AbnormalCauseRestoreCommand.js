SIE.defineCommand('SIE.Web.Equipments.Abnormal.Commands.AbnormalCauseRestoreCommand', {
    meta: { text: "恢复", group: "edit", iconCls: "icon-Submit icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        return current &&
            current.getExceptionStopType() !== 0;
    },
    /**
     * 执行
     * @param {any} view
     */
    execute: function (view) {
        var current = view.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.Web.Equipments.Abnormal.ViewModels.RestoreReasonViewModel",
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
                entity.setAbnormalCauseId(current.data.Id);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "停线恢复".t(),
                    width: 500,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.Reason === null) {
                                SIE.Msg.showError("恢复原因不能为空!".t());
                                return false;
                            }
                            view.execute({
                                data: indata,
                                success: function (res) {
                                    win.close();
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