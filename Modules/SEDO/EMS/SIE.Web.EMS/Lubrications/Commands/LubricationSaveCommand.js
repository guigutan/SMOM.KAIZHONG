SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.LubricationSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
   * 是否可执行 
   * @method canExecute 
   * @param {ListLogicalView} view 列表逻辑视图
   * @return {Boolean} 能执行返回true，否则返回false
   */
    canExecute: function (view) {
        var order = view.getCurrent();
        if (!order)
            return false;
        var data = order.data;
        if (data == null) return false;
        return data.EquipAccountId > 0 ;
    },

    execute: function (view) {
        var me = this;
        me.saveLubrications(view);

    },

    saveLubrications: function (view) {
        var me = this;
        var data = view.getCurrent().data;
        var children = view.getChildren();
        if (!this.onValidation(view)) { SIE.MessageBox.showError("信息填写不完整！".L10N()); return; }

        var detailChild = children.first(function (p) { return p.model == "SIE.EMS.Lubrications.LubricationDetail"; });
        var details = [];
        if (detailChild && detailChild.getData() && detailChild.getData().data.items && detailChild.getData().data.items.length > 0) {
            details = detailChild.getData().data.items.select(function (p) { return p.data; });
        }
        data.LubricationDetailList = details;

        me.view.getData().dirty = true;//设置保存
        Ext.MessageBox.show({
            msg: '正在保存数据'.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });

        data.OrderNo = data.No;
        view.execute({
            //withChildren: withChildren,
            data: data,
            success: function (res) {
                Ext.MessageBox.hide();
                view._current.IsSavedMain = true;
                view._current.markSaved();
                view.syncCmdState(view, false);
                me.onSaved(view, res);
            }
        });
    },
    //保存后方法
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showToast('保存成功'.t(), '完成');
        window.setTimeout(function () {
            CRT.Event.fire("SIE.EMS.Lubrications.Lubrication_refresh");
            CRT.Workbench.closeCurrentTab();
        }, 1000);
    },
    //判断添加的属性值是否重复
    isRepeat: function (ary) {
        var me = this;
        var nary = [];
        SIE.each(ary, function (item) {
            Ext.Array.push(nary, item.data);
        })
        nary = nary.sort(me.compare('DefinitionId'));
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