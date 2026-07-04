SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.D2.DeleteRow', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除行", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.hasSelectedEntities()) {

            var select = view.getSelection();
            var allAdd = true;
            //此次新增加的数据可以删除
            for (var i = 0; i < select.length; i++) {
                if (select[i].isAdd == undefined || select[i].isAdd == false) {
                    allAdd = false;
                }
            }
            if (allAdd)
                return true;

            var sampleQtyArray = view.getSelection().select(function (c) { return c.data.SampleQty; });
            var defaultArray = [];//1-20为默认数据，不能删除
            for (var i = 1; i <= 20; i++) {
                defaultArray.push(i);
            }

            for (var i = 0; i < sampleQtyArray.length; i++) {
                if (Ext.Array.contains(defaultArray, sampleQtyArray[i])) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source 
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        var msg = Ext.String.format('相同[样本数]的数据将一同删除，你确定删除选择的数据吗？'.L10N(), view.getSelection().length);
        msg += "删除后，需要再次点击保存！".L10N();

        SIE.Msg.askQuestion(msg, function () {

            var selection = view.getSelection();
            if (selection.length > 0) {
                var deleteSampleQty = selection.select(function (c) { return c.data.SampleQty; });
                var store = view.getData();
                var deleteData = store.data.items.filter(function (c) { if (Ext.Array.contains(deleteSampleQty, c.data.SampleQty)) { return c; } });
                store.remove(deleteData);

                var data = view.getData().data;
                if (data.length > 0) {
                    //不选中一行，列表的tbar（如果命令过多）会变更位置
                    view.getControl().setSelection(data.items[0]);
                    view.setCurrent(data.items[0], true);
                } else {
                    view.setCurrent(null, true);
                }
            }
           
        });

    },
});