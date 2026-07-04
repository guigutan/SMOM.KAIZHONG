SIE.defineCommand('SIE.Web.Resources.ProcessTechs.Commands.ProcessTechEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-TextQuality icon-blue" },

    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },

    _onEntityPropertyChanged: function (e) {

        var me = this;
        var entity = e.entity;
        var processTechTypeId = entity.getProcessTechTypeId();
        if (processTechTypeId) {
            SIE.invokeDataQuery({
                type: "SIE.Web.Resources.ProcessTechs.DataQuery.ProcessTechTypeQueryer",
                method: "GetProcessTechType",
                params: [processTechTypeId],
                async: false,
                token: me.view.token,
                callback: function (res) {
                    if (res.Success && res.Result != null) {
                        var algorithmMarking = res.Result.getData().items[0].getAlgorithmMarking();
                        entity.setAlgorithmMarking(algorithmMarking);
                    }
                },
            });
        }

        if (e.property.length > 0) {
            if (e.property.indexOf('IsScheduling') >= 0) {
                if (entity.getIsScheduling()) {
                    entity.setOffsetTime(null);
                }
                else {
                    entity.setTransferTime(null);
                }
            }
        }
    }
});