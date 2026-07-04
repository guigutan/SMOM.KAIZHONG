Ext.define("SIE.Web.EMS.EquipLends.Scrpits.EquipLendManageDetailBehavior", {
    onCreated: function (view) {
        var entity = CRT.Context.PageContext.getCurrentRecord();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setNo(params.No);
        }
    },
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        view.mon(entity, "propertyChanged", me._onEntityPropertyChanged, this);
    },
    _onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if (e.property.length > 0) {
            if (e.property == 'LendObject') {
                entity.setLendEnterpriseId(null);
                entity.setLendEmployeeId(null);
                entity.setSupplierId(null);
                entity.setSupplierName(null);
            }
        }
    }
})
