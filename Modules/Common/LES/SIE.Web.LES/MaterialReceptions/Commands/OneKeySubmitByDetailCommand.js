SIE.defineCommand('SIE.Web.LES.MaterialReceptions.Commands.OneKeySubmitByDetailCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length < 1) {
            return false;
        }
        var current = view.getCurrent();
        if (!current || current.getState() !== 1)
            return false;
        return true;
    },
    execute: function (view) {
        var me = this;
        var storeData = view.getSelection();
        var viewModelList = [];
        storeData.forEach(item => {
            if (item.getState() !== 1) {
                return;
            }
            viewModelList.push(item.data);
        });
        var indata = {};
        indata.data = Ext.encode(viewModelList);
        this.view.execute({
            data: indata,
            success: function (res) {
                if (res.Success) {
                    var m = me;
                    view.reloadData();
                    SIE.Msg.showInstantMessage("提交成功".t());
                }
            }
        });
    },
});

