/*物料扩展属性编辑器*/
Ext.define('SIE.Web.Items.Common.Editors.ItemExtPropSelectEditor', {
    extend: 'Ext.util.Observable',
    alias: 'widget.ItemExtPropSelectEditor',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cur;
        var token;
        var view;
        if (field.readOnly) {
            return;
        }
        if (me.up('form')) {
            if (field.up().ownerCt.SIEView) {
                cur = field.up().ownerCt.SIEView.getCurrent();
                token = field.up().ownerCt.SIEView.token;
                view = field.up().ownerCt.SIEView;
            }
            else {//QueryView
                cur = this.ownerLayout.owner.SIEView._current;
                token = this.ownerLayout.owner.SIEView.token;
            }
        }
        else {
            cur = field.up().context.record;
            if (field.up().context.view.ownerCt.SIEView) {
                token = field.up().context.view.ownerCt.SIEView.token;
                view = field.up().context.view.ownerCt.SIEView;
            }
            else {
                token = field.up('gridpanel').ownerCt.SIEView.token;
                view = field.up('gridpanel').ownerCt.SIEView;
            }
        }
        var isAllRequired = this.config.IsAllRequired;
        var dbField = this.config.DbField;
        var dataQuery = this.config.DataQuery;
        var productBomIdField = this.config.ProductBomIdField
        var productBomId = (productBomIdField != undefined && productBomIdField != "") ? cur.data[productBomIdField] : 0;
        var dataQueryMethod = this.config.DataQueryMethod;
        var sourceProperty = me.name;
        var sourceData = cur.data[sourceProperty];
        var itemId = cur.data[this.config.ItemIdField];
        if (itemId) {
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.Items.Common.DataQuery.ItemExtPropRecordsQueryer",
                method: 'GetItemExtPropRecordsValue',
                token: token,
                params: [itemId, cur.data[dbField], productBomId],
                success: function (res) {
                    if (res.Result != null) {
                        var ui = SIE.Web.Items.ItemExtPropertyAction.CreateCtl(res.Result, sourceData);
                        var win = SIE.Window.show({
                            title: "物料扩展信息".t(),
                            width: 600,
                            height: 400,
                            items: ui,
                            buttons: [
                                {
                                    xtype: "button", text: "重置".t(), handler: function () {
                                        var me = this;
                                        var radio = this.up('window').query('[valueName=radioFieldName]');
                                        radio.forEach(function (p) {
                                            p.setValue(false);
                                        });
                                    }
                                },
                                {
                                    xtype: "button", text: "确定".t(), handler: function () {
                                        var radio = this.up('window').query('[valueName=radioFieldName]').where(function (p) { return p.checked; });
                                        if (isAllRequired) {
                                            var allradio = this.up('window').query('[xtype=radiogroup]');
                                            if (allradio.length != radio.length) {
                                                SIE.Msg.showError("所有选项都必须选择一个！".t());
                                                return;
                                            }
                                        }
                                        var value = "";
                                        var dbValue = "";
                                        radio.forEach(function (p) {
                                            value += p.headName + ":" + p.boxLabel + ";";
                                            dbValue += p.definitionId + ":" + p.boxLabel + ";";
                                        });
                                        if (value != "") {
                                            value = value.substring(0, value.length - 1);
                                        }
                                        if (dbValue != "") {
                                            dbValue = dbValue.substring(0, dbValue.length - 1);
                                        }
                                        cur.set(sourceProperty, value);
                                        if (dbField != null && dbField != "")
                                            cur.set(dbField, dbValue);
                                        if (dataQuery != null && dataQuery != "") {
                                            SIE.invokeDataQuery({
                                                async: false,
                                                type: dataQuery,
                                                method: dataQueryMethod,
                                                token: view.token,
                                                params: [view.getCurrent().data],
                                                success: function (res) {
                                                    if (res.Result) {
                                                        view.reloadData();
                                                    }
                                                }
                                            })
                                        }
                                        this.up('window').close();

                                    }
                                },
                                {
                                    xtype: "button", text: "取消".t(), handler: function () {
                                        this.up('window').close();
                                    }
                                }
                            ],
                        });
                    }
                }
            });
        }
        else {
            //SIE.Msg.showError(Ext.String.format('当前实体没有属性[{0}]，或该属性没有值'.t(), this.config.ItemIdField));
            SIE.Msg.showError("请先维护物料编码信息".t());
        }
    },    
});
