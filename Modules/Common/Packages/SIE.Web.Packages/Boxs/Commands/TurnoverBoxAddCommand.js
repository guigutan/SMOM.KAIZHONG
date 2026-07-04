SIE.defineCommand('SIE.Web.Packages.Boxs.Commands.TurnoverBoxAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        entity.mon(entity, 'propertyChanged', me.boxTypeChanged, me);
    },
    /**
        * 周转箱属性变更事件处理
        * @param e 属性     
        */
    boxTypeChanged: function (e) {
        var entity = e.entity;
        var me = this;
        if (e.property.length > 0) {
            if (entity != null) {
                if (e.property == 'Type' && e.value != null) {
                    if (e.value == "配送周转箱") {
                        entity.setCapacity(1);
                        var editor = Ext.ComponentQuery.query('[xtype=BoxCapacityEditor]');
                        if (editor != undefined && editor.length > 0)
                            editor[0].setDisabled(true);
                    }
                    else {
                        var editor = Ext.ComponentQuery.query('[xtype=BoxCapacityEditor]');
                        if (editor != undefined && editor.length > 0)
                            editor[0].setDisabled(false);
                    }
                }
            }
        }
    },
});