SIE.defineCommand('SIE.Web.MES.TeamManagement.RatedItems.Commands.RatedItemEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-TextQuality icon-blue" },  

    onEditting: function (entity) {
        if (entity && entity.data.IsSystem) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },

    _onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property.indexOf('MinScore') >= 0) {
                var data = e.entity.data;
                var entity = e.entity;
                entity.setMaxScore(data.MinScore);
            }
            if (e.property.indexOf('MaxScore') >= 0) {
                var data = e.entity.data;
                var entity = e.entity;
                entity.setMinScore(data.MaxScore);
            }
        }
    }
});