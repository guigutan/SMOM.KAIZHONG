SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.WorkOrderCompletionCommand', {
    //extend: 'SIE.cmd.Save',
    meta: { text: "手动完工", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (listView) {

        var selectModels = listView.getCurrent();
        if (selectModels == null) return false;
        else {
            if (selectModels.getFinishQty() >= selectModels.getPlanQty()) {
                return true;
            }
            else
                return false;
        }
        return true;
    },
    execute: function (view) {
        view.execute({
            data: view.getCurrent().getData(),
            success: function (res) {
                debugger;
                if (res.Success)
                    alert("该工单已经完工！");
            }
        });
    }
});