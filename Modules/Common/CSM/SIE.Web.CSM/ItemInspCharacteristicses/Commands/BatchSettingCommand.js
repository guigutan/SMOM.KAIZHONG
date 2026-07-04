SIE.defineCommand('SIE.Web.CSM.ItemInspCharacteristicses.Commands.BatchSettingCommand', {
    meta: { text: "批量设置", group: "edit" },
    _curPositionItem: null,//当前操作的元素
    canExecute: function (view) {
        var selectionCount = view.getSelection().length;
        if (selectionCount > 1) return true;
        return false;

    },
    execute: function (listView, source) {
        var me = this;
        me._curPositionItem = listView.getCurrent();
        var selections = listView.getSelection();
        var types = new Ext.data.Store({
            fields: ['value', 'text'],
            data: [["null", null], ["Day", '天'.t()], ["Batch", '批'.t()]]
        });
        var setDialog = new Ext.Window({
            title: "批量配置  物料检验特性".t(),
            frame: true,
            modal: true,
            width: 350,
            height: 250,
            minWidth: 150,
            minHeight: 100,
            layout: 'fit',
            plain: true,
            items: [{
                xtype: 'form',
                defaultType: 'checkboxfield',
                fieldDefaults: {
                    labelWidth: 70
                },
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                bodyPadding: 5,
                border: false,

                items: [
                {
                    xtype: "radiofield",
                    id: 'recurringInspection',
                    boxLabel: '物料周期检'.t(),
                    name: 'type',
                    inputValue: 'recurringInspection',
                    checked:true,
                    checkDirty: function () {
                        var recurringInspection = Ext.getCmp('recurringInspection');
                        if (recurringInspection.getValue() == true) {
                            var intervalPeriod = Ext.getCmp('intervalPeriod');
                            var periodType = Ext.getCmp('periodType');
                            intervalPeriod.disabled = false;
                            periodType.disabled = false;
                        }
                    }
                },
                {
                    xtype: "combobox",
                    id: 'periodType',
                    fieldLabel: '周期类型'.t(),
                    name: 'periodType',
                    store: types,
                    queryMode: 'local',
                    displayField: 'text',
                    valueField: 'value',
                    tpl: Ext.create('Ext.XTemplate',
            '<ul class="x-list-plain"><tpl for=".">',
            '<li role="option" class="x-boundlist-item">{text}&nbsp;</li>',
            '</tpl></ul>'
        ),
                    displayTpl: Ext.create('Ext.XTemplate',
                        '<tpl for=".">',
                        '{text}',
                        '</tpl>'
                    ),
                },
                {
                    xtype: "numberfield",
                    minValue: 0,
                    id: 'intervalPeriod',
                    fieldLabel: '间隔周期'.t(),
                    name: 'intervalPeriod',
                    width: 100,
                    allowDecimals: false
                },
                {
                    xtype: "radiofield",
                    id: 'confirmInspection',
                    boxLabel: '确认检'.t(),
                    name: 'type',
                    checkDirty: function () {
                        var confirmInspection = Ext.getCmp('confirmInspection');
                        if (confirmInspection.getValue() == true) {
                            var intervalPeriod = Ext.getCmp('intervalPeriod');
                            var periodType = Ext.getCmp('periodType');
                            intervalPeriod.setValue(null);
                            intervalPeriod.disabled = true;
                            periodType.setValue(null);
                            periodType.disabled = true;
                        }

                    }
                },
                ],
            }],


            buttons: [
                {
                    xtype: "button",
                    text: '确定'.t(),
                    handler: function () {
                        var intervalPeriod = Ext.getCmp('intervalPeriod');
                        var periodType = Ext.getCmp('periodType');
                        var recurringInspection = Ext.getCmp('recurringInspection');
                        var confirmInspection = Ext.getCmp('confirmInspection');
                        if (recurringInspection.getValue() == true && (intervalPeriod.getValue() == null || periodType.getValue() == null)) {
                            Ext.Msg.alert('提示'.t(), "配置为物料周期检时，周期类型、间隔周期均为必填项！".t());
                        } else {
                            var indata = {};
                            var entityIdList = [];
                            for (var i in selections) {
                                if (i < selections.length) {
                                    entityIdList[i] = selections[i].data.Id;
                                }
                            }
                            indata.Data = Ext.encode({
                                EntityIdList: entityIdList,
                                RecurringInspection: recurringInspection.getValue(),
                                ConfirmInspection:confirmInspection.getValue(),
                                IntervalPeriod: intervalPeriod.getValue(),
                                PeriodType: periodType.getValue(),

                            });
                            listView.execute({
                                data: indata,
                                success: function (res) {
                                    setDialog.close();
                                    listView.reloadData();
                                }
                            });
                        }

                    }
                }, {
                    xtype: "button",
                    text: '取消'.t(),
                    handler: function () {
                        this.up("window").close();
                    }
                }
            ]
        });
        setDialog.show();
    },
});