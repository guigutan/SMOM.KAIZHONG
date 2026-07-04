Ext.define('SIE.Web.EMS.SpareParts.Applys.Behaviors.DetailBehavior',
    {
        onDataLoaded: function (view) {
        },

        _propertyChanged: function (e,view) {
            var me = this;

            var entity = e.entity;
            var property = e.property;
            if (property == "SparePartId") {
                me._setPartDepotCount(view,entity);
            }
            if (property == "SparePartDepotId") {
                me._setPartDepotCount(view,entity);
            }
        },
        /*
         * 设置库存数
         */
        _setPartDepotCount: function (view,entity) {
            var me = this;
            entity.setDepotAmount(null);
            entity.setSparePartSiteId_Display(null);
            entity.setSparePartSiteId(null);
           
            if (entity.getSparePartId() != null && entity.getSparePartDepotId() != null) {
                SIE.invokeDataQuery({
                    method: 'GetPartDepotCount',
                    params: [entity.getSparePartId(), entity.getSparePartDepotId()],               
                    type: 'SIE.Web.EMS.SpareParts.Applys.DataQuerys.SparePartAppDataQuery',
                    token: view.getToken(),
                    success: function (res) {
                        if (res.Success) {
                            entity.setDepotAmount(res.Result.DepotAmount);
                            var sparePartSite = res.Result.SparePartSite;
                            if (Ext.isEmpty(sparePartSite) == false) {
                                entity.setSparePartSiteId_Display(sparePartSite.Name);
                                entity.setSparePartSiteId(sparePartSite.Id);
                            }
                        }
                    }
                });
            }
        }

    });
