SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.SaveSparePartReceiveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaving: function (view, res) {

        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
        SIE.each(detailChildView.getData().data.items, function (model) {
            model.data.OutDepotLineNo = model.data.PartOutDepotDetailId_Display;
        });
        return this.callParent(arguments);
    },
    onSaved: function (view, res) {
        this.callParent(arguments);
        CRT.Workbench.closeCurrentTab();
    }
});