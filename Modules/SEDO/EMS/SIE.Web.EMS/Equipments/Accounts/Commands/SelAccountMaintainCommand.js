SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.SelAccountMaintainCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelMaintainProjectCommand',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },  
/**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var datas = view.getData();
        if (datas && datas.isDirty())
            return false;

        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.CreateBy !== null;
        }
        return false;
    }, 
});