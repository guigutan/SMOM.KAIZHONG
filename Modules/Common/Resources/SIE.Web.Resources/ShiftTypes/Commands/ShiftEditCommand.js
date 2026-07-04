SIE.defineCommand('SIE.Web.Resources.ShiftTypes.Commands.ShiftEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            //this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },
    //_onEntityPropertyChanged: function (e) {
    //    if (e.property.length > 0) {
    //        if (e.property.indexOf('BeginTime') >= 0 || e.property.indexOf('EndTime') >= 0) {
    //            if (e.entity.data.BeginTime.toTimeString() > e.entity.data.EndTime.toTimeString()) {
    //                e.entity.setIsOverDay(true);
    //            }
    //            else {
    //                e.entity.setIsOverDay(false);
    //            }
    //        }
    //    }
    //}
});
