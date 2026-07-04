Ext.define('SIE.Web.Andon.Andons.Behaviors.AndonManageBehavior', {
    onCreated: function (view) {
        var entity = CRT.Context.PageContext.getCurrentRecord();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setAndonManageCode(params.AndonManageCode);
            entity.setTriggerId(params.TriggerId);
            entity.setTriggerTime(params.TriggerTime);
            entity.setFaultTime(params.FaultTime);
            entity.setWorkGroup(params.WorkGroup);
            entity.setState(params.State);
        }
    },
    onViewReady: function (view) {
        var entity = view.getCurrent();
        this.view = view;
        view.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);

        var itemDetail = view.findChild("SIE.Andon.Andons.AndonManageCallMaterial");
        var tabPanel = itemDetail.getControl().ownerCt.ownerCt;
        if (!entity.data.AskMaterial) {
            tabPanel.hide();
        }
        else {
            tabPanel.show();
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property == 'AndonId') {
            var andonId = e.entity.data.AndonId;
            var entity = e.entity;
            SIE.invokeDataQuery({
                method: "GetLineStopAndAskMaterial",
                params: [andonId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.Andon.Andons.DataQuery.AndonManageDataQuery",
                token: this.view.token,
                success: function (res) {
                    var data = res.Result;
                    entity.setLineStop(data[0]);
                    entity.setAskMaterial(data[1]);
                }
            });
        }
        if (e.property == 'StationId') {
            var stationId = e.entity.data.StationId;
            var entity = e.entity;
            SIE.invokeDataQuery({
                method: "GetMakingWorkOrder",
                params: [stationId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.Andon.Andons.DataQuery.AndonManageDataQuery",
                token: this.view.token,
                success: function (res) {
                    if (res.Result.data.items[0] != null) {
                        var data = res.Result.data.items[0];
                        entity.setWorkOrderId_Display(data.getNo());
                        entity.setWorkOrderId(data.getId());
                    }
                }
            });
        }
        if (e.property == 'WipResourceId' || e.property == 'WorkOrderId' || e.property == 'ProcessId' || e.property == 'AskMaterial') {
            var itemDetail = e.entity.belongsView.findChild("SIE.Andon.Andons.AndonManageCallMaterial");
            var itemDetailData = itemDetail.getData();
            itemDetailData.removeAll();
        }
        if (e.property == 'AskMaterial') {
            var itemDetail = e.entity.belongsView.findChild("SIE.Andon.Andons.AndonManageCallMaterial");
            var tabPanel = itemDetail.getControl().ownerCt.ownerCt;
            if (!e.entity.data.AskMaterial) {
                tabPanel.hide();
            }
            else {
                tabPanel.show();
            }
        }
    }
});
