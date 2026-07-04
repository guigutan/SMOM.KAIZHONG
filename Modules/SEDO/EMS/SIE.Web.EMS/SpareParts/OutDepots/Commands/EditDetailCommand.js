SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.EditDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-Edit icon-blue" },
    //executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    //isTabExist: false, //填写报告页签是否已打开
    //tab: null,  //填写报告页签
    canExecute: function (view) {

        //需要选中才可使用
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        //获取父视图
        var parent = view._parent;

        //判断父视图及父对象是否为空
        if (parent == null) {
            return false;
        } else if (parent.getCurrent() == null) {
            return false;
        }

        //获取父对象
        var parentCurrent = parent.getCurrent();

        //父对象出库状态为出库不可使用，父对象的相关单据为空时不可使用
        if (parentCurrent.getOutDepotState() != 0) {
            return false;
        }
        else if (parentCurrent.getReleDoc() == null || parentCurrent.getReleDoc() == "")
        {
            return false;
        }
        return true;
    },
});