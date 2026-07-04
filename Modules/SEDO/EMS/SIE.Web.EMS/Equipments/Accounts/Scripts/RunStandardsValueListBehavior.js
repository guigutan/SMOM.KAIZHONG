Ext.define('SIE.Web.EMS.Equipments.Accounts.Scripts.RunStandardsValueListBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onDataLoaded: function (view) {
        var me = this;
        me.view = view;
        var entity = view.getData();
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
    },
    onEntityPropertyChanged: function (e) {
        debugger;
        var me = this;
        var entity = me.view.getCurrent();
        var parent = me.view.getParent().getData().data;
        if (e.property === 'StandardType') {
            entity.setStandardUnit(e.value);
            if (e.value === 40 && entity.getLastExecuteDate() && entity.getAmount() > 0) {
                var lastDate = me.addDays(entity.getLastExecuteDate(), entity.getAmount());
                entity.setNextExecuteDate(lastDate);
            }

            if (e.value !== 40) {
                entity.setNextExecuteDate(null);
                entity.setLastExecuteDate(null);
            }
        }
        if (e.property === 'LastExecuteDate' && entity.getStandardType() === 40 && entity.getAmount() > 0) {
            if (e.value === null) {
                entity.setNextExecuteDate(null);
            } else {
                var lastDate = me.addDays(e.value, entity.getAmount());
                entity.setNextExecuteDate(lastDate);
            }
        }
        if (e.property === "Amount" && entity.getStandardType() === 40 && entity.getAmount() > 0 && entity.getLastExecuteDate()) {
            var lastDate = me.addDays(entity.getLastExecuteDate(), entity.getAmount());
            entity.setNextExecuteDate(lastDate);

        }
    },
    //日期加上天数后的新日期.
    addDays: function (date, days) {
        var nd = new Date(date);
        nd = nd.valueOf();
        nd = nd + days * 24 * 60 * 60 * 1000;
        nd = new Date(nd);
        var y = nd.getFullYear();
        var m = nd.getMonth() + 1;
        var d = nd.getDate();
        if (m <= 9) m = "0" + m;
        if (d <= 9) d = "0" + d;
        var cdate = y + "-" + m + "-" + d;
        return cdate;
    }
});