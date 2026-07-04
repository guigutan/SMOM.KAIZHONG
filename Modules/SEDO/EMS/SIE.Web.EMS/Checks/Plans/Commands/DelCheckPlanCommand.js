SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.DelCheckPlanCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-green" },
    /**
    * 是否执行删除
    * @param {*} view 视图
    */
    canExecute: function (view) {
        return view.getSelection().length > 0;
    },

    /**
    * 执行删除
    * @param {*} view 视图
    * @param {*} source 源
    */
    execute: function (view, source) {
        Ext.MessageBox.confirm("提示".t(), "删除该点检任务同时删除往后的点检任务，是否执行？".t(), function (btnId) {
            if (btnId == "yes") {
                Ext.each(view.getSelection(), function (sel) {
                    var selDateYear = sel.getCheckDate().getYear();
                    var selDateMonth = sel.getCheckDate().getMonth();
                    var selDateDay = sel.getCheckDate().getDate();

                    var entities = [];
                    for (var index = 0; index < view.getData().getData().items.length; index++) {
                        var data = view.getData().getData().items[index];
                        if (data.getCheckDate().getYear() > selDateYear ||
                            (data.getCheckDate().getYear() >= selDateYear && data.getCheckDate().getMonth() > selDateMonth) ||
                            (data.getCheckDate().getYear() >= selDateYear &&
                                data.getCheckDate().getMonth() >= selDateMonth &&
                                data.getCheckDate().getDate() >= selDateDay)) {
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
