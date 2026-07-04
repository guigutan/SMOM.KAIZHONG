/**
 * 产品BOM保存命令
 */
SIE.defineCommand('SIE.Web.Items.ProductBoms.Commands.ProductBomSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * 保存后刷新按钮状态
     * @param {any} view 当前视图
     * @param {any} res 
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        view.reloadData();
        if (current != undefined) {
            view.setCurrent(current);
        }
        view.syncCmdState(view, false);
        me.onSavedMsg(view, res);
    }
});