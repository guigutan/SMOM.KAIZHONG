Ext.define("SIE.Web.CSM.AddressAction", {
    statics: {
        onEntityPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            var data = e.entity.data;
            if (e.property.length > 0) {
                if (e.property == 'Country') {
                    entity.setProvince(null);
                }

                if (e.property == 'Province') {
                    entity.setCity(null);
                }

                if (e.property == 'City') {
                    entity.setArea(null);
                }

                if (e.property == "Area") {
                    entity.setAddress(null);
                }

                if (e.property == "Address") {
                    var fullAddress = data.Country + data.Province + data.City + data.Area + data.Address;
                    entity.setFullAddress(fullAddress)
                }
            }
        }
    }
});