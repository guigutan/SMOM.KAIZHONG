SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemEnableCommand', {
    meta: { text: "启用", group: "business", iconCls: "icon-NetworkNormal icon-green" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.State != 0)//1:启用状态 0:禁用状态
                res = false;
        });
        return res;
    },
    execute: function (listView, source) {
        SIE.Msg.askQuestion('确定启用选中的资料?'.t(), function () {
            listView.execute({
                data: listView.getSelectionIds(),
                success: function (res) {
                    SIE.each(listView.getSelection(), function (model) {
                        model.data.State = 1;
                    });
                    listView.reloadData();
                }
            });

        }, function () { }
        );
    }
});