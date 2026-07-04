SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.TurnOverRuleDetailEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }

        var rule = view.getParent();
        if (rule == null) return false;
        var ruleCur = view.getParent().getCurrent();
        if (ruleCur == null) return false;
        if (ruleCur != null)
            if (ruleCur.data.IsDefault === true) return false;

        return true;
    },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property == 'OrderType') {
                e.entity.setTransactionId(null);
            }
        }
    },
    showView: function (editEntity) {
        var me = this;
        SIE.AutoUI.getMeta({
            isDetail: true,
            ignoreCommands: false,
            ignoreQuery: false,
            isAggt: true,
            token: editEntity.token,
            model: this.view.model,
            viewGroup: "TurnOverRuleDetailGroup",
            callback: function (res) {
                var view = SIE.AutoUI.generateAggtControl(res);
                view._view.setCurrent(editEntity);
                var ui = view.getControl();
                var win = SIE.Window.show({
                    title: "编辑明细".t(),
                    height: document.body.clientHeight * 0.8,
                    width: document.body.clientWidth * 0.8,
                    buttons: ['确定'.t(), '取消'.t()],
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var data1 = view._view.getCurrent().getData();
                            var data2 = view._view.getChildren()[0].getControl().getStore().data.items.select(p => p.data);
                            var result = true;
                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Inventory.Strategy.TurnOverRuleDetailSortRuleViewModelDataQuery",
                                method: 'SaveDatas',
                                token: me.view.token,
                                params: [data1, data2],
                                callback: function (res) {
                                    result = res.Success;
                                }
                            });
                            if (result) {
                                me.view.reloadData();
                                win.close();
                            }
                            else {
                                return result;
                            }
                        }
                    }
                });
            }
        });
    },

});