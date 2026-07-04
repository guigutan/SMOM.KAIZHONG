/*SPC动态编辑器*/
Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.Editors.DynamicTypeEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cur;
        if (field.readOnly) {
            return;
        }
        cur = field.up().context.record;
        if (field.up().context.view.ownerCt.SIEView) {
            token = field.up().context.view.ownerCt.SIEView.token;
        }
        var editorConfig = null;
        if (cur.data.FieldProp === 2) {
            editorConfig = {
                allowBlank: true,
                labelAlign: "right",
                labelWidth: 85,
                xtype: "datetimefield"
            };
        } else if (cur.data.FieldProp === 1) {
            editorConfig = {
                allowBlank: true,
                grow: false,
                labelAlign: "right",
                labelWidth: 85,
                maxLength: 200,
                xtype: "textfield"
            };
        } else if (cur.data.FieldProp === 3) {

            editorConfig = {
                allowBlank: true,
                grow: false,
                labelAlign: "right",
                labelWidth: 85,
                maxLength: 200,
                xtype: "numberfield"
            }
        }
        else if (cur.data.FieldProp === 4 || cur.data.FieldProp === 5) {
            var data = [];
            var i = 0;
            var tree = Ext.getCmp("AbnomalRuleTopicTree");
            var nodes = tree.store.query('field', cur.data.LayerColumn, false, true).items;
            if (nodes[0]) {
                var source = nodes[0].getData().comboxSource;
                source.forEach(function (item) {
                    var arry = [];
                    i++;
                    arry.push(i);
                    arry.push(item);
                    data.push(arry);
                });
            }
            var store = Ext.create('Ext.data.ArrayStore', {
                fields: [
                    'value',
                    'text',
                ],
                autoLoad: true,
                data: data
            });
            editorConfig= {
                xtype: 'combobox',
                publishes: 'value',
                displayField: 'text',
                anchor: '-15',
                store: store,
                minChars: 0,
                queryMode: 'local',
                typeAhead: true
                };
        }
        if (editorConfig) {
            var item = Ext.clone(editorConfig);
            var editor = Ext.create(item);
            if (me.value)
                editor.setValue(me.value);

            var win = SIE.Window.show({
                title: "默认值设置".t(),
                minWidth: 100,
                height:120,
                width: 250,
                autoScroll: false,
                items: editor,
                callback: function (btn) {
                    if (btn == "确定".t()) {
                        var value = editor.getValue();
                        if (editor.xtype === 'datetimefield') {
                            value = editor.rawDateText;
                        }
                        if (field.dataIndex === "Value1")
                            cur.setValue1(value);
                        else if (field.dataIndex === "Value2")
                            cur.setValue2(value);
                    }
                }
            });
        }
    }
});
