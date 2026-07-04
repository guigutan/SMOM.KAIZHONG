SIE.defineCommand('SIE.Web.MES.WorkOrders.CopyWorkOrderCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", hierarchy: "工单生成", iconCls: "icon-ContentCopy icon-blue" },
    canExecute: function (listView) {
        if (listView == null || listView.getCurrent() == null || listView.getSelection().length > 1) return false;
        return true;
    },
    _setCopyEntity: function (data) {
        data.data.ActuFinishDate = null;
        data.data.ActuStartDate = null;
        data.data.StorageQty = null;
        data.data.ScrapQty = 0;
        data.data.OnlineQty = 0;
        data.data.FinishQty = 0;
        data.data.ParentId = null;
        data.data.ErpWorkOrderNo = '';
        data.data.PlanNo = '';
        data.data.ProcessTechOrderCode = '';
        data.data.BeforeTechOrderCode = '';
        data.data.IsCommonMode = false;
        data.data.IsMainMaterial = false;
        data.data.Proportion = 0;
        data.data.IsPanelWorkOrder = false;
        data.data.PanelWorkOrderId = null;
        data.data.PanelPrintQty = 0;
        data.data.PrepareState = 0;
        data.data.GeneratedQty = 0;
        data.data.WorkOrderMpType = null;
        data.data.InitStorageQty = 0;
        data.data.ApplyWmsStorageQty = 0;
        data.data.WmsStorageQty = 0;
        data.data.ApplyWmsReturnQty = 0;
        data.data.WmsReturnQty = 0;
    },
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: editEntity.getId(),
            isNew: true,
            title: this.getEditViewTitle(editEntity),
            isDetail: true,
            params: {
                token: me.view.token,
                action: 1,
                oldWorkOrderId: me.view.getCurrent().getId()
            }
        });
    }
});