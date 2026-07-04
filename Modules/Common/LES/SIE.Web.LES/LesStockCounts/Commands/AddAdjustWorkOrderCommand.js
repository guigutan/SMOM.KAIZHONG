SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.AddAdjustWorkOrderCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "新增", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        return true;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            var parentCur = me.view.getParent().getCurrent();
            entity.setDtlId(parentCur.data.DtlId);
            entity.setQty(0);
            me.view.getParent().childDatas.push(entity.data);
           
        }
    },
});