SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.TurnOverRuleDetailAddCommand', {
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
        this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
    },
    showView: function (editEntity) {
        var me = this;
        SIE.AutoUI.getMeta({
            isDetail: true,
            isAggt: true,
            token: editEntity.token,
            model: this.view.model,
            async: false,
            ignoreQuery: false,
            viewGroup: "TurnOverRuleDetailGroup",
            callback: function (res) {
                var view = SIE.AutoUI.generateAggtControl(res);
                view._view.setCurrent(editEntity);
                view._view.getChildren()[0].reloadData();
                var ui = view.getControl();
                var win = SIE.Window.show({
                    title: "添加明细".t(),
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
    onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property == 'OrderType') {
                e.entity.setTransactionId(null);
            }
        }
    },
});