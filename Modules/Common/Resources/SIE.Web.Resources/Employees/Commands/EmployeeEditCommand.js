SIE.defineCommand('SIE.Web.Resources.Employees.Commands.EmployeeEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        var me = this;
        if (!me.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                isDetail: true,
                ignoreQuery: true,
                model: this.view.model,
                callback: function (meta) {
                    meta.token = me.view.token;
                    me.viewMeta = meta;
                }
            });
        }
        me.viewMeta.mainBlock.formConfig.items.forEach(function (item) {
            if (item.name == "UserId") {
                if (entity.getUserId())
                    item.readOnly = true;
                else
                    item.readOnly = false;
            }
        })
    },
});