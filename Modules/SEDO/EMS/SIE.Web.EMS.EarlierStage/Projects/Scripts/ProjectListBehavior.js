Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectListBehavior', {
    /**
      * view生命周期函数--view准备完成
      * @param {ListLogicView} view 生成的view
      */
    onViewReady: function (view) {
        var me = this;
        me.view = view;
        var grid = view.getControl().SIEView.getControl().ownerGrid;
        grid.mon(grid, 'rowclick', me.rowclick, me);

    },
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        var mainMe = this;
        var me = this.view;
        var isReadOnly = (record.data.ApprovalStatus == 20 || record.data.ApprovalStatus == 30 || record.data.ApprovalStatus == 40);

        me.getChildren().forEach(function (children) {
           //设置按钮禁用
            if (children.model != "SIE.EMS.InventoryPlans.InventoryPlanEquipment") {
                var children_btns = children.getControl().getView().grid.query('button');
                if (isReadOnly) {

                    children_btns.forEach(function (btn) {
                        mainMe.btndisable(btn, isReadOnly);
                    });
                }
                var selectionModel_child = children.getControl().getSelectionModel();
                if (selectionModel_child) {
                    selectionModel_child.mon(selectionModel_child, "selectionchange", function (selmodel, selection) {
                        var parentCurrentEntity = children.getParent().getCurrent();
                        var parentReadOnly = true;

                        if (parentCurrentEntity) {
                            var selectData = parentCurrentEntity.data;
                            parentReadOnly = (selectData.ApprovalStatus == 20 || selectData.ApprovalStatus == 30 || selectData.ApprovalStatus == 40);
                        }
                        
                        var selbtns = children.getParent().getControl().SIEView.getControl().ownerGrid.query('button');
                        if (parentReadOnly) {
                            selbtns.forEach(function (btn) {
                                mainMe.btndisable(btn, parentReadOnly);
                            });
                        }
                        children_btns.forEach(function (btn) {
                            if (parentReadOnly) {
                                mainMe.btndisable(btn, parentReadOnly);
                            }
                        });
                    });
                }

            } else {
                var form = children.getControl();////dataChanged"
                form.mon(form.SIEView, "currentChanged", function () {
                    setTimeout(function () {
                        var parentCurrentEntity = children.getParent().getCurrent();
                        var parentReadOnly = true;

                        if (parentCurrentEntity) {
                            var selectData = parentCurrentEntity.data;
                            parentReadOnly = (selectData.ApprovalStatus == 20 || selectData.ApprovalStatus == 30 || selectData.ApprovalStatus == 40);
                        }

                        var selbtns = children.getParent().getControl().SIEView.getControl().ownerGrid.query('button');
                        if (parentReadOnly) {
                            selbtns.forEach(function (btn) {
                                mainMe.btndisable(btn, parentReadOnly);
                            });
                        }
                    }, 50);
                }, me);
            }
            var tabpanel = children.getControl().up('tabpanel');
            if (tabpanel) {
                tabpanel.mon(tabpanel, 'tabchange', function () {
                    var parentCurrentEntity = children.getParent().getCurrent();
                    var parentReadOnly = true;

                    if (parentCurrentEntity) {
                        var selectData = parentCurrentEntity.data;
                        parentReadOnly = (selectData.ApprovalStatus == 20 || selectData.ApprovalStatus == 30 || selectData.ApprovalStatus == 40);
                    }

                    //设置按钮禁用
                    if (children_btns != null) {
                        children_btns.forEach(function (btn) {
                            if (parentReadOnly) {
                                mainMe.btndisable(btn, parentReadOnly);
                            }
                        });
                    }

                    var selbtns = children.getParent().getControl().SIEView.getControl().ownerGrid.query('button');
                    if (parentReadOnly) {
                        selbtns.forEach(function (btn) {
                            mainMe.btndisable(btn, parentReadOnly);
                        });
                    }
                });
            }
        });

    },
    btndisable: function (btn, isReadOnly) {
        if (btn.tooltip == "导入") {
            if (isReadOnly) {
                btn.disable();
            }
        }
    }
});

