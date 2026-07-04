SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageAddExpCommand', {
    meta: { text: "加入经验库", group: "edit", iconCls: "icon-Export icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getCurrent();
        if (selectModels == null)
            return false;
        var res = true;
        if (selectModels.getExperienceFlag() == true)
            res = false;
        if (selectModels.getState() == 50)
            res = false;
        return res;
    },
    execute: function (view, source) {
        var current = view.getCurrent()
        SIE.Msg.askQuestion('是否加入经验库？'.t(), function () {
            view.execute({
                data: current.data,
                success: function (res) {
                    if (res.Success) {
                        view._parent.reloadData();
                    }
                }
            });
        });
    }
});
