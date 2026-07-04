SIE.defineCommand('SIE.Web.Resources.ShiftTypes.Commands.ShiftAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            entity.setBeginTime(new Date(new Date(new Date().toDateString()).getTime()));
            entity.setEndTime(new Date(new Date(new Date().toDateString()).getTime() + 24 * 60 * 60 * 1000 - 1));
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },
    _onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            if (e.property.indexOf('BeginTime') >= 0 || e.property.indexOf('EndTime') >= 0) {
                if (e.entity.data.BeginTime.toTimeString() > e.entity.data.EndTime.toTimeString()) {
                    e.entity.setIsOverDay(true);
                }
                else {
                    e.entity.setIsOverDay(false);
                }
            }
        }
    }
});