/**
 * 产品BOM属性值保存命令
 */
SIE.defineCommand('SIE.Web.Items.ProductBoms.Commands.BomPropertyValuesSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var result = view.getData().isDirty();
        return result;
    },
    execute: function (view, source) {
        var me = this;
        var proBomId = this.view.getParent().getCurrent().data.Id;
        var proData = this.view.getData().data;
        var proValue = proData.items.select(function (p) { return p.data; });
        var validatevalue = true;
        //验证属性值是否合法
        proValue.forEach(function (model) {
            if (model.BomValue != undefined && model.Values == undefined) { model.Values = model.BomValue.split(';'); }
            if (model.Values == undefined || model.DefinitionId == undefined || (model.DefinitionId != undefined && (model.Values == undefined|| model.Values.length == 0))) {
                validatevalue = false;
            }
            model.DefinitionValueId = 1;
        });
        if (!validatevalue) {
            SIE.Msg.showWarning("属性值不能为空".t());
            return false;
        }
        //单独保存属性值
        SIE.invokeDataQuery({
            method: 'SaveProductBomProValue',
            params: [proValue, proBomId],
            action: 'queryer',
            type: 'SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery',
            token: view.token,
            success: function (res) {
                me.onSaved(view);
            }
        });
    },
    /**
     * 成功保存刷新按钮状态
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var current = view.getCurrent();
        if (current != undefined) {
            current.markSaved();
        }
        else {
            view.reloadData();
        }
        view.syncCmdState(view, false);
    }
});