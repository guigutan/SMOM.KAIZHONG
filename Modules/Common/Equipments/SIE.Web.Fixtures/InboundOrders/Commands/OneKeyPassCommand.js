SIE.defineCommand('SIE.Web.Fixtures.InboundOrders.Commands.OneKeyPassCommand', {
    meta: { text: "一键设置库位", group: "business", iconCls: "icon-Check icon-blue", tooltip: "请先设置第一行数据的库位后点击", },
    canExecute: function (view) {
        var currentData = view.getParent().getCurrent();
        if (currentData == null) return false;
        var res = true;
        if (currentData.data.InboundStatus !== 20&&currentData.data.InboundStatus !== 10) {
            res = false;
            return false;
        }
        return res;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion('确定将未设置库位的数据全部录入相同库位？'.L10N(), function () {
            debugger;
            var stroageId = view.getData().getData().items[0].data.StorageLocationId;
            var stroageId_display = view.getData().getData().items[0].data.StorageLocationId_Display;
            if (stroageId == 0 || stroageId == undefined) {
                SIE.Msg.showMessage("一键设置库位失败，请先设置第一行数据的库位！".t());
                return false;
            }
            view.getData().getData().items.forEach(item => {
                item.setStorageLocationId_Display(stroageId_display);
                item.setStorageLocationId(stroageId);
            });
            SIE.Msg.showMessage("设置完成!".t());
            view.getParent().reloadData();
        });
    }
});