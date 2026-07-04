SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.FixtureInvDeleteCommand', {
    extend: 'SIE.Web.Core.Common.Commands.ImmediateDeleteCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        //主表盘点状态为【盘点中】时，来源为【盘盈新增】的数据才能删除
        //主表盘点状态为【复盘中】时，来源为【盘盈新增】且初盘结果为空的数据才能删除
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var parent = view._parent.getCurrent();
        if (parent == null) {
            return false;
        }
        if (parent.data.InventoryTaskStatus !== 20 && parent.data.InventoryTaskStatus !== 40) {
            return false;
        }
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.InventoryAssetSource !== 20) {
                res = false;
                return false;
            }
            if (parent.data.InventoryTaskStatus == 40 && model.data.FirstResult !== null) {
                res = false;
                return false;
            }
        });
        return res;
    }
})