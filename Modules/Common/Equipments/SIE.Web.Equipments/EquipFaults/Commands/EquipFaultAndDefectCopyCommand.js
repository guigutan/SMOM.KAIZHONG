/*
 ** 设备故障与系统缺陷对应关系复制命令
 * @class SIE.Web.Equipments.EquipFaults.Commands.EquipFaultAndDefectCopyCommand
 */
SIE.defineCommand('SIE.Web.Equipments.EquipFaults.Commands.EquipFaultAndDefectCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },

    canExecute: function (view) {
        return view.getCurrent() != null && view.getSelection().length == 1;
    },
    onEditting: function (entity) {
        if (entity) {
            entity.setEquipBadCode(entity.getEquipBadCode() + "复制".t());
            entity.setEquipDefectName(entity.getEquipDefectName() + "复制".t());
        }
    }
});