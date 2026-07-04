SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.AddTurnOverRuleDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var rule = view.getParent();
        if (rule == null) return false;
        var ruleCur = view.getParent().getCurrent();
        if (ruleCur == null) return false;
        if (ruleCur != null) {
            if (ruleCur.isNew() || ruleCur.data.IsDefault === true) return false;
        }

        return true;
    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var data = res.Result;
                var lineNo = me.view.getData().count() + 1;
                if (me.view.getData().count() > 1) {
                    var tempLineNoList = me.view.getData().getData().items.where(function (p) { return p.getLineNo() != null; }).select(function (p) { return p.getLineNo(); });
                    lineNo = tempLineNoList.max() + 1;
                }

                entity.setLineNo(lineNo);
                entity.setSortField1(null);
                entity.setSortField2(null);
                entity.setSortField3(null);
                entity.setSortField4(null);
                entity.setSortField5(null);
                entity.setFieldType1(null);
                entity.setFieldType2(null);
                entity.setFieldType3(null);
                entity.setFieldType4(null);
                entity.setFieldType5(null);
                entity.setSortType1(null);
                entity.setSortType2(null);
                entity.setSortType3(null);
                entity.setSortType4(null);
                entity.setSortType5(null);
            }
        }, me.view);

        this.mon(entity, 'propertyChanged', SIE.Web.Inventory.TurnOverRuleDetailAction.onEntityPropertyChanged, this);
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
                    title: me.getEditViewTitle(entity),
                    width: '75%',
                    height: '70%',
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