SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.SaveExeMaintainPlanCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.data) {
            if (entity.getExeState() != 0 && entity.getExeState() != 4)
                return false;
            if (!entity.getWhetherBegin()) {
                return false;
            }
        }
        return true;
    },
    onSavedMsg: function (view, res) {
        //SIE.Msg.showInstantMessage('保存成功'.t());
    }
});