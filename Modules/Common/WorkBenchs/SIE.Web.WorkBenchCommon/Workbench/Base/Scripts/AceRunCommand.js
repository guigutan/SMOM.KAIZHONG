Ext.define('SIE.Web.WorkBenchCommon.Workbench.Base.Scripts.LayoutRunCommand', {
    Run: function (codeContent) {
        var param = { content: Ext.util.Base64.encode(codeContent) };
        CRT.Workbench.showPageDialog({
            id: 'layoutPreview',
            text: "布局预览",
            url: '/WorkBench/LayoutPreview',
            params: param,
            method: 'POST',
            view: null
        });
    }
});

Ext.define('SIE.Web.WorkBenchCommon.Workbench.Base.Scripts.ComponentRunCommand', {
    Run: function (codeContent) {
        var param = { content: Ext.util.Base64.encode(codeContent) };
        CRT.Workbench.showPageDialog({
            id: 'layoutPreview',
            text: "组件预览",
            url: '/WorkBench/RunComponentPreview',
            params: param,
            method: 'POST',
            view: null
        });
    }
});