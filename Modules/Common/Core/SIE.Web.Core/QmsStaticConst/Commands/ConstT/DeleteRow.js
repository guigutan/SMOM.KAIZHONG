SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.ConstT.DeleteRow', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除行", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.hasSelectedEntities()) {
            var result = true;
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
            var defaultArray = [];//1-100为默认数据，不能删除
            for (var i = 1; i <= 100; i++) {
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
        var msg = Ext.String.format('你确定删除选择的{0}条数据吗？'.L10N(), view.getSelection().length);
        msg += "删除后，需要再次点击保存！".L10N();

        SIE.Msg.askQuestion(msg, function () {
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                //不选中一行，列表的tbar（如果命令过多）会变更位置
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }
        });

    },
});