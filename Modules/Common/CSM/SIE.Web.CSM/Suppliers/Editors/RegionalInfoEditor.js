Ext.define('SIE.Web.CSM.Suppliers.Editors.RegionalInfoEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);

    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cur;
        if (me.up('form')) {
            cur = field.up().SIEView.getCurrent();
        }
        else {
            cur = field.up().context.record;
        }
        SIE.AutoUI.getMeta({
            model: 'SIE.Web.CSM.Suppliers.ViewModels.RegionalViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                entity.setCountry(cur.data.Country);
                entity.setProvince(cur.data.Province);
                entity.setCity(cur.data.City);
                entity.setArea(cur.data.Area);
                entity.setAddress(cur.data.Address);
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "区域".t(),
                    width: 550,
                    height: 350,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var region = detailView.getCurrent().data;
                            var addressStr = region.Country + region.Province + region.City + region.Area + region.Address;
                            cur.setCountry(region.Country);
                            cur.setProvince(region.Province);
                            cur.setCity(region.City);
                            cur.setArea(region.Area);
                            cur.setAddress(region.Address);
                            cur.setFullAddress(addressStr);
                        }
                    }
                });
            },
        });
    }
});