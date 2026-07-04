SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.ViewLabelCommand", {
    meta: { text: "查看标签", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var asn = view.getCurrent();
        return asn != null && asn.data;
    },
    execute: function (view, source) {
        var asnNo = view.getCurrent().data.No;
        var tabItem = CRT.Workbench.getTabPanel().items.items.first(function (f) { return f.id == 'tab_SIEWMSCommonPackingLabelExSIEWMS'; });
        if (tabItem) {
            if (tabItem.url.indexOf(asnNo) == -1) {
                CRT.Workbench.closeTab(tabItem);
            }
        }

        CRT.Workbench.addPage({
            entityType: "SIE.WMS.Common.PackingLabelEx,SIE.WMS",
            id: "SIE.WMS.Common.PackingLabelExSIEWMS",
            module: "SIE.WMS.Common.PackingLabelEx,SIE.WMS",
            title: "标签查询".t(),
            params: {
                asnNo: asnNo,
            },
        });

    }
})