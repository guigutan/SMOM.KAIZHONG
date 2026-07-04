/**
 * ComboBox扩展，修改空值下拉行高问题
 */
Ext.define('SIE.control.KpiXComboBox', {
    extend: 'SIE.control.XComboBox',
    alias: 'widget.kpixcombobox',
    _SelectItems: [],
    queryMode: 'remote',
    defaultListConfig: {
        loadingHeight: 70,
        minWidth: 70,
        maxHeight: 300,
        shadow: 'sides',
        getInnerTpl: function (displayField) {
            return '{[values["' + displayField + '"].trim() == "" ? "&nbsp;": values["' + displayField + '"]]}';
        },
    },
    listeners: {
        // 直接监听 expand 事件
        expand: function (combo, eOpts) {

            var token = combo.up().SIEView.token;
            var store = combo.getStore();
            var code = combo.up().SIEView.getCurrent().getCode();
            if (Ext.isEmpty(code)) {
                store.removeAll();
            }
            else {
                SIE.invokeDataQuery({
                    type: "SIE.Web.WorkBenchCommon.Workbench.KPI.DataQueryer.QuotaTargetSettingDataQueryer",
                    method: "GetQuotaTargetSettingNameDic",
                    params: [code],
                    async: false,
                    token: token,
                    success: function (res) {                     
                        store.removeAll();
                        var newData = [];
                        store.add(res.Result);
                    }
                });
            }
        }
    },
    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        if (me.ischeckbox) {
            me.multiSelect = true;
            me.listConfig = {
                itemTpl: Ext.create('Ext.XTemplate', '<input type=checkbox>{text}'),
                onItemSelect: function (record) {
                    var me = this;
                    var node = me.getNode(record);
                    if (node) {
                        Ext.fly(node).addCls(me.selectedItemCls);
                        var checkboxs = node.getElementsByTagName("input");
                        if (checkboxs != null) {
                            var checkbox = checkboxs[0];
                            checkbox.checked = true;
                            me.up().setValue(me.up()._SelectItems);
                        }
                    }
                },
                listeners: {
                    itemclick: function (view, record, item, index, e, eOpts) {
                        var me = this;
                        var isSelected = view.isSelected(item);
                        var checkboxs = item.getElementsByTagName("input");
                        if (checkboxs != null) {
                            var checkbox = checkboxs[0];
                            if (!isSelected) {
                                checkbox.checked = true;
                                me.up()._SelectItems.push(record.data);
                            } else {
                                checkbox.checked = false;
                                var index = me.up()._SelectItems.indexOf(record.data);
                                me.up()._SelectItems.splice(index, 1);
                            }
                        }
                    },
                    //expand: function () {
                    //	console.log(222);
                    //	this.getStore().load();
                    //}
                }
            }
        }
    },
    setValue: function (value) {
        var me = this;
        me.callParent(arguments);
        if (me.ischeckbox) {
            var entity;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record;
            }
            entity.data[me.name] = me.value;
        }
    }
});