SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.AddDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {      
        var parentResult = this.callParent(arguments);
        if (parentResult == true) {
            if (view.getParent() && view.getParent().getCurrent()) {
                var auditState = view.getParent().getCurrent().getAuditState();
                return auditState == 0 || auditState == 2;//申请单状态=创建 或=已驳回
            }
            return false;
        }
        else
            return false;
    },
    onItemCreated: function (entity) {
        if (entity) {
            //1.0 设置视图属性-设备型号Id（原因：明细中备件的下拉框要根据此字段筛选）
            var parentEquipModelId = this.view.getParent().getCurrent().getEquipModelId();
            entity.setEquipModelId(parentEquipModelId);
        }
    }
});