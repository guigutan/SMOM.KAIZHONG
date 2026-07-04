SIE.defineCommand('SIE.Web.MES.WorkOrders.UnionBarcodeCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "条码关联", group: "edit", hierarchy: "条码", iconCls: "icon-Link icon-blue" },
    canExecute: function (listView) {
        if (listView.getSelection() == null || listView.getSelection().length !== 1 || listView.getCurrent() == null) return false;
        var entity = listView.getCurrent();
        var type = entity.getType();
        return type == 2;
    },
    execute: function (listView, source) {
        var me = this;
        var workOrder = listView.getCurrent();
        var productId = workOrder.getProductId();
        var workOrderNo = workOrder.getNo();
        var productName = workOrder.getWorkOrderProductName();
        SIE.invokeDataQuery({
            method: 'GetRetrospectType',
            params: [productId],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            token: listView.token,
            success: function (res) {
                var retrospectType = res.Result;
                if (retrospectType && retrospectType == 1) {
                    SIE.Msg.showWarning(Ext.String.format('工单[{0}]的产品[{1}]是批次类型，不能条码关联！'.t(), workOrderNo, productName));
                }
                else {
                    var id = 'menu_' + 'SIE.Web.MES.WorkOrders.Reworks.WorkOrderUnionBarcode,SIE.Web.MES'.replace(/[.|,]/g, '');
                    CRT.Workbench.addPage({
                        tabId: id,
                        entityType: 'SIE.Web.MES.WorkOrders.Reworks.WorkOrderUnionBarcode',
                        title: me.getEditViewTitle(workOrder),
                        isDetail: true,
                        isNew: true,
                        module: listView.module,
                        params: {
                            token: listView.token,
                            workOrder: {
                                Id: workOrder.getId(),
                                No: workOrder.getNo(),
                                PlanQty: workOrder.getPlanQty(),
                                UseOldSn: workOrder.getUseOldSn()
                            }
                        }
                    });
                }
            }
        });
    }
});