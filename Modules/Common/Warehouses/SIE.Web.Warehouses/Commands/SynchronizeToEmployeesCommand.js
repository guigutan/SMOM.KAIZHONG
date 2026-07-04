SIE.defineCommand('SIE.Web.Warehouses.Commands.SynchronizeToEmployeesCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "同步给员工", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var me = this;
        try {
            //var parentData = view._parent.getCurrent();
            if (view.getParent().getCurrent() == null) {
                return false;
            }
            return true;
        } catch (error) {
            return false
        }
    },

    execute: function (view) {
        var me = this;
        me.view = view;
        me.showView();
    },

    showView: function (view) {
        var me = this;
        var view = me.view;
        SIE.AutoUI.getMeta({
            model: "SIE.Resources.Employees.EmployeeSelect",
            ignoreCommands: false,
            ignoreQuery: false,
            isDetail: false,
            isAggt: true,
            viewGroup:"LookUpQueryView",
            callback: function (res) {
                var blocks = res;
                blocks.mainBlock.gridConfig.selModel = {
                    selType: 'checkboxmodel',
                    singleSelect: false, //是否单选
                    checkOnly: true, //只允许用户通过复选框选中
                    pruneRemoved: true //默认true，翻页保持勾选
                };
                //blocks.children[0].mainBlock.gridConfig.selModel = {
                //    selType: 'checkboxmodel',
                //    singleSelect: false, //是否单选
                //    checkOnly: true, //只允许用户通过复选框选中
                //    pruneRemoved: true //默认true，翻页保持勾选
                //};
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var listView = ui.getView();

                listView._relations[0]._target.tryExecuteQuery();
                //me.setSumQtyFun(listView);
                view.createAsnView = listView;
                var flag = true;
                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: '同步给员工'.t(),
                    items: items,
                    height: "60%",
                    width: "50%",
                    buttons: ['覆盖同步'.t(), '追加同步'.t()],
                    callback: function (btn) {
                        if (btn == "覆盖同步") {
                            var parentData = view.getParent().getCurrent().getData();
                            var selections = listView.getSelection();
                            if (selections.length == 0) {
                                SIE.Msg.showWarning("没有可提交的数据!".t());
                                return false;
                            }
                            var employeeIds = [];
                            SIE.each(selections, function (Item) {
                                var employeeId = Item.getData().Id;
                                employeeIds.push(employeeId);
                            });
                            view.execute({
                                data: { employeeId: parentData.Id, employeeIds: employeeIds,type:1},
                                success: function (res) {
                                    flag = true;
                                    view.reloadData();
                                    view.getParent().reloadData();
                                    SIE.Msg.hide();
                                    win.close();
                                },
                                error: function (r) {
                                    flag = true;
                                }

                            });
                        }
                        if (btn == "追加同步") {
                            //var selections = listView.getSelection();
                            //if (selections.length == 0) {
                            //    SIE.Msg.showWarning("没有可提交的数据!".t());
                            //    return false;
                            //}
                            //me.submitData(listView, 2);
                            var parentData = view.getParent().getCurrent().getData();
                            var selections = listView.getSelection();
                            if (selections.length == 0) {
                                SIE.Msg.showWarning("没有可提交的数据!".t());
                                return false;
                            }
                            var employeeIds = [];
                            SIE.each(selections, function (Item) {
                                var employeeId = Item.getData().Id;
                                employeeIds.push(employeeId);
                            });
                            view.execute({
                                data: { employeeId: parentData.Id, employeeIds: employeeIds, type: 2 },
                                success: function (res) {
                                    flag = true;
                                    view.reloadData();
                                    view.getParent().reloadData();
                                    SIE.Msg.hide();
                                    win.close();
                                },
                                error: function (r) {
                                    flag = true;
                                }

                            });
                        }
                    }
                });
            }
        });
    },

    submitData(listView,type) {
        var selections = listView.getSelection();
        if (selections.length == 0) {
            SIE.Msg.showWarning("没有可提交的数据!".t());
            return false;
        }
    }
    // end 
});