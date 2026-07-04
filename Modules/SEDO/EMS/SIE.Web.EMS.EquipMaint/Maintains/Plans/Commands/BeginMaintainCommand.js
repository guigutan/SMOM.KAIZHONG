SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.BeginMaintainCommand', {
    meta: { text: "保养开始", group: "edit", iconCls: "icon-Play icon-blue" },
    canExecute: function (view) {
        var me = this;
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getWhetherBegin()) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var me = this;
        var entity = view.getCurrent().getData();
        view.execute({
            data: entity,
            callback: function (res) {
                if (res.Success) {
                    SIE.Msg.showMessage("保养开始".t());
                    var _selectBeginTime = view.getCurrent().getSelectBeginTime();
                    var _selectEndTime = view.getCurrent().getSelectEndTime();
                    CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
                    CRT.Event.fire(view.model + '_' + view.getCurrent().getId() + '_refresh', view.getCurrent().getId());
                }
            }
        })
    },
});