SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.AddOnShelvesRuleDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var rule = view.getParent();
        if (rule == null) return false;
        var ruleCur = view.getParent().getCurrent();
        if (ruleCur == null) return false;
        if (ruleCur != null) {
            if (ruleCur.isNew() || ruleCur.data.WarehouseId <= 0) return false;
        }

        return true;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var rule = this.view.getParent().getCurrent().data;
            var model = entity.data;
            var me = this;
            var lineNo = me.view.getData().count() + 1;
            var ct = me.view.getData().count();
            var tempLineNo = 0;
            if (ct > 0) {
                tempLineNo = parseInt(me.view.getData().data.items[0].data.LineNo);
                for (var i = 1; i < me.view.getData().data.items.length; i++) {
                    var curLineNo = parseInt(me.view.getData().data.items[i].data.LineNo);
                    curLineNo > tempLineNo ? tempLineNo = curLineNo : tempLineNo
                }
            }
            if (tempLineNo > ct) {
                lineNo = tempLineNo + 1;
            }
            entity.setWarehouseId(rule.WarehouseId);
            entity.setWarehouseId_Display(rule.WarehouseName);
            entity.setLineNo(lineNo);
            this.mon(entity, 'propertyChanged', SIE.Web.Inventory.Strategy.OnShelvesRuleDetailAction.onEntityPropertyChanged, this);
        }
    },
    showView: function (entity) {
        var me = this;
        var meta = null;
        SIE.AutoUI.getMeta({
            isDetail: true,
            ignoreCommands: false,
            ignoreQuery: false,
            isAggt: true,
            token: entity.token,
            model: this.view.model,
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                detailView._view.setCurrent(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: me.getEditViewTitle(entity),
                    width: '75%',
                    height: '70%',
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            entity = detailView._view.getCurrent();
                            me.view.execute({
                                data: entity.getData(),
                                command: "SIE.Web.Inventory.Strategy.Commands.ValidOnShelvesRuleDtlCommand",
                                success: function (res) {
                                    var isImmediate = me.view.isImmediate();
                                    me.view.afterEdit(entity, isImmediate, me.isCopy);
                                    me.confirm(entity, isImmediate, me.isCopy);
                                    win.close();
                                }
                            });
                            return false;
                        }
                    }
                });
            }
        });
    },
});