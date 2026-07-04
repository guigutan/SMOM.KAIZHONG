SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.CancelReturnCommand", {
    meta: { text: "取消", group: "edit", iconCls: "icon-Cancel icon-red" },
    execute: function (view, source) {
        view.getCurrent().markSaved();
        CRT.Workbench.closeCurrentTab();
    },
})