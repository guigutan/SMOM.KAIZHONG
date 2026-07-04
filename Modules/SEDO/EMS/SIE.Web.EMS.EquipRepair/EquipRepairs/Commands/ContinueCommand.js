SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ContinueCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "维修继续", group: "edit", iconCls: "icon-Play icon-blue" },
    selectedItems:[],
    canExecute: function (listview) {

        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            var employeeIdsArr = [Ext.isEmpty(item.data.RepairMasterId) ? "" : item.data.RepairMasterId.toString()];
            if (!Ext.isEmpty(item.data.RepairEmployeeIds))
                employeeIdsArr = employeeIdsArr.concat(item.data.RepairEmployeeIds.split(','));
            if (item.data.RepairState != 6 || employeeIdsArr.indexOf(curId) < 0)
                return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否对此单据恢复维修？".t(),
            function () {
                //提交时，数据设置为脏
                view.getCurrent().dirty = true;
                me.doSave(view);
            });
    },
    onSaved: function (view, res) {
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        SIE.Msg.showInstantMessage('操作成功！'.t());
    }
});