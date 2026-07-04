SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.CallMaterialWoUpCommand', {
    meta: { text: "上移", group: "edit", iconCls: "icon-ArrowLongUp icon-blue" },
    canExecute: function (view) {
        var me = this;
        var curEntity = this.view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        var curData = curEntity.getData();
        var preEntity = this._getPreEntity(view, curData);
        if (preEntity != null) {
            var preData = preEntity.getData();
            if (curData != null && curData.WoState == 0 && preData != null && preData.WoState == 0) { return true; }
        }
        
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var curEntity = this.view.getCurrent();
        var curData = curEntity.getData();
        var preEntity = this._getPreEntity(view, curData);
        if (preEntity != null) {
            preEntity.dirty = true;
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
    _getPreEntity: function (view, curData) {
        var me = this;
        var indexs = [];
        var data = view.getData();
        var items = data.data.items;
        for (var i = 0; i < items.length; i++) {
            var item = items[i].getData();
            if (item.Index < curData.Index)
                indexs.push(i);
        }

        if (indexs.length > 0) {
            var preIndex = indexs.max();
            return items[preIndex];
        }
        
        return null;
    }
});