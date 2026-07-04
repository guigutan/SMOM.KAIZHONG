SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.CallMaterialWoDownCommand', {
    meta: { text: "下移", group: "edit", iconCls: "icon-ArrowLongDown icon-blue" },
    canExecute: function (view) {
        var me = this;
        var curEntity = this.view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        var curData = curEntity.getData();
        var lastEntity = this._getLastEntity(view);
        if (lastEntity != null) {
            var lastData = lastEntity.getData();
            if (curData != null && curData.WoState == 0 && lastData != null && lastData.Index != curData.Index) { return true; }
        }

        return false;
    },
    execute: function (view, source) {
        var me = this;
        var curEntity = this.view.getCurrent();
        var curData = curEntity.getData();
        var nextEntity = this._getNextEntity(view, curData);
        if (nextEntity != null) {
            nextEntity.dirty = true;
            curEntity.dirty = true;

            view.execute({
                success: function (res) {
                    var errMsg = res.Result;
                    if (errMsg == '操作成功')
                        view.reloadData();
                    else
                        SIE.Msg.showMessage(errMsg);
                }
            });
        }
    },
    _getLastEntity: function (view) {
        var me = this;
        var data = view.getData();
        var items = data.data.items;
        if (items.length > 0) {
            return items[items.length - 1];
        }
        
        return null;
    },
    _getNextEntity: function (view, curData) {
        var me = this;
        var indexs = [];
        var data = view.getData();
        var items = data.data.items;
        for (var i = 0; i < items.length; i++) {
            var item = items[i].getData();
            if (item.Index > curData.Index)
                indexs.push(i);
        }

        if (indexs.length > 0) {
            var nextIndex = indexs.min();
            return items[nextIndex];
        }

        return null;
    }
});