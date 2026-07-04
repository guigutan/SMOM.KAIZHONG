SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.EnablePortalCommand', {
    meta: { text: "启用门户", group: "edit", iconCls: "icon-Play icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        if (listview.getData().isDirty()) return false;
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.IsPortal === true) {
                return false;
            }
        }
        return true;
    },
    execute: function (listview, source) {
        var me = this;
        SIE.Msg.askQuestion(Ext.String.format('确定启用选中的{0}条资料？'.t(), this.selectedItems.length),
            function () {
                //减少网络传输，此自定义命令只需要传选中的ID
                var selectIds = listview.getSelectionIds(this.selectedItems);
                listview.execute({
                    withIds: true,
                    selectIds: selectIds,
                    success: function (res) { //回调
                        listview.reloadData();
                        if (res.Result.length > 0) {
                            me.showView(res.Result);
                        }
                    }
                });
            });
    },
    showView(data) {
        var me = this;
        var view = me.view;
        SIE.AutoUI.getMeta({
            model: "SIE.CSM.Suppliers.ViewModels.SupplierPswViewModel",
            ignoreQuery: true,
            callback: function (res) {
                var blocks = res;
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var listView = ui.getView();
                view.createAsnView = listView;
                //主页面
                var View = ui._view;
                var newStore = Ext.create('Ext.data.Store', {
                    model: "SIE.CSM.Suppliers.ViewModels.SupplierPswViewModel",
                    data: data
                });
                View.getControl().setStore(newStore);
                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: '已启用随机密码，密码如下'.t(),
                    items: items,
                    height: 300,
                    width: 400,
                    buttons: ['确定'.t()],
                    callback: function (main) {

                    }
                });
            }
        })
    }
});