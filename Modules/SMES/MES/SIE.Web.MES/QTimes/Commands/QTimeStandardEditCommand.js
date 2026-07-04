SIE.defineCommand("SIE.Web.MES.QTimes.Commands.QTimeStandardEditCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        // 资源切换显示工厂
        if (e.property == 'WipResourceId') {
            SIE.invokeDataQuery({
                method: "GetFactoryByWipId",
                params: [e.value],
                async: false,
                action: 'queryer',
                type: "SIE.Web.MES.QTimes.DataQueryers.QTDataQueryer",
                token: this.view.token,
                success: function (res) {
                    var current = me.view.getCurrent();
                    current.setFactoryId(res.Result.Id);
                    current.setFactoryId_Display(res.Result.Name);
                }
            });
        }
        // 开始工序切换选择状态
        if (e.property == 'StartProcessId') {
            me.view.getCurrent().setStartState(null);
        }
        // 结束工序切换选择状态
        if (e.property == 'EndProcessId') {
            me.view.getCurrent().setEndState(null);
        }
    }
});
