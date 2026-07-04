SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendCancelCommand", {
    meta: { text: "取消", group: "edit", iconCls: "icon-Cancel icon-red" },
    execute: function (view, source) {
        view.getCurrent().markSaved();
        CRT.Workbench.closeCurrentTab();
    },
})


