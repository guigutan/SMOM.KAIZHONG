SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.SelAccountCheckCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelCheckProjectCommand',
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

        var entity = view.getParent();
        if (entity.model == "SIE.EMS.Equipments.Accounts.EquipAccountCheckMgt")
            entity = view.getParent().getParent();

        if (entity != null && entity.getCurrent()) {
            return entity.getCurrent().data.CreateBy !== null;
        }
        return false;
    },
});