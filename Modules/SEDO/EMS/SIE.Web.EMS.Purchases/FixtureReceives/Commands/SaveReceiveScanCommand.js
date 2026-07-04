SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.SaveReceiveScanCommand', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;

        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
        var snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveSn"; });
        if (!detailChildView  || !snChildView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        var detailList = [];
        SIE.each(detailChildView.getData().data.items, function (model) {
            detailList.push(model.data);
        });
        var snList = [];
        SIE.each(snChildView.getData().data.items, function (model) {
            model.data.CreateDate = null;
            model.data.UpdateDate = null;
            snList.push(model.data);
        });

        //电子签名信息
        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer",
            method: "SaveReceiveScan",
            params: [fromEntity.data, detailList, snList],
            async: false,
            token: view.token,
            logInfo: signdata,
            success: function (res) {
                SIE.Msg.showInstantMessage('保存成功'.t());
                CRT.Workbench.closeCurrentTab();
                CRT.Event.fire("SIE.EMS.Purchases.FixtureReceives.FixtureReceive_refresh");
            }
        });
    }
});