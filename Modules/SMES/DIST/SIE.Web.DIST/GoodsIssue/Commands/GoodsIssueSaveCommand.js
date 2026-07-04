SIE.defineCommand('SIE.Web.DIST.GoodsIssueSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    execute: function (view, res) {
        var me = this;
        var children = view.getChildren();
        var withChildren = children.length > 0;
        var m = "SIE.Web.Items.ViewModels.PropertyValueViewModel";
        var provalueChild = children.first(function (p) { return p.model == m; });
        var proData = provalueChild.getData().data;
        var proValue = proData.items.select(function (p) { return p.data; });
        if (me.isRepeat(proValue)) {
            SIE.Msg.showWarning("该发料属性的属性值已经存在！".t());
            return false;
        }
        var woId = view.getData().data.Id;
        var proValueItem = view._current[provalueChild.getAssociateKey()];
        me.view.getData().dirty = true;//设置保存
        var validatevalue = true;
        proValue.forEach(function (model) {
            if (model.Values == undefined || model.DefinitionId == undefined) {
                validatevalue = false;
            }
            model.DefinitionValueId = 1;
        });
        if (!validatevalue) {
            SIE.Msg.showWarning("属性值不能为空".t());
            return false;
        }
        view.execute({
            withChildren: false,
            success: function (res) {
                SIE.invokeDataQuery({
                    method: 'SaveGoodIssueProValue',
                    params: [proValue, woId],
                    action: 'queryer',
                    type: 'SIE.Web.DIST.GoodsIssueDataQueryer',
                    token: view.token,
                    success: function (res) {
                        me.onSaved(view);
                    }
                });
            }
        });
    },
    onSaved: function (view, res) {
        var me = this;
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        var current = view.getCurrent();
        current.markSaved();
        me.onSavedMsg(view, res);
        view.syncCmdState(view, false);
    },
    //判断添加的属性值是否重复
    isRepeat: function (ary) {
        var me = this;
        var nary = ary.sort(me.compare('DefinitionId'));
        for (var i = 0; i < ary.length - 1; i++) {
            if (nary[i].DefinitionId == nary[i + 1].DefinitionId) {
                return true;
            }
        }
        return false;
    },
    compare: function (property) {
        return function (a, b) {
            var value1 = a[property];
            var value2 = b[property];
            return value1 - value2;
        }
    }
});