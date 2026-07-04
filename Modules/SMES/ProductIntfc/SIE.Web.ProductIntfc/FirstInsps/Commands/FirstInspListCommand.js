SIE.defineCommand('SIE.Web.ProductIntfc.FirstInsps.Commands.FirstInspListCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit" },
    canExecute: function (view) {
        return view.getCurrent() !== null && view.getSelection().length == 1 && view.getCurrent().data.Parameter != 0;
    },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },

    _onEntityPropertyChanged: function (e) {
        var me = this;
        var data = e.entity;
        if (e.property.length > 0) {
            if (e.property.indexOf('Parameter') >= 0) {
                if (e.entity.getParameter() == 0)
                    e.entity.setIsSelect = true;
            }
        }
    },
});