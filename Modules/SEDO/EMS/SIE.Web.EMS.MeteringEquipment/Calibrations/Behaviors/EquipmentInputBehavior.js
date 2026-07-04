Ext.define('SIE.Web.EMS.MeteringEquipment.Calibrations.Behaviors.EquipmentInputBehavior',
    {
        onDataLoaded: function (view) {
            var inspectionDate = new Date();  
            var items = view.getData().data.items;
            for (var i = 0; i < items.length; i++) {
                var record = items[i];
                if (record.get('InspectionDate') === null) {
                    record.set('InspectionDate', inspectionDate);
                }
            }
        }
    })
