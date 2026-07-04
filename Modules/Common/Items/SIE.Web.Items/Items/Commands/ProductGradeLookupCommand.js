SIE.defineCommand('SIE.Web.Items.Items.Commands.ProductGradeLookupCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Items.Items.ProductGrade' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /** 生成弹出界面Meta */
    getViewMeta: function (source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: me.dataParams.targetClassName,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            viewGroup:"ButtonSelectViewConfig",//设置选择弹窗视图
            callback: function (res) {
                me.getMetacallback(res, source);
            }
        });
    },
    _reloadTargetViewData: function () {
        
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                var itemId = me.view._parent.getCurrent().data.Id;
                var criteria = dialogView._relations[0]._target.getData().data;
                criteria.ItemId = itemId;
                me.mon(store, 'load', me.onLoad, this);
                //if (dialogView._relations[0]) { //存在查询面板时
                //    SIE.invokeDataQuery({
                //        type: "SIE.Web.Items.Items.DataQuery.ItemDataQuery",
                //        method: "GetProductGrades",
                //        params: [criteria, itemId],
                //        async: false,
                //        token: dialogView.token,
                //        callback: function (res) {
                //            if (res.Success) {
                //                var productGrades = res.Result.data;
                //                var store = dialogView.getControl().getStore();
                //                store.setData(productGrades);
                //                dialogView.getControl().setStore(store);
                //            }
                //        }
                //    });
                //}
                //else {
                    dialogView.loadData();
               // }
            }
        }
    },
    save: function (win) {
        var me = this;
        var itemId = me.view._parent.getCurrent().data.Id;
        var selections = me._targetView.getSelection();
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var gradeId = item.getId();
                var code = item.getCode();
                var name = item.getName();
                var describe = item.getDescribe();
                if (me._sourceViewSelectItems.indexOf(gradeId) === -1) {
                    var grade = { ItemId: itemId, Code: code, Name: name, Describe: describe};
                    operationDatas.push(grade);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close(); //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                }
            },
                me._ownerView);
        } else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});