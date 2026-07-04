SIE.defineCommand('SIE.Web.ESop.EngDocuments.Commands.EngDocDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectList = view.getSelection();
        if (selectList == null) {
            return false;
        }
        
        return selectList.length > 0;
    },
    execute: function (view, source) {
        var me = this;
        if (view.isListView) {
            var isImmediate = view.isImmediate();
            var msg = Ext.String.format('你确定删除选择的{0}条数据吗？'.L10N(), view.getSelection().length);
            if (isImmediate)
                msg += "确定后将直接删除！".L10N();
            else
                msg += "删除后，需要再次点击保存！".L10N();

            SIE.Msg.askQuestion(msg, function () {
                if (isImmediate) {
                    var children = view.getChildren();
                    var withChildren = children.length > 0;
                    var store = view.getData();
                    view.execute({
                        withChildren: withChildren,
                        withIds: true,
                        selectIds: view.getSelectionIds(),
                        success: function (res) {
                            view.removeSelection();
                            store.commitChanges();
                            me._viewReload(view);
                            view.setCurrent(null, true);
                        },
                        error: function (res) {
                            store.rejectChanges();
                        }
                    });
                }
                else {
                    view.removeSelection();
                    view.setCurrent(null, true);
                }
            });
        }
    }
});
