SIE.defineCommand('SIE.Web.Packages.Boxs.Commands.TurnoverBoxEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
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
        if (e.property === 'Type' && e.value === '生产周转箱') {
            entity.setCapacity(1);
        }
    },
});
