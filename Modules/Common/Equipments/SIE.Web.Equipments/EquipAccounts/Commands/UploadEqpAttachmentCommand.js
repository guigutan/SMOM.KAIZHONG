SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.UploadEqpAttachmentCommand', {
    extend: 'SIE.Web.Core.Common.Commands.UploadZipAttachmentCommand',
    meta: { text: "上传", group: "edit", iconCls: "icon-Upload icon-blue" },

    /**
     * 是否可以执行
     * @param {*} view
     * @returns 总是可以执行，
     * 子类可以根据具体情况覆写
     */
    canExecute: function (view) {
        if (view.getParent().getCurrent() != null) {
            return view.getParent().getCurrent().data.CreateBy !== null;
        }
        return false;
    },
    /**
     *重新加载子菜单列表
     * @param {*} listView
     */
    afterSave: function (listView) {
        listView.reloadData();
        //var parent = listView.getParent();
        //parent.refreshData(parent.getCurrent().getId());
        //CRT.Event.fire(parent.model + '_refresh', parent.getCurrent().getId());
    }
});