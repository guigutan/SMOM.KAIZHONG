SIE.defineCommand('SIE.Web.Items.ProductBoms.Commands.ProductBomDefaultCommand', {
    meta: { text: "设置缺省", group: "business", iconCls: "icon-ListConfig icon-blue" },
    canExecute: function (view) {
        return view._current !== null && !view._current.isNew() && view._current.data.IsDefault === false;
    },

    execute: function (view) {
        var bom = view._current;
        if (!bom)
            return;
        SIE.Msg.askQuestion(Ext.String.format('是否将产品BOM[{0}]设置为缺省?'.t(), bom.getName()), function () {
            SIE.invokeDataQuery({
                method: 'SetDefaultProductBomWithExtProp',
                params: [bom.getProductId(), bom.getId(),bom.getItemExtPropName()],
                action: 'queryer',
                type: 'SIE.Web.Items.ProductBoms.DataQuery.ProductBomDataQuery',
                token: view.token,
                success: function (res) {
                    if (res.Success)
                        view.reloadData();
                }
            });
        });
    }
});