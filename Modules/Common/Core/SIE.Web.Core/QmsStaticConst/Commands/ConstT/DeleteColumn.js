SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.ConstT.DeleteColumn', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除列", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var lastFocused = view.getControl().view.lastFocused;
        if (!lastFocused)
            return false;
        var defaultArray = ["0.25", "0.2", "0.15", "0.1", "0.05", "0.025", "0.01", "0.005", "0.003"];//默认数据，不能删除
        var focuseColumnText = lastFocused.column.text;
        if (Ext.Number.from(focuseColumnText, -1) == -1) {
            return false;//排除不是数字类型的列
        }

        if (Ext.Array.contains(defaultArray, focuseColumnText)) {
            return false;
        }
        return true;

    },
    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source 
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        var lastFocused = view.getControl().view.getLastFocused();
        if (!lastFocused)
            return false;
        var msg = Ext.String.format('你确定删除选中列吗？'.t());
        SIE.Msg.askQuestion(msg, function () {
            SIE.Msg.wait('数据删除中，请稍候...'.t());
            var focuseColumnText = lastFocused.column.text;
            var store = view.getData();
            var removeColumn = store.changeColumns.find(function (c) { return c.header == focuseColumnText });
            store.data.items.forEach(function (c) {
                delete c.data[removeColumn.dataIndex];
                c.dirty = true;
            });
            store.changeColumns.remove(removeColumn);
            view.getController().drawGrid(view);
            SIE.Msg.hide();
        });    
    },
});