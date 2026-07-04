SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.QuickMaintainPlanCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "一键保养", group: "edit" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null;
        }
        var pCurrent = view.getParent().getCurrent();
        return pCurrent && pCurrent.getExeState() != SIE.EMS.Maintains.Plans.ExeState.Performed
    },
    execute: function (view, source) {
        var me = this;
        var datas = view.getData().data;
        if (datas == null || datas.length <= 0) return;

        datas.items.forEach(function (data) {
            if (data.getMinValue() == null && data.getMaxValue() == null)
                data.setMaintainResult(1);
        });
    }
});