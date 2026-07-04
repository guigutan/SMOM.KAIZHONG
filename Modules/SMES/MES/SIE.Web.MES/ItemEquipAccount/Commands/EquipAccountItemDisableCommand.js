SIE.defineCommand('SIE.Web.MES.ItemEquipAccount.Commands.EquipAccountItemDisableCommand', {
    meta: { text: "禁用", group: "business", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.State == 0)
                res = false;
        });
        return res;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion('确认禁用选中的检具与产品的关系类型？'.t(), function () {
            listView.execute({
                data: listView.getSelectionIds(),
                success: function (res) {
                    SIE.each(listView.getSelection(), function (model) {
                        model.data.State = 0;
                    });
                    listView.reloadData();
                }
            });
        });
    }
});
