
SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.ComponentPreviewCommand', {
    meta: { text: "预览", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getCurrent() == null || view.getCurrent().data.Content == null || view.getCurrent().data.Content == '') {
            return false;
        }
        return true;
    },
    execute: function (listView, source) {
        var me = this;
        var code = listView.getCurrent().data.Code;
        var param = { Code: code };

        CRT.Workbench.showPageDialog({
            id: 'componentPreview',
            text: "组件预览".L10N(),
            url: '/WorkBench/ComponentPreview',
            params: param,
            method: 'POST',
            view: null
        });
       
    },
});