SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.SyncAllPrepareQtyCommand", {
    meta: { text: "同步备料数", group: "edit", iconCls: "icon-EditAdd icon-green" },
    execute: function (view) {
        var dataList = view.getData().data.items;
        if (dataList.length <= 0) {
            return;
        }
        else {
            dataList.forEach(function (i) {
                i.setQty(i.getCanPrepareQty());
            })
        }
    }
})