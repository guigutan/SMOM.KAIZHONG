SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DelWorkingHourCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        //需要选中才可使用
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        //如果没有父视图
        if (view._parent == null) {
            return false;
        }
        var parentEntity = view.getParent().getCurrent();
        if (parentEntity == null) {
            return false;
        }
        //单据状态为“待维修”、“维修中”、“暂停中”、“待确认”、“待评分”、“已完成”
        if (parentEntity.getRepairState() >= 1 && parentEntity.getRepairState() <= 6) {
            return true;
        }
        //当前系统操作人是单据的维修责任人或拥有派工权限的人
        
        return false;
    },

});