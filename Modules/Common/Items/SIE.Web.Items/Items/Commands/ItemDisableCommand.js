SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemDisableCommand', {
    meta: { text: "禁用", group: "business", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.State != 1 || model.isNew())//1:启用状态 0:禁用状态
            {
                res = false;
            }
        });
        return res;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion('确定禁用选中的资料?'.t(), function () {
            listView.execute({
                data: listView.getSelectionIds(),
                success: function (res) {
                    SIE.each(listView.getSelection(), function (model) {
                        model.data.State = 0;
                    });
                    listView.reloadData();
                }
            });

        }, function () { }
        );
    }
});