SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddStaffCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加员工", group: "edit", iconCls: "icon-AddEntity icon-green" },
    showView: function (editEntity) {
        var me = this;
        var row_data = me.view.getParent().getCurrent();
        var token = me.view.getParent().token;
        var andonTypeId = row_data.getId();
        var addAlter = [];
        var deletAlter = [];
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                model: "SIE.Resources.Employees.Employee",
                ignoreCommands: true,
                isDetail: false,
                ignoreQuery: true,
                viewGroup: "SelectEmployeeView",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;

                    mainBlock.storeConfig.pageSize = 9999999;
                    //右视图
                    var rightView = SIE.AutoUI.createListView(mainBlock);
                    var rightui = rightView.getControl();
                    rightui.flex = 1;

                    SIE.invokeDataQuery({
                        type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeTriggerPowerDataQuery",
                        method: "GetEmployeeAlternative",
                        params: [andonTypeId, null],
                        async: false,
                        token: token,
                        callback: function (res) {
                            if (res.Success && res.Result) {
                                res.Result.each(function (item) {
                                    var entity = rightView.createNewItem();
                                    entity.set('Code', item.data.ObjectCode);
                                    entity.set('Name', item.data.ObjectName);
                                    entity.markSaved();
                                });
                            }
                        }
                    });
                    //左视图
                    mainBlock.gridConfig.tbar = [{
                        name: 'employee_searchTxt',
                        emptyText: '编码或名称'.t(),
                        xtype: 'textfield',
                        listeners: {
                            //回车按钮事件
                            specialKey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    var store = this.up("panel").getStore();
                                    var itemView = this.up("panel").SIEView;
                                    var searchTxt = this.ownerCt.child("[name = employee_searchTxt]");
                                    var Filter = {
                                        Method: "GetEmployeeAll",
                                        Parameters: [andonTypeId, searchTxt.value],
                                        IsPaging: true
                                    };
                                    Filter = Ext.encode(Filter);
                                    itemView.loadData({
                                        filter: Filter,
                                        action: 'queryer',
                                        type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeTriggerPowerDataQuery",
                                        token: token,
                                        callback: function (records) {
                                            me.leftData = records[0];
                                        }
                                    });
                                }
                            }
                        }
                    },
                    {
                        name: 'employee_searchBtn',
                        text: '查找'.t(),
                        xtype: 'button',
                        handler: function () {
                            var store = this.up("panel").getStore();
                            var itemView = this.up("panel").SIEView;
                            var searchTxt = this.ownerCt.child("[name=employee_searchTxt]");
                            var Filter = {
                                Method: "GetEmployeeAll",
                                Parameters: [andonTypeId, searchTxt.value],
                                IsPaging: true
                            };
                            Filter = Ext.encode(Filter);
                            itemView.loadData({
                                filter: Filter,
                                action: 'queryer',
                                type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeTriggerPowerDataQuery",
                                token: token,
                                callback: function (records) {
                                    me.leftData = records[0];
                                }
                            });
                        }
                    }];
                    var leftView = SIE.AutoUI.createListView(mainBlock);
                    var leftui = leftView.getControl();
                    leftui.flex = 1;
                    var leftFilter = {
                        Method: "GetEmployeeAll",
                        Parameters: [andonTypeId, ""],
                        IsPaging: true
                    };
                    leftFilter = Ext.encode(leftFilter);
                    leftView.loadData({
                        filter: leftFilter,
                        action: 'queryer',
                        type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeTriggerPowerDataQuery",
                        token: token,
                        callback: function (records) {
                            me.leftData = records[0];
                        }
                    });
                    var buttons = Ext.create({
                        xtype: 'panel',
                        layout: 'center',
                        items: [{
                            xtype: 'panel',
                            layout: {
                                type: 'vbox',
                            },
                            items: [{
                                xtype: 'button',
                                text: '>>',
                                style: { margin: '10px' },
                                handler: function () {
                                    var selectList = leftView.getSelectedEntities();
                                    if (selectList.length > 10) {
                                        var msg = '一次选择添加的员工数不能超过10！'.L10N();
                                        SIE.Msg.showMessage(msg);
                                        return
                                    }
                                    var rightStore = rightView.getData();
                                    for (var i = 0; i < selectList.length; i++) {
                                        var index = rightStore.findBy(function (item) {
                                            return selectList[i].data.Code == item.data.Code;
                                        });
                                        if (index >= 0) {
                                            var msg = '员工['.L10N() + selectList[i].data.Code + ']已添加,不允许重复添加!'.L10N();
                                            SIE.Msg.showMessage(msg);
                                            return
                                        }
                                    }
                                    selectList.forEach(function (item) {
                                        addAlter.push(item.data.Code);
                                        var deleteindex = deletAlter.indexOf(item.data.Code);
                                        if (deleteindex != -1) {
                                            deletAlter.splice(deleteindex, 1);
                                        }
                                        leftView.getData().remove(item);
                                        rightView.getData().add(item.data);
                                    });
                                    leftView.unSelectEntities(selectList);
                                }
                            }, {
                                xtype: 'button',
                                text: '<<',
                                style: { margin: '10px' },
                                handler: function () {
                                    var selectList = rightView.getSelectedEntities();
                                    if (selectList.length > 10) {
                                        var msg = '一次选择移除的员工数不能超过10！'.L10N();
                                        SIE.Msg.showMessage(msg);
                                        return
                                    }
                                    selectList.forEach(function (item) {
                                        deletAlter.push(item.data.Code);
                                        var addindex = addAlter.indexOf(item.data.Code);
                                        if (addindex != -1) {
                                            addAlter.splice(addindex, 1);
                                        }
                                        rightView.getData().remove(item);
                                        leftView.getData().add(item);
                                    });
                                    rightView.unSelectEntities(selectList);
                                }
                            }]
                        }]
                    });
                    var panel = Ext.create({
                        xtype: 'panel',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [leftView.getControl(), buttons, rightView.getControl()]
                    });
                    var win = SIE.Window.show({
                        title: "添加员工".t(),
                        width: '60%',
                        height: '60%',
                        items: panel,
                        callback: function (btn) {
                            if (btn == '确定'.t()) {
                                var isSaved = row_data.dirty == true;
                                if (isSaved) {
                                    SIE.Msg.showWarning('主表数据未保存，请先保存后再操作'.t());
                                    return;
                                }
                                SIE.invokeDataQuery({
                                    type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeTriggerPowerDataQuery",
                                    method: "SaveEmployee",
                                    params: [andonTypeId, addAlter, deletAlter],
                                    async: false,
                                    token: token,
                                    callback: function (res) {
                                        if (res.Success) {
                                            CRT.Event.fire("SIE.Andon.Andons.AndonType_refresh");
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }
    },
});