SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.SaveRunStandardValueCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaving: function (view) {
        //定标类型是“时间周期”上次执行日期必填
        if (view.getCurrent() && view.getCurrent().getStandardType() ===40) {
            {
                if (view.getCurrent().getData().LastExecuteDate === null) {
                    SIE.Msg.showError('定标类型是“时间周期”时,上次执行日期必填!'.t());
                    return false;
                }
                if (view.getCurrent().getData().LeadTime >= view.getCurrent().getData().Amount){
                    SIE.Msg.showError('定标类型是“时间周期”时,预计期须小于周期量!'.t());
                    return false;
                }
                return true;
            }
        } else {
            return true;
        }
        
    }
});