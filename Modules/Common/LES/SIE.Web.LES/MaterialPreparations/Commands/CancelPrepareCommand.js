SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.CancelPrepareCommand", {
    meta: { text: "取消", group: "edit", iconCls: "icon-Cancel icon-red" },
    execute: function (view, source) {
        view.getCurrent().markSaved();
        CRT.Workbench.closeCurrentTab();
    },
})