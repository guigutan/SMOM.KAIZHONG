/*WMS物料扩展属性编辑器通用方法*/
Ext.define('SIE.Web.Items.ItemExtPropertyAction', {
    statics: {
        /**
         * 创建控件
         * @param {any} PropertyValueData
         * @param {any} sourceData
         */
        CreateCtl: function (PropertyValueData, sourceData) {
            var items = [];
            var sourceArray = [];
            if (sourceData && sourceData != "") {
                sourceData.split(';').forEach(function (p) {
                    var vals = p.split(':');
                    sourceArray.push({ Name: vals[0], Value: vals[1] });
                });
            }
            for (var i = 0; i < PropertyValueData.length; i++) {
                var curr = PropertyValueData[i];
                var children = SIE.Web.Items.ItemExtPropertyAction.CreateRadio(curr.Values, curr.Name, curr.DefinitionId, sourceArray);
                items.push({
                    xtype: 'radiogroup',
                    height: 35,
                    fieldLabel: curr.Name,
                    //labelAlign: 'right',
                    labelWidth: 80,
                    style: 'margin-top:5px;',
                    items: children,
                    allowBlank: true,
                });
            }
            var height = 40 * 10;
            return {
                layout: {
                    type: 'vbox',
                },
                style: 'height:600px;',
                fieldStyle: 'height:600px;',
                innerCtStyle: 'height:600px;',
                border: 0,
                items: items,
                listeners: {
                    afterRender: function (c) {
                        document.getElementById(c.id + "-body").style.overflow = "auto";
                    }
                }
            };
        },
        /**
         * 生成radio
         * @param {any} Values
         * @param {any} Name
         * @param {any} DefinitionId
         * @param {any} sourceArray
         */
        CreateRadio: function (Values, Name, DefinitionId, sourceArray) {
            var radios = [];
            var source = sourceArray.first(function (p) {
                return p.Name == Name;
            });
            for (var i = 0; i < Values.length; i++) {
                var text = Values[i].Value;
                var checked = Values[i].IsChecked;
                if (source != null && source != "" && source.Value == text) {
                    checked = true;
                }
                radios.push({
                    tips: text,
                    headName: Name,
                    definitionId: DefinitionId,
                    valueName: 'radioFieldName',
                    boxLabel: text, inputValue: text, checked: checked, style: 'width:100px;overflow:hidden;white-space:nowrap;margin-right:10px;',
                    listeners: {
                        render: function (comp) {
                            Ext.QuickTips.init();
                            Ext.QuickTips.register({
                                target: comp.id,
                                text: comp.tips
                            });
                        },
                    }
                });
            }
            return radios;
        },
    }
});