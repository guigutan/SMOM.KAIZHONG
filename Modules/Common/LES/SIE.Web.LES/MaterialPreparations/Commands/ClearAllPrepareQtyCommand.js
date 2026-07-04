SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.ClearAllPrepareQtyCommand", {
    meta: { text: "清空备料数", group: "edit", iconCls: "icon-EditMinus icon-red" },
    execute: function (view) {
        var dataList = view.getData().data.items;
        if (dataList.length <= 0) {
            return;
        }
        else {
            dataList.forEach(function (i) {
                i.setQty(0);
            })
        }
    }
})