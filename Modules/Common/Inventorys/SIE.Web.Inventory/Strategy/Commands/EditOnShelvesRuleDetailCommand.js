SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.EditOnShelvesRuleDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        var me = this;
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.Inventory.Strategy.OnShelvesRuleDetailAction.onEntityPropertyChanged, this);
        }
    },
    showView: function (editEntity) {
        var me = this;
        var meta = null;
        SIE.AutoUI.getMeta({
            isDetail: true,
            ignoreCommands: false,
            ignoreQuery: false,
            isAggt: true,
            token: editEntity.token,
            model: this.view.model,
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                detailView._view.setCurrent(editEntity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: Ext.String.format("修改-明细[行号{0}]".t(), editEntity.data.LineNo),
                    width: '75%',
                    height: '90%',
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            editEntity = detailView._view.getCurrent();                           
                            me.view.execute({
                                data: editEntity.getData(),
                                command: "SIE.Web.Inventory.Strategy.Commands.ValidOnShelvesRuleDtlCommand",
                                success: function (res) {
                                    var isImmediate = me.view.isImmediate();
                                    me.view.afterEdit(editEntity, isImmediate, me.isCopy);
                                    me.confirm(editEntity, isImmediate, me.isCopy);
                                    win.close();
                                }
                            });
                            return false;
                        }
                        if (btn == "取消".t()) {
                            editEntity.reject();
                        }
                    }
                });
            }
        });
    },
});