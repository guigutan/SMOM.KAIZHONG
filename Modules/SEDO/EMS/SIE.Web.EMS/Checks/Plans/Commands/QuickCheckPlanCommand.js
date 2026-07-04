SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.QuickCheckPlanCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "一键点检", group: "edit" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null && (entity.getExeState() == 0 || entity.getExeState() == 4);
        }

        return false;
    },
    execute: function (view, source) {
        var me = this;
        var datas = view.getData().data;
        if (datas == null || datas.length <= 0) return;

        datas.items.forEach(function (data) {
            if (data.getMinValue() == null && data.getMaxValue() == null)
                data.setCheckResult(1);
        });
    }
});