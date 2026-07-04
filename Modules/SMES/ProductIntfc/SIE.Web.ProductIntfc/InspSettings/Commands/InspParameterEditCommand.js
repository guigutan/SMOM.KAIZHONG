SIE.defineCommand('SIE.Web.ProductIntfc.InspSettings.Commands.InspParameterEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },
   
    _onEntityPropertyChanged: function (e) {
        var me = this;
        var data = e.entity;
        if (e.property.length > 0) {
            if (e.property.indexOf('InspType') >= 0) {
                if (e.entity.getInspType() == 0)
                    e.entity.setProcessType(1);
                else if (e.entity.getInspType() == 1) {
                    e.entity.setProcessType(0);
                    e.entity.setInspDimension(0);
                }
            }

            if (e.property.indexOf('ProcessType') >= 0) {
                if (e.entity.getProcessType() != 2) {
                    e.entity.setProductFamily(null);
                    e.entity.setInspProcess(null);
                }
            }
        }
    },
});