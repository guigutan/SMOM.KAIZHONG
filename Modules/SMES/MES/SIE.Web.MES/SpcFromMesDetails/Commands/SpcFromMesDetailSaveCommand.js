SIE.defineCommand('SIE.Web.MES.SpcFromMesDetails.Commands.SpcFromMesDetailSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

    onSaved: function (view, res) {
        // 调用基类方法
        this.callParent(arguments);

        // 获取父视图并刷新
        var parentView = view.getParent();
        if (parentView) {
            var parentEntity = parentView.getCurrent();
            if (parentEntity) {
                parentView.refreshData(parentEntity.getId());
            }
        }
    }
});




