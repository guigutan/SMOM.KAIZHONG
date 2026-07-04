Ext.define('SIE.Web.Items.ProductBoms.AlternativeEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cell = field.up();
        var row_data = cell.context.record;
        var token = me.up().up().up().SIEView.token;
        var bomDetailId = row_data.getId();
        if (row_data.phantom == true) {
            SIE.Msg.showError("当前产品BOM明细未保存，请先保存再选择替代料值！".t());
            return false;
        }
        SIE.AutoUI.getMeta({
            model: "SIE.Items.Item",
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: true,
            viewGroup: "AlternativeView",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                //创建右侧试图并初始化数据
                var rightView = SIE.AutoUI.createListView(mainBlock);
                var rightui = rightView.getControl();
                rightui.flex = 1;
                SIE.invokeDataQuery({
                    type: "SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery",
                    method: "GetItemAlternative",
                    params: [bomDetailId],
                    async: false,
                    token: token,
                    callback: function (res) {
                        if (res.Success && res.Result) {
                            res.Result.each(function (item) {
                                var entity = rightView.createNewItem();
                                entity.set('Code', item.data.Code);
                                entity.set('Name', item.data.Name);
                                entity.markSaved();
                            });
                        }
                    }
                });
                //创建左侧试图并出示化数据
                mainBlock.gridConfig.tbar = [{
                    name: 'item_searchTxt',
                    emptyText: '编码'.t(),
                    xtype: 'textfield',
                    listeners: {
                        specialKey: function (field, e) {
                            if (e.getKey() == Ext.EventObject.ENTER) {
                                var store = this.up("panel").getStore();
                                var itemView = this.up("panel").SIEView;
                                var searchTxt = this.ownerCt.child("[name=item_searchTxt]");
                                SIE.invokeDataQuery({
                                    type: 'SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery',
                                    method: "GetAllItems",
                                    params: [searchTxt.value, null],
                                    async: false,
                                    token: token,
                                    callback: function (res) {
                                        if (res.Success && res.Result) {
                                            var control = itemView.getControl();
                                            store = control.getStore();
                                            store.setData(res.Result.data.items);
                                        }
                                    }
                                });
                            }
                        }
                    }
                },
                {
                    name: 'item_searchBtn',
                    text: '查找'.t(),
                    xtype: 'button',
                    handler: function () {
                        var store = this.up("panel").getStore();
                        var itemView = this.up("panel").SIEView;
                        var searchTxt = this.ownerCt.child("[name=item_searchTxt]");
                        SIE.invokeDataQuery({
                            type: 'SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery',
                            method: "GetAllItems",
                            params: [searchTxt.value, null],
                            async: false,
                            token: token,
                            callback: function (res) {
                                if (res.Success && res.Result) {
                                    var control = itemView.getControl();
                                    store = control.getStore();
                                    store.setData(res.Result.data.items);
                                }
                            }
                        });
                    }
                }];
                var leftView = SIE.AutoUI.createListView(mainBlock);
                var leftui = leftView.getControl();
                leftui.flex = 1;
                var leftFilter = {
                    Method: 'GetAllItems',
                    Parameters: [""],
                    IsPaging: true
                };
                leftFilter = Ext.encode(leftFilter);
                leftView.loadData({
                    filter: leftFilter,
                    action: 'queryer',
                    type: 'SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery',
                    token: token,
                    callback: function (records) {
                        me.leftData = records[0];
                    }
                });
                //创建中间按钮
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
                                var rightStore = rightView.getData();
                                for (var i = 0; i < selectList.length; i++) {
                                    var index = rightStore.findBy(function (item) {
                                        return selectList[i].data.Code == item.data.Code;
                                    });
                                    if (index >= 0) {
                                        var msg = Ext.String.format('替代料[{0}]已添加,不允许重复添加!'.L10N(), selectList[i].data.Code);
                                        var msg = '替代料['.L10N() + selectList[i].data.Code+']已添加,不允许重复添加!'.L10N();
                                        SIE.Msg.showMessage(msg);
                                        return;
                                    }
                                };
                                selectList.forEach(function (item) {
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
                                selectList.forEach(function (item) {
                                    rightView.getData().remove(item);
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
                    title: "替代料值选择".t(),
                    width: '60%',
                    height: '65%',
                    items: panel,
                    callback: function (btn) {
                        if (btn == '确定'.t()) {
                            var selectStore = rightView.getData();
                            var indata = [];
                            selectStore.data.items.forEach(function (p) { indata.push(p.getCode()) });
                            SIE.invokeDataQuery({
                                type: "SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery",
                                method: "SaveAlternative",
                                params: [bomDetailId, indata],
                                async: false,
                                token: token,
                                callback: function (res) {
                                    if (res.Success) {
                                        row_data.setAlternative(indata.join(";"));
                                        CRT.Event.fire("SIE.Items.ProductBom_refresh");
                                    }
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});