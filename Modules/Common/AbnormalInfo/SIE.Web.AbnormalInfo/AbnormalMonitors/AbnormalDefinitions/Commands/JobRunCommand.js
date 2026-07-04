SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands.JobRunCommand', {
    meta: { text: "运行", group: "business", iconCls: "icon-Play icon-green", tooltip: "立即执行任务", },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var current = view.getCurrent();
        if (!current || current.isNew())
            return false
        return true;
    },
    execute: function (view, source) {
        var current = view.getCurrent();
        SIE.Msg.askQuestion(Ext.String.format('确认立刻执行当前定义[{0}]?'.t(), current.getCode()), function () {
            view.execute({
                withIds: true,
                selectIds: [current.getId()],
                success: function (res) {
                    view.reloadData();
                    SIE.Msg.showMessage(Ext.String.format('定义[{0}]已开始执行'.t(), current.getCode()));
                }
            });
        });
    }
})