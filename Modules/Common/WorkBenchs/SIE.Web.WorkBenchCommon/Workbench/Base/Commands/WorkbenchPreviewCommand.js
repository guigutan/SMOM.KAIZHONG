
SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.WorkbenchPreviewCommand', {
    meta: { text: "预览", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },

    canExecute: function (view) {
        return view.getCurrent() != null;
    },
    execute: function (listView, source) {
        var me = this;
        var data = listView.getCurrent().data;
        var param = { Code: data.Code};
        listView.execute({
            data: { LayoutCode: data.LayoutCode },
            success: function (res) {
                CRT.Workbench.showPageDialog({
                    id: 'workbenchPreview',
                    text: "工作台预览".L10N(),
                    url: '/WorkBench/ViewWorkBench',
                    params: param,
                    method: 'POST',
                    view: null
                });
            }
        }, listView);

    },
});