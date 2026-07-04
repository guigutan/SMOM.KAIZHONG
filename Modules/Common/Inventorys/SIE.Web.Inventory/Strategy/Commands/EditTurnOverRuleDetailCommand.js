SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.EditTurnOverRuleDetailCommand', {
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
            this.mon(entity, 'propertyChanged', SIE.Web.Inventory.TurnOverRuleDetailAction.onEntityPropertyChanged, this);
        }
    },
    showView: function (entity) {
        var me = this;
        var meta = null;
        SIE.AutoUI.getMeta({
            model: this.view.model,
            isDetail: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: Ext.String.format("修改-明细[行号{0}]".t(), entity.data.LineNo),
                    width: '75%',
                    height: '90%',
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = entity.data;
                            me.view.execute({
                                data: indata,
                                command: "SIE.Web.Inventory.Strategy.Commands.ValidTurnOverRuleDetailCommand",
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