SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.UnPublishWorkbenchCommand', {
    meta: { text: "取消发布", group: "edit", hierarchy: '工作台发布'.t(), iconCls: "iconfont icon-AlignBottom icon-green" },

    canExecute: function (view) {
        return view.getCurrent() != null;
    },
    execute: function (view, source) {
        var me = this;
        var data = view.getCurrent().data;

        view.execute({
            data: data.Code ,
            success: function (res) {
                SIE.Msg.showInstantMessage("取消发布成功".L10N())
            }
        }, view);

    },
});