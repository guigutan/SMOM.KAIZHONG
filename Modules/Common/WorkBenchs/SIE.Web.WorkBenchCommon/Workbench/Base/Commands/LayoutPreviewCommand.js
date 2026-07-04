
SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.Base.Commands.LayoutPreviewCommand', {
    meta: { text: "预览", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() == null || view.getCurrent().data.Content == null || view.getCurrent().data.Content == '') {
            return false;
        }
        return true;
    },
    execute: function (listView, source) {
        var me = this;
        var codeContent = listView.getCurrent().data.Content;
       
            var param = { content: Ext.util.Base64.encode(codeContent) };
            CRT.Workbench.showPageDialog({
                id: 'layoutPreview',
                text: "布局预览".L10N(),
                url: '/WorkBench/LayoutPreview',
                params: param,
                method: 'POST',
                view: null
            });
    }
});