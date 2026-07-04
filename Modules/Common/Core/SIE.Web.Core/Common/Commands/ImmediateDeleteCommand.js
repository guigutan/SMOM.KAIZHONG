SIE.defineCommand('SIE.Web.Core.Common.Commands.ImmediateDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    execute: function (view) {
        var me = this;
        if (view.isListView) {
            var msg = Ext.String.format('你确定删除选择的{0}条数据吗？确定后将直接删除！'.L10N(), view.getSelection().length);

            SIE.Msg.askQuestion(msg, function () {
                    view.removeSelection();
                    var children = view.getChildren();
                    var withChildren = children.length > 0;
                    var store = view.getData();
                    view.execute({
                        withChildren: withChildren,
                        success: function (res) {
                            store.commitChanges();
                            me._viewReload(view);
                            view.setCurrent(null, true);
                        },
                        error: function (res) {
                            store.rejectChanges();
                        }
                    });
            });
        }
    }
})