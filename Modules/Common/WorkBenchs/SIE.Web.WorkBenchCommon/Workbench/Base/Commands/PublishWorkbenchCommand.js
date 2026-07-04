SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.PublishWorkbenchCommand', {
    meta: { text: "发布", group: "edit", hierarchy: '工作台发布'.t(), iconCls: "iconfont icon-AlignTop icon-blue" },

    canExecute: function (view) {
        return view.getCurrent() != null;
    },
    execute: function (view, source) {
        var me = this;
        var data = view.getCurrent().data;

        view.execute({
            data: { Code: data.Code },
            success: function (res) {
                SIE.Msg.showInstantMessage("发布成功".L10N())
            }
        }, view);

    },
});