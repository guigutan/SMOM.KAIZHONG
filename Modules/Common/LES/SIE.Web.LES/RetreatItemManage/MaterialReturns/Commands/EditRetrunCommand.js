SIE.defineCommand("SIE.Web.LES.RetreatItemManage.MaterialReturns.EditRetrunCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var selectedItems = view.getSelection();
        if (selectedItems.length === 0) {
            return false;
        }
        var res = true;
        SIE.each(selectedItems, function (model) {
            //只有单据来源待提交可以
            if ((model.data.ReturnState != 10)
            ) {
                res = false;
                return false;
            }
        });
        return res;
    },
});