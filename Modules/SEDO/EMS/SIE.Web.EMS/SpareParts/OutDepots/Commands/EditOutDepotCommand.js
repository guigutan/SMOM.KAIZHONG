SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.EditOutDepotCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-Edit icon-blue" },
    //executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    //isTabExist: false, //填写报告页签是否已打开
    //tab: null,  //填写报告页签
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        if (view.getCurrent() == null || view.getCurrent().getOutDepotState() != 0) {
            return false;
        }
        return true;
    },
});