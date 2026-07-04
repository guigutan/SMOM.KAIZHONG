SIE.defineCommand('SIE.Web.EMS.Checks.Commands.DelMaintainPlanCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        return view.getSelection().length > 0;
    },
    execute: function (view, source) {
        Ext.MessageBox.confirm("提示".t(), "删除该保养任务同时删除往后的保养任务，是否执行？".t(), function (btnId) {
            if (btnId == "yes") {
                Ext.each(view.getSelection(), function (sel) {
                    var selDateYear = sel.getPlanBeginDate().getYear();
                    var selDateMonth = sel.getPlanBeginDate().getMonth();

                    var entities = [];
                    for (var index = 0; index < view.getData().getData().items.length; index++) {
                        var data = view.getData().getData().items[index];
                        var planBeginDate = data.getPlanBeginDate();
                        if (planBeginDate.getYear() > selDateYear || (planBeginDate.getYear() >= selDateYear && planBeginDate.getMonth() >= selDateMonth)) {
                            entities.push(data);
                        }
                    }
                    for (var index in entities) {
                        view.getData().remove(entities[index]);
                    }
                });
            }
        });
    }
});
