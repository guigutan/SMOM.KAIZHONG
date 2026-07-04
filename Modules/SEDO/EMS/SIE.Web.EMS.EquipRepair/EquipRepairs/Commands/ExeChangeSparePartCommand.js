SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ExeChangeSparePartCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "更换", group: "edit", iconCls: "icon-Check" },
    selectedItems: [],
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();

            this.selectedItems = listview.getSelection();
            if (this.selectedItems.length === 0)
                return false;

            var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
            var item = parentEntity;
            var employeeIdsArr = [Ext.isEmpty(item.data.RepairMasterId) ? "" : item.data.RepairMasterId.toString()];
            if (!Ext.isEmpty(item.data.RepairEmployeeIds))
                employeeIdsArr = employeeIdsArr.concat(item.data.RepairEmployeeIds.split(','));
            if (item.data.RepairState == 0 || item.data.RepairState == 4 || item.data.RepairState == 5 || item.data.RepairState == 7 || item.data.RepairState == 8 || employeeIdsArr.indexOf(curId) < 0)
                return false;
            return true;
        }
        else
            return false;
    },
    execute: function (view) {
        //if (view.getData().isDirty()) {
        //    SIE.Msg.showWarning('请先保存数据'.t());
        //    return;
        //}

        var datas = [];
        Ext.each(view.getData().data.items, function (data) { datas.push(data.data); });
        view.execute({
            data: datas,
            success: function (res) {
                SIE.Msg.showInstantMessage('备件更换成功'.t());
                view.loadChildData(true);
            }
        });
    }
});
