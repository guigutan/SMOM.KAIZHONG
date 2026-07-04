Ext.define('SIE.Web.Items.ProductBoms.ItemPropertyEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cell = field.up();
        var row_data = cell.context.record;//行数据
        var itemProperty = row_data.getItemProperty();//文本框值
        var token = this.up().grid.SIEView.token;
        var bomDetailId = row_data.getId();
        if (row_data.phantom == true) {
            SIE.Msg.showError("当前产品BOM明细未保存，请先保存再选择物料属性值！".t());
            return false;
        }
        SIE.AutoUI.getMeta({
            model: "SIE.Items.ProductBomDetailPropertyValue",
            ignoreCommands: false,
            ignoreQuery: false,
            isReadonly: false,
            viewGroup: "BomPropertyLookupView",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var gridConfig = mainBlock.gridConfig;
                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "物料属性值选择".t(),
                    items: ui,
                    width: 800,
                    height: 450,
                    buttons: ['保存'.t(), '取消'.t()],
                    callback: function (btn) {
                        if (btn == "保存".t()) {
                            var selection = listView.getData().data.items;
                            var itemPropertyList = [];
                            var productBomDetailPropertyValueList = [];
                            for (var i = 0; i < selection.length; i++) {
                                itemPropertyList[i] = selection[i].getDefinitionId_Display();
                                productBomDetailPropertyValueList[i] = selection[i].data;
                            }
                            itemProperty = itemPropertyList.join(";");
                            row_data.setItemProperty(itemProperty);
                            row_data.dirty = true;
                            var indata = {};
                            indata.Data = Ext.encode({ BomDetailId: bomDetailId, ProductBomDetailPropertyValueList: productBomDetailPropertyValueList });
                            SIE.invokeCommand({
                                cmd: "SIE.Web.Items.ProductBoms.Commands.ItemPropertySaveCommand",
                                data: indata,
                                token: token,
                                callback: function (res) {
                                    var res = res;
                                    if (res.Success) {
                                        SIE.Msg.showInstantMessage('保存成功'.t());
                                    } else {
                                        SIE.Msg.showError(res.Message.t());
                                    }
                                }
                            });
                        }
                    }
                });
                var filter = {
                    Method: 'GetItemPropertyProperty',
                    Parameters: [bomDetailId]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    type: 'SIE.Web.Items.ViewModels.PropertyValueDataQueryer',
                    token: token,
                });
            },
        });
    }
});