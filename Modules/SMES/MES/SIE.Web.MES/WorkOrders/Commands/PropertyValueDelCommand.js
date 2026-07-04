/*重写删除，由于框架的删除命令，删除后不会把主实体数据变脏，导致保存按钮不亮*/
SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.WorkOrderDetailDelCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    extend: 'SIE.cmd.Delete',
    canExecute: function (view) {
        if (view.getCurrent() == null) {//选中项
            return false;
        }
        return true;
    },
    setSaveCommandable: function (view) {
        var firParent = view._parent;
        var woParent = firParent;
        if (view.model == "SIE.MES.WorkOrders.WorkOrderProcessBom") view._children.forEach(function (p) {
            p.getControl().getStore().data.clear();
            //此处重新设置Store某种情况会引起原下拉框dom被销毁，下拉报错,获取Store再设置同样的Store好似也没有必要，因此注释掉
            //p.getControl().setStore(p.getControl().getStore());
        });
        if (firParent.model != "SIE.MES.WorkOrders.WorkOrder") {
            var p = view._parent._parent;
            if (p.model == "SIE.MES.WorkOrders.WorkOrder") {
                woParent = view._parent._parent;
            }
        }
        woParent.getData().dirty = true;
        woParent.syncCmdState(woParent, true);
    },
    execute: function (view) {
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
                }
                else {
                    view.removeSelection();
                    var data = view.getData().data;
                    if (data.length > 0) {
                        //不选中一行，列表的tbar（如果命令过多）会变更位置
                        view.getControl().setSelection(data.items[0]);
                        view.setCurrent(data.items[0], true);
                    }
                    else
                        view.setCurrent(null, true);
                }
                me.setSaveCommandable(view);
            });
        }
        else {
            //form view       
        }
    }
});