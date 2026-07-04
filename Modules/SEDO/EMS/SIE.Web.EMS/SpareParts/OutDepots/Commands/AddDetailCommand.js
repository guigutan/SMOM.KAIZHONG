SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.AddDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
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