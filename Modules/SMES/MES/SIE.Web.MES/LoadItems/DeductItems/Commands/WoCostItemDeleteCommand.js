SIE.defineCommand("SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemDeleteCommand", {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.State != 10) //待提交10才可删除
                res = false;
        });
        return res;
    }
});