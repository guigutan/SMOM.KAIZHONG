SIE.defineCommand("SIE.Web.ProductIntfc.InspLogs.Commands.InspLogExamineCommand", {
    meta: { text: "审核", group: "edit", iconCls: "icon-Checkmark icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getInspectionStatus() == 2) {
            return false;
        }
        if (entity.getIsCall()) {
            return false;
        }
        return true;
    },

    execute: function (view, source) {
        var entity = view.getCurrent();
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "ExamineViewStr",
            token: this.view.token,
            model: "SIE.ProductIntfc.InspLogs.InspLog",
            module:"SIE.ProductIntfc.ProductInsps.ProductInsp,SIE.ProductIntfc",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "审核".t(),
                    width: 740,
                    height: 300,
                    items: ui,
                    buttons: [],
                    listeners: {
                    }
                });
            }
        });
    }
});
