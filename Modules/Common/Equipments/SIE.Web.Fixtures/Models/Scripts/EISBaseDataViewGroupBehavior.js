Ext.define('SIE.Web.Fixtures.Models.Scripts.EISBaseDataViewGroupBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onDataLoaded: function (view) {
            var me = this;
            var entity = view.getCurrent();
            view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        onEntityPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === 'IsFeeder' && e.value != true) {
                    entity.setSlotType(null);
                }
                if (e.value == true) {
                    if (e.property === 'IsFeeder') {
                        entity.setSlotType(null);
                        entity.setIsFeeder(true);
                        entity.setIsScraper(false);
                        entity.setIsSteelNet(false);

                    }
                    if (e.property === 'IsSteelNet') {
                        entity.setSlotType(null);
                        entity.setIsFeeder(false);
                        entity.setIsScraper(false);
                        entity.setIsSteelNet(true);
                    }
                    if (e.property === 'IsScraper') {
                        entity.setSlotType(null);
                        entity.setIsFeeder(false);
                        entity.setIsScraper(true);
                        entity.setIsSteelNet(false);
                    }
                }
            }
        }
    });


