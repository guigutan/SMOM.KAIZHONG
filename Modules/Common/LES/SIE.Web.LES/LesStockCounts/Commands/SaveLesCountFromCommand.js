SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.SaveLesCountFromCommand', {
    meta: { text: "保存", group: "edit", iconCls: "iconfont icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.FormSave',
    canExecute: function (view) {
        if (view.isDetailView) {
            var cur = view.getCurrent();
            /*
            弹出页签主从都是form表单，保存cur.belongsView多次赋值，异步无法确保belongsView为form表单(当前view)
            */
            if (cur.belongsView && cur.belongsView != view) {
                cur.belongsView = view;
            }
            if (cur == null || !cur.isDirty()) {
                return false;
            }

            if (view.getData().isDirty())
                return true;
            //完工-50 关闭-60
            if (cur.data.State == 50 || cur.data.State == 60) {
                return false;
            }
        }
        return this.callParent(arguments);
    },
    /**
        * 视图数据提交保存回调处理
        * @param view 当前视图
        */
    doSave: function (view) {
        var me = this;
        var children = view.getChildren();
        if (children[1].getData().data.items.length > 50) {
            Ext.MessageBox.show({
                msg: '正在保存数据'.t(),
                progressText: '...',
                width: 300,
                wait: {
                    interval: 200
                }
            });
        }
        var withChildren = children.length > 0;
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                me.onSaved(view, res);
            }
        });
    },
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
            method: 'GetLesStockCountState',
            token: me.view.token,
            params: [current.getId()],
            success: function (res) {
                if (res.Result != null) {
                    current.setState(res.Result);
                    current.markSaved();
                }
            }
        });
        CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
        view.getChildren().forEach(function (item) {
            item._curPid = "";
        });
        view.setCurrent(view.getCurrent(), true);//重新刷新store保持当前行数据跟数据库相同
        me.onSavedMsg(view, res);
        if (view.getChildren().length > 1 && view.getChildren()[1].getData().count() > 0)
            view.getChildren()[1].reloadData();

        var rangeView = me.view.findChild("SIE.LES.LesStockCounts.LesStockCountRange");
        if (rangeView) {
            var range = rangeView.getCurrent();
            if (res.Result.LesStockCountRangeList.length > 0) {
                range.setItems(res.Result.LesStockCountRangeList[0].Items);
            }
        }
        view.getData().markSaved();
        view.syncCmdState();
    },
});