SIE.defineCommand('SIE.Web.LES.MaterialReceptions.Commands.SubmitByDetailCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (entity) {
        if (entity.getData().data.items.length <= 0) {
            return false;
        }
        return true;
    },
    execute: function (entity) {
        var me = this;
        var storeData = entity.getData().data.items;
        var viewModelList = [];
        storeData.forEach(item => {
            if (item.data.Qty === 0) {
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
                    m.view.getData().removeAll()
                    SIE.Msg.showInstantMessage("提交成功".t());
                }
            }
        });
    },
});

