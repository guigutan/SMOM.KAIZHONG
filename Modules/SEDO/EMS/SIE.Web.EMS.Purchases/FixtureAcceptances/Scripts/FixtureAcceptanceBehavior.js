Ext.define('SIE.Web.EMS.Purchases.FixtureAcceptances.FixtureAcceptanceBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        view.snView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureAcceptances.FixtureAcceptanceSn"; });
        view.mon(view, 'currentChanged', me.currentChanged, view);

        tabActiveIndex = 0;
        var selectionModel = view.getControl().getSelectionModel();
        if (selectionModel) {

            selectionModel.mon(selectionModel, "selectionchange", function (selmodel, selection) {
                if (selection.length == 1) {
                    var isReadOnly = (selection[0].data.ApprovalStatus !== 10 &&selection[0].data.ApprovalStatus !== 50);
                    tabActiveIndex = view._children[0].getControl().ownerCt.ownerCt.getActiveTab();
                    view._children[0].getControl().ownerCt.ownerCt.setActiveTab(1);
                    view._children[0].getControl().ownerCt.ownerCt.setActiveTab(2);
                    view._children[0].getControl().ownerCt.ownerCt.setActiveTab(3);
                    view._children[0].getControl().ownerCt.ownerCt.setActiveTab(tabActiveIndex);
                    view.getChildren().forEach(function (children) {
                        children.setIsReadonly(isReadOnly);//设置子节点文本域只读//设置按钮禁用
                        var children_btns = children.getControl().getView().grid.query('button');
                        if (isReadOnly) {
                            children_btns.forEach(function (btn) {
                                btn.disable();
                            });
                        }

                        var tabpanel = children.getControl().up('tabpanel');
                        if (tabpanel) {
                            tabpanel.mon(tabpanel, 'tabchange', function () {
                                var approvalStatus = children.getParent().getSelection()[0].getData().ApprovalStatus;
                                var parentReadOnly = (approvalStatus !== 50 && approvalStatus!==10);
                                children.setIsReadonly(isReadOnly);//设置子节点文本域只读//设置按钮禁用
                                children_btns.forEach(function (btn) {
                                    if (parentReadOnly) {
                                        btn.disable();
                                    }
                                });
                            });
                        }
                    });
                }
            }, this);
        }
    },
    currentChanged: function (config) {
        var me = this;
        var curEntity = me.getCurrent();
        if (!curEntity) {
            return;
        }
        if (me.snView) {
            let tabPanel = me.snView.getControl().ownerCt.ownerCt;
            if (tabPanel.getActiveTab().title.indexOf('序列'.t()) > -1)
                tabPanel.setActiveTab(0);
            if (curEntity.data.ManageMode === 5) {
                me.snView._control.ownerLayout.config.owner.tab.setVisible(true);
            }
            else {
                me.snView._control.ownerLayout.config.owner.tab.setVisible(false);
            }
        }
    }
});