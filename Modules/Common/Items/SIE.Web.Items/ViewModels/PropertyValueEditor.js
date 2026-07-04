Ext.define('SIE.Web.Items.ViewModels.PropertyValueEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cell = field.up();
        var row_data = cell.context.record;//行数据
        var itemId = row_data.getItemId();//文本框值
        var definitionId = row_data.getDefinitionId();//属性定义ID
        var token = this.up().grid.SIEView.token;
        var parentListView = this.up().grid.SIEView._parent;
        var values = row_data.data.Values;
        if (!values) row_data.data.Values = [];
        var bomValue = row_data.getBomValue();

        if (definitionId == null || definitionId == 0) {
            SIE.Msg.showWarning('请先选择属性'.t());
            return;
        }
        SIE.AutoUI.getMeta({
            model: "SIE.Items.ItemPropertyValue",
            ignoreCommands: true,
            ignoreQuery: false,
            isReadonly: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                mainBlock.paramer = parentListView;
                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "产品属性选择".t(),
                    items: ui,
                    width: 800,
                    height: 450,
                    Parameters: parentListView,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var selection = listView.getSelection();
                            if (selection.length == 0) {
                                SIE.Msg.showWarning('没有可提交的数据'.t());
                                return false;
                            } else {
                                var proArray = [];
                                for (var i = 0; i < selection.length; i++) {
                                    proArray[i] = selection[i].getValue();
                                }
                                bomValue = proArray.join(";");
                                row_data.setBomValue(bomValue);
                                if (row_data.getData().WoBomValue != undefined) {
                                    row_data.setWoBomValue(bomValue);
                                }
                                row_data.setValue(bomValue);
                                row_data.setItemValue(bomValue);
                                row_data.data.Values = proArray;
                                row_data.dirty = true;
                                listView.paramer.syncCmdState(listView.paramer, true);
                            }
                        }
                    }
                });
                var filter = {
                    Method: 'GetBomValueProperty',
                    Parameters: [itemId, definitionId]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    type: 'SIE.Web.Items.ViewModels.PropertyValueDataQueryer',
                    token: token,
                    callback: function (records, operation, success) {
                        var records = records;
                        var selectd = bomValue.split(";");
                        var selModel = listView.getSelectionModel();
                        for (var i = 0; i < records[0].length; i++) {
                            if (Ext.Array.contains(selectd, records[0][i].getValue())) {
                                selModel.select(records[0][i], true);
                            }
                        }
                    },
                });
            },
        });
    }
});