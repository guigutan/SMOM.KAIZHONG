SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.SelUseDepaCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelDeviceDepaCommand',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null;
        }
        return false;
    }
});