SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.EditSuppCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-Edit icon-blue" },
    //executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    //isTabExist: false, //填写报告页签是否已打开
    //tab: null,  //填写报告页签
    canExecute: function (view) {
        var parent = view._parent;

        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        if (parent == null) {

            return false;
        }
        if (parent.getCurrent().getOutDepotState() != 0) {
            return false;
        }
        return true;
    },
});