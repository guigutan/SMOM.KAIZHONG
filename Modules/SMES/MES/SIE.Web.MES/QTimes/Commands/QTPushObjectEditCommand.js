SIE.defineCommand("SIE.Web.MES.QTimes.Commands.QTPushObjectEditCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        // 切换类型清空id、编码、名称
        if (e.property == 'ObjectType') {
            var current = me.view.getCurrent();
            current.setObjectId(0);
            current.setObjectCode(null);
            current.setObjectName(null);
        }
    }
});
