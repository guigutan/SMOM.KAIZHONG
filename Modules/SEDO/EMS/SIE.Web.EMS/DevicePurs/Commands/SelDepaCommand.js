SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.SelDepaCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelDeviceDepaCommand',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
/**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null;
        }
        return false;
    },
});