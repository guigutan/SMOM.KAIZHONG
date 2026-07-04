Ext.define("SIE.Web.MES.QTimes.Scripts.QTPushEditor", {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cell = field.up();
        var row_data = cell.context.record;
        var type = row_data.getObjectType();
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                model: "SIE.MES.QTimes.ViewModels.QTPushObjectViewModel",
                ignoreCommands: true,
                isDetail: false,
                ignoreQuery: true,
                viewGroup: "ListView",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    mainBlock.gridConfig.tbar = [{
                        name: 'object_searchTxt',
                        emptyText: '编码或名称'.t(),
                        xtype: 'textfield',
                        listeners: {
                            //回车按钮事件
                            specialKey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    var store = this.up("panel").getStore();
                                    var itemView = this.up("panel").SIEView;
                                    var searchTxt = this.ownerCt.child("[name = object_searchTxt]");
                                    var Filter = {
                                        Method: "GetPushTypeDatas",
                                        Parameters: [type, searchTxt.value],
                                        IsPaging: true
                                    };
                                    Filter = Ext.encode(Filter);
                                    itemView.loadData({
                                        filter: Filter,
                                        action: 'queryer',
                                        type: "SIE.Web.MES.QTimes.DataQueryers.QTDataQueryer",
                                        callback: function (records) {
                                            /*me.leftData = records[0];*/
                                        }
                                    });
                                }
                            }
                        }
                    },
                    {
                        name: 'object_searchBtn',
                        text: '查找'.t(),
                        xtype: 'button',
                        handler: function () {
                            var store = this.up("panel").getStore();
                            var itemView = this.up("panel").SIEView;
                            var searchTxt = this.ownerCt.child("[name=object_searchTxt]");
                            var Filter = {
                                Method: "GetPushTypeDatas",
                                Parameters: [type, searchTxt.value],
                                IsPaging: true
                            };
                            Filter = Ext.encode(Filter);
                            itemView.loadData({
                                filter: Filter,
                                action: 'queryer',
                                type: "SIE.Web.MES.QTimes.DataQueryers.QTDataQueryer",
                                callback: function (records) {
                                    /*me.leftData = records[0];*/
                                }
                            });
                        }
                    }];
                    var leftView = SIE.AutoUI.createListView(mainBlock);
                    var leftui = leftView.getControl();
                    leftui.flex = 1;
                    var leftFilter = {
                        Method: "GetPushTypeDatas",
                        Parameters: [type, ""],
                        IsPaging: true
                    };
                    leftFilter = Ext.encode(leftFilter);
                    leftView.loadData({
                        filter: leftFilter,
                        action: 'queryer',
                        type: "SIE.Web.MES.QTimes.DataQueryers.QTDataQueryer",
                        callback: function (records) {
                            /*me.leftData = records[0];*/
                        }
                    });
                    var panel = Ext.create({
                        xtype: 'panel',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [leftView.getControl()]
                    });
                    var win = SIE.Window.show({
                        title: "添加".t(),
                        width: '33%',
                        height: '55%',
                        items: panel,
                        callback: function (btn) {
                            if (btn == '确定'.t()) {
                                var pushObject = row_data;
                                var item = leftView.getCurrent();
                                pushObject.setObjectId(item.getObjectId());
                                pushObject.setObjectCode(item.getObjectCode());
                                pushObject.setObjectName(item.getObjectName());
                            }
                        }
                    });
                }
            });
        }   
    }
});
